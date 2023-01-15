using Microsoft.Windows.ApplicationModel.Resources;

namespace PocketClient.Desktop.Helpers;

public static class Localization
{
    private static readonly ResourceLoader _resourceLoader = new();

    public static string Format(this string resourceKey) => _resourceLoader.GetString(resourceKey);

    public static string Format(this string resourceKey, object value)
    {
        var format = _resourceLoader.GetString(resourceKey);
        return string.Format(format, value);
    }

    public static string Format(this string resourceKey, object value1, object value2)
    {
        var format = _resourceLoader.GetString(resourceKey);
        return string.Format(format, value1, value2);
    }

    public static string Format(this string resourceKey, object value1, object value2, object value3)
    {
        var format = _resourceLoader.GetString(resourceKey);
        return string.Format(format, value1, value2, value3);
    }
}
