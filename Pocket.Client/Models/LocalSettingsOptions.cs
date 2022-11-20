namespace Pocket.Client.Models;

public class LocalSettingsOptions
{
    public string? ApplicationDataFolder
    {
        get; set;
    }

    public string? LocalSettingsFile
    {
        get; set;
    }

    public string? PocketDbFile
    {
        get; set; 
    }

    public string? ImageCacheFolder
    {
        get; set; 
    }

    public string? PocketConsumerKey
    {
        get; set; 
    }

    public string? OAuthCallbackUri
    {
        get; set; 
    }
}
