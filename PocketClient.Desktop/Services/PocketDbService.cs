using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PocketClient.Core.Contracts.Services;
using PocketClient.Core.Data;
using PocketClient.Core.Helpers;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Models;
using PocketClient.HttpSdk;

namespace PocketClient.Desktop.Services;

public class PocketDbService : IPocketDbService
{
    private const string _defaultApplicationDataFolder = "PocketDesktop\\ApplicationData";
    private const string _defaultPocketDbFile = "Pocket.db";
    private const string _pocketLastUpdatedAtKey = "LastUpdatedAt";

    private readonly PocketHttpClient _pocketClient;
    private readonly ILocalSettingsService _localSettingsService;
    private readonly IAppNotificationService _appNotificationService;
    private readonly ILogger<PocketDbService> _logger;

    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string _applicationDataFolder;
    private readonly string _pocketDbFile;

    private bool _initialized;
    private bool _synced;
    private bool _syncing;

    public PocketDbService(
        PocketHttpClient pocketClient,
        ILocalSettingsService localSettingsService,
        IAppNotificationService appNotificationService,
        IOptions<LocalSettingsOptions> options,
        ILogger<PocketDbService> logger)
    {
        _pocketClient = pocketClient;
        _localSettingsService = localSettingsService;
        _appNotificationService = appNotificationService;
        _logger = logger;

        _initialized = false;
        _synced = false;

        var _options = options.Value;

        _applicationDataFolder = Path.Combine(_localApplicationData, _options.ApplicationDataFolder ?? _defaultApplicationDataFolder);
        _pocketDbFile = _options.PocketDbFile ?? _defaultPocketDbFile;
    }

    public async Task InitializeAsync()
    {
        if (!_initialized)
        {
            var path = GetPocketDbPath();
            if (!Directory.Exists(_applicationDataFolder))
            {
                Directory.CreateDirectory(_applicationDataFolder);
            }

            if (!File.Exists(path))
            {
                var dbFile = File.Create(path);
                dbFile.Close();
            }

            await App.GetService<PocketDbContext>().Database.MigrateAsync();

            _initialized = true;
        }
    }

    public string GetPocketDbPath()
    {
        return Path.Combine(_applicationDataFolder, _pocketDbFile);
    }

    public bool IsSyncingData()
    {
        return _syncing;
    }

    public async Task SyncItemsAsync(bool fullSync = false, bool force = false)
    {
        if (!_synced || force)
        {
            _syncing = true;
            var page = 0;
            var pageSize = 50;
            bool hasMoreItems;
            var count = 0;
            DateTimeOffset? lastUpdatedAt = null;

            if (fullSync)
            {
                _logger.LogInformation("Full sync requested");
                await App.GetService<IPocketDataPersistenceService>().ClearDbAsync();
            }
            else
            {
                _logger.LogInformation("Incremental sync requested");
                try
                {
                    lastUpdatedAt = await _localSettingsService.ReadSettingAsync<DateTimeOffset>(_pocketLastUpdatedAtKey);

                    // Indicates that the lastUpdatedAt setting is not set.
                    if (lastUpdatedAt.ToString() == "1/1/0001 12:00:00 AM")
                    {
                        lastUpdatedAt = null;
                    }
                }
                catch { }
            }

            do
            {
                var filter = new PocketItemFilter()
                {
                    State = PocketItemState.All,
                    DetailType = PocketItemDetailType.Complete,
                    SortBy = PocketItemSortMethod.Newest,
                    Since = lastUpdatedAt,
                };

                List<PocketItem> items;

                try
                {
                    items = await _pocketClient.GetItemsAsync(filter, pageSize, page * pageSize);
                }
                catch (Exception ex)
                {
                    _syncing = false;
                    _logger.LogError(ex, "Failed to get items from server");
                    WeakReferenceMessenger.Default.Send(new SyncFailureMessage("Failed to get items from server", ex));
                    return;
                }

                hasMoreItems = items.Count == pageSize;
                count += items.Count;
                page++;

                foreach (var item in items)
                {
                    try
                    {
                        if (item.Status == PocketItemStatus.ShouldDelete)
                        {
                            await App.GetService<IPocketDataPersistenceService>().RemoveItemAsync(item.ItemId);
                        }
                        else
                        {
                            var normalizedItem = PocketItemHelper.NormalizeRawPocketItem(item);
                            await App.GetService<IPocketDataPersistenceService>().AddOrUpdateItemAsync(normalizedItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        // In case of an error, we log the error and continue with the next item.
                        _logger.LogError(ex, "Failed to persist item");
                    }
                }
            } while (hasMoreItems);

            await _localSettingsService.SaveSettingAsync(_pocketLastUpdatedAtKey, DateTimeOffset.Now);

            _syncing = false;
            _synced = true;

            if (count > 0)
            {
                //_appNotificationService.Show($"Pocket Desktop successfully synced {count} new items from server");
                WeakReferenceMessenger.Default.Send(new SyncedItemsMessage());
            }
        }
    }

    public async Task ClearDbAsync()
    {
        await App.GetService<IPocketDataPersistenceService>().ClearDbAsync();
        await _localSettingsService.SaveSettingAsync(_pocketLastUpdatedAtKey, string.Empty);
    }
}
