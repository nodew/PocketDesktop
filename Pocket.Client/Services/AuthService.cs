using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Pocket.Client.Contracts.Services;
using Pocket.Client.Models;
using Pocket.Core;

namespace Pocket.Client.Services;

public class AuthService : IAuthService
{
    private const string _userNameKey = "Username";
    private const string _userAccessTokenKey = "AccessToken";
    private const string _userRequestTokenKey = "RequestToken";
    private const string _defaultOAuthCallbackUri = "pocket-desktop-app://oauth-callback";

    private readonly ILocalSettingsService _localSettingsService;
    private readonly PocketClient _pocketClient;
    private readonly LocalSettingsOptions _options;
    private string? _userAccessToken;
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
            _pocketClient.SetConsumerKey(_options.PocketConsumerKey);
        }

        _userAccessToken = await _localSettingsService.ReadSettingAsync<string>(_userAccessTokenKey);
        if (!string.IsNullOrEmpty(_userAccessToken))
        {
            _pocketClient.SetAccessToken(_userAccessToken);
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
        var result = await _pocketClient.AuthorizeAsync(token);
        _pocketClient.SetAccessToken(result.AccessToken);
        await _localSettingsService.SaveSettingAsync(_userAccessTokenKey, result.AccessToken);
        await _localSettingsService.SaveSettingAsync(_userNameKey, result.Username);
        _isAuthorized = true;
    }
}
