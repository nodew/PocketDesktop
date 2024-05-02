using Microsoft.UI.Xaml;

namespace PocketClient.Desktop.Models;

public class ThemeItem(string key, string displayName, ElementTheme theme)
{
    public string Key
    {
        get; set;
    } = key;

    public string DisplayName
    {
        get; set;
    } = displayName;

    public ElementTheme Theme
    {
        get; set;
    } = theme;
}
