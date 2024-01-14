using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Models;
using PocketClient.HttpSdk;

namespace PocketClient.Desktop.Services;

public class AuthService : IAuthService
{
    private const string _userNameKey = "Username";
    private const string _userAccessTokenKey = "AccessToken";
    private const string _userRequestTokenKey = "RequestToken";
    private const string _defaultOAuthCallbackUri = "pocket-desktop-app://oauth-callback";

    private readonly ILocalSettingsService _localSettingsService;
    private readonly PocketHttpClient _pocketHttpClient;
    private readonly LocalSettingsOptions _options;
    private string? _userAccessToken;
    private bool _initialized;
    private bool _isAuthorized;

    public AuthService(
        ILocalSettingsService localSettingsService,
        PocketHttpClient pocketHttpClient,
        IOptions<LocalSettingsOptions> options)
    {
        _localSettingsService = localSettingsService;
        _pocketHttpClient = pocketHttpClient;
        _options = options.Value;

        _userAccessToken = string.Empty;
        _isAuthorized = false;
    }

    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        if (string.IsNullOrEmpty(_options.PocketConsumerKey))
        {
            throw new Exception("Consumer key isn't configured");
        }
        else
        {
            _pocketHttpClient.SetConsumerKey(_options.PocketConsumerKey);
        }

        _userAccessToken = await _localSettingsService.ReadSettingAsync<string>(_userAccessTokenKey);
        if (!string.IsNullOrEmpty(_userAccessToken))
        {
            _pocketHttpClient.SetAccessToken(_userAccessToken);
            _isAuthorized = true;
        }

        _initialized = true;

    }

    public bool IsAuthorized()
    {
        return _isAuthorized;
    }

    public async Task LaunchAuthorizationAsync()
    {
        var oauthCallbackUri = _options.OAuthCallbackUri ?? _defaultOAuthCallbackUri;
        var token = await _pocketHttpClient.GetTokenAsync(oauthCallbackUri);

        if (token == null)
        {
            throw new Exception("Token is null");
        }

        await _localSettingsService.SaveSettingAsync(_userRequestTokenKey, token);

        var pocketURL = new Uri($"https://getpocket.com/auth/authorize?request_token={token}&redirect_uri={Uri.EscapeDataString(oauthCallbackUri)}");

        await Windows.System.Launcher.LaunchUriAsync(pocketURL);
    }

    public async Task AuthorizeAsync()
    {
        var token = await _localSettingsService.ReadSettingAsync<string>(_userRequestTokenKey);
        var result = await _pocketHttpClient.AuthorizeAsync(token);
        _pocketHttpClient.SetAccessToken(result.AccessToken);
        await _localSettingsService.SaveSettingAsync(_userAccessTokenKey, result.AccessToken);
        await _localSettingsService.SaveSettingAsync(_userNameKey, result.Username);
        _isAuthorized = true;
    }

    public async Task LogoutAsync()
    {
        await _localSettingsService.SaveSettingAsync(_userAccessTokenKey, string.Empty);
        await _localSettingsService.SaveSettingAsync(_userNameKey, string.Empty);
        _isAuthorized = false;

        WeakReferenceMessenger.Default.Send(new UserLoggedOutMessage());
    }
}
