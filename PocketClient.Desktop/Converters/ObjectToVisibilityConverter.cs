using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace PocketClient.Desktop.Converters;

public class ObjectToVisibilityConverter : IValueConverter
{
    public Visibility NullValue
    {
        get; set;
    }

    public Visibility NonNullValue
    {
        get; set;
    }

    public ObjectToVisibilityConverter()
    {
        NullValue = Visibility.Collapsed;
        NonNullValue = Visibility.Visible;
    }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null)
        {
            return NullValue;
        }
        else
        {
            return NonNullValue;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
