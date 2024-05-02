namespace PocketClient.Desktop.Models;

public class LanguageItem(string key, string displayName)
{
    public string Key
    {
        get; set;
    } = key;

    public string DisplayName
    {
        get; set;
    } = displayName;
}
