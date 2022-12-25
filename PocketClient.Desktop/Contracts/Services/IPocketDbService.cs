namespace PocketClient.Desktop.Contracts.Services;

public interface IPocketDbService
{
    public Task InitializeAsync();

    public string GetPocketDbPath();

    public Task SyncItemsAsync(bool fullSync = false, bool force = false);
}
