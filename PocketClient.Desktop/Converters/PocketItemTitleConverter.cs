using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using PocketClient.Core.Models;

namespace PocketClient.Desktop.Converters;

public class PocketItemTitleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var item = (PocketItem)value;

        if (item == null)
        {
            return string.Empty;
        }

        if (string.IsNullOrEmpty(item.Title))
        {
            return item.Url.ToString();
        }

        return item.Title;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
