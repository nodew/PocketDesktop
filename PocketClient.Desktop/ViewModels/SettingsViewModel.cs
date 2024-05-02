using System.Reflection;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;

using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Helpers;
using PocketClient.Desktop.Models;
using Windows.ApplicationModel;
using Windows.Globalization;

namespace PocketClient.Desktop.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IPocketDbService _pocketDbService;
    private readonly ILogger<SettingsViewModel> _logger;
    private readonly IAuthService _authService;
    private readonly DispatcherQueue dispatcherQueue;

    public readonly List<ThemeItem> SupportedThemes = new()
    {
        new ThemeItem("Light", "Settings_Theme_Light".Format(), ElementTheme.Light),
        new ThemeItem("Dark", "Settings_Theme_Dark".Format(), ElementTheme.Dark),
        new ThemeItem("Default", "Settings_Theme_Default".Format(), ElementTheme.Default),
    };

    public readonly List<LanguageItem> SupportedLanguages = new()
    {
        new LanguageItem("en-US", "English"),
        new LanguageItem("zh-Hans-CN", "简体中文"),
        new LanguageItem("Default", "Settings_Language_Default".Format()),
    };

    [ObservableProperty]
    private ThemeItem selectedTheme;

    [ObservableProperty]
    private LanguageItem selectedLanguage;

    [ObservableProperty]
    private string version;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SyncDataCommand))]
    private bool syncing;

    public SettingsViewModel(
        IThemeSelectorService themeSelectorService,
        IPocketDbService pocketDbService,
        ILogger<SettingsViewModel> logger,
        IAuthService authService)
    {
        _themeSelectorService = themeSelectorService;
        _pocketDbService = pocketDbService;
        _logger = logger;
        _authService = authService;

        selectedLanguage = SupportedLanguages.FirstOrDefault(l => l.Key == GetPreferredLanguage()) ?? SupportedLanguages.Last();
        selectedTheme = SupportedThemes.FirstOrDefault(t => t.Theme == _themeSelectorService.Theme) ?? SupportedThemes.Last();

        version = GetVersion();
        syncing = _pocketDbService.IsSyncingData();
        dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    }

    private static string GetVersion()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    private static string GetPreferredLanguage()
    {
        if (string.IsNullOrWhiteSpace(ApplicationLanguages.PrimaryLanguageOverride))
        {
            return "Default";
        }

        return ApplicationLanguages.PrimaryLanguageOverride;
    }

    [RelayCommand(CanExecute = nameof(CanSwitchTheme))]
    private async Task SwitchTheme(ThemeItem item)
    {
        await _themeSelectorService.SetThemeAsync(item.Theme);
    }

    private bool CanSwitchTheme(ThemeItem item)
    {
        return item != null && item.Theme != _themeSelectorService.Theme;
    }

    [RelayCommand(CanExecute = nameof(CanSwitchLanguage))]
    private void SwitchLanguage(LanguageItem item)
    {
        if (item.Key == "Default")
        {
            ApplicationLanguages.PrimaryLanguageOverride = "";
        }
        else
        {
            ApplicationLanguages.PrimaryLanguageOverride = item.Key;
        }
    }

    private bool CanSwitchLanguage(LanguageItem item)
    {
        return item != null && item.Key != GetPreferredLanguage();
    }

    [RelayCommand(CanExecute = nameof(CanSyncData))]
    private async Task SyncData()
    {
        Syncing = true;

        try
        {
            await _pocketDbService.SyncItemsAsync(fullSync: true, force: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync data in settings page");

            await App.MainWindow.ShowMessageDialogAsync(ex.Message, "Exception_DialogTitle".Format());
        }
        finally
        {
            Syncing = false;
        }
    }

    private bool CanSyncData()
    {
        return !Syncing;
    }

    [RelayCommand]
    private async Task Logout()
    {
        await _authService.LogoutAsync();
    }
}
