using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace PocketClient.Desktop.UserControls;

public class ListDetailsTemplateSelector : DataTemplateSelector
{
    public DataTemplate? EmptyTemplate
    {
        get; set;
    }

    public DataTemplate? DetailsTemplate
    {
        get; set;
    }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        return item is null ? EmptyTemplate : DetailsTemplate;
    }
}
