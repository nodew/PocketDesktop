using Microsoft.UI.Xaml.Data;

namespace PocketClient.Desktop.Converters;

public class LanguageToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (parameter is string)
        {
            return parameter.Equals(value);
        }
        
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
