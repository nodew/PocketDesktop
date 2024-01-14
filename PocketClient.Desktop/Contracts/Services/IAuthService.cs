namespace PocketClient.Desktop.Contracts.Services;

public interface IAuthService
{
    public Task InitializeAsync();

    public bool IsAuthorized();

    public Task LaunchAuthorizationAsync();

    public Task AuthorizeAsync();

    public Task LogoutAsync();
}
