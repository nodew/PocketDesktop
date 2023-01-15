using Microsoft.UI.Xaml.Data;
using PocketClient.Desktop.Helpers;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.Converters;

public class FilterOptionToLocalizedStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        switch (value)
        {
            case PocketItemFilterOption.All:
                return "FilterOption_All/Text".GetLocalized();
            case PocketItemFilterOption.UnArchived:
                return "FilterOption_MyList/Text".GetLocalized();
            case PocketItemFilterOption.Archived:
                return "FilterOption_Archived/Text".GetLocalized();
            case PocketItemFilterOption.Favorited:
                return "FilterOption_Favorited/Text".GetLocalized();
            default:
                return "Unknown";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
