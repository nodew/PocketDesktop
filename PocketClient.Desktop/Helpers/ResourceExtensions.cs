using Microsoft.Windows.ApplicationModel.Resources;

namespace PocketClient.Desktop.Helpers;

public static class ResourceExtensions
{
    private static readonly ResourceLoader _resourceLoader = new();

    public static string GetLocalized(this string resourceKey) => _resourceLoader.GetString(resourceKey);

    public static string GetLocalized(this string resourceKey, string value)
    {
        var format = _resourceLoader.GetString(resourceKey);
        return string.Format(format, value);
    }

    public static string GetLocalized(this string resourceKey, string value1, string value2)
    {
        var format = _resourceLoader.GetString(resourceKey);
        return string.Format(format, value1, value2);
    }

    public static string GetLocalized(this string resourceKey, string value1, string value2, string value3)
    {
        var format = _resourceLoader.GetString(resourceKey);
        return string.Format(format, value1, value2, value3);
    }
}
