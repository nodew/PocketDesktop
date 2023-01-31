using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace PocketClient.Desktop.Converters;

public class PositiveNumberToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null)
        {
            return Visibility.Collapsed;
        }

        if (int.TryParse(value.ToString(), out var n))
        {
            if (n > 0)
            {
                return Visibility.Visible;
            }
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
