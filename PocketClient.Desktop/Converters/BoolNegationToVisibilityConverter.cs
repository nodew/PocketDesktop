using CommunityToolkit.WinUI.UI.Converters;
using Microsoft.UI.Xaml;

namespace PocketClient.Desktop.Converters;

public class BoolNegationToVisibilityConverter : BoolToObjectConverter
{
    public BoolNegationToVisibilityConverter()
    {
        TrueValue = Visibility.Collapsed;
        FalseValue = Visibility.Visible;
    }
}

