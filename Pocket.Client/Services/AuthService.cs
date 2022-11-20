using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Pocket.Client.Contracts.Services;
using Pocket.Client.Models;
using Pocket.Core;

namespace Pocket.Client.Services;

public class AuthService : IAuthService
{
    private const string _userNameKey = "Username";
    private const string _userTokenKey = "AccessToken";
    private const string _defaultOAuthCallbackUri = "pocket-desktop-app://oauth-callback";

    private readonly ILocalSettingsService _localSettingsService;
    private readonly PocketClient _pocketClient;
    private readonly LocalSettingsOptions _options;
    private string? _userToken;
    private bool _initialized;
    private bool _isAuthorized;

    public AuthService(
        ILocalSettingsService localSettingsService, 
        PocketClient pocketClient, 
        IOptions<LocalSettingsOptions> options)
    {
        _localSettingsService = localSettingsService;
        _pocketClient = pocketClient;
        _options = options.Value;

        _userToken = string.Empty;
        _isAuthorized = false;
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        if (string.IsNullOrEmpty(_options.PocketConsumerKey))
        {
            throw new Exception("Consumer key isn't configured");
        }
        else
        {
            _pocketClient.SetConsumerKey(_options.PocketConsumerKey);
        }

        _userToken = await _localSettingsService.ReadSettingAsync<string>(_userTokenKey);
        if (!string.IsNullOrEmpty(_userToken))
        {
            _pocketClient.SetAccessToken(_userToken);
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
        var token = await _pocketClient.GetTokenAsync(oauthCallbackUri);
        var pocketURL = new Uri($"https://getpocket.com/auth/authorize?request_token={token}&redirect_uri={Uri.EscapeDataString(oauthCallbackUri)}");

        await Windows.System.Launcher.LaunchUriAsync(pocketURL);
    }

    public async Task AuthorizeAsync()
    {
        var result = await _pocketClient.AuthorizeAsync();
        _pocketClient.SetAccessToken(result.AccessToken);
        await _localSettingsService.SaveSettingAsync(_userTokenKey, result.AccessToken);
        await _localSettingsService.SaveSettingAsync(_userNameKey, result.Username);
        _isAuthorized = true;
    }
}
