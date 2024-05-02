namespace PocketClient.Desktop.Contracts.Services;

public interface IPocketDbService
{
    public Task InitializeAsync();

    public bool IsSyncingData();

    public string GetPocketDbPath();

    public Task SyncItemsAsync(bool fullSync = false, bool force = false);

    public Task ClearDbAsync();
}
