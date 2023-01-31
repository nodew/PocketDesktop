using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Core.Models;

namespace PocketClient.Desktop.UserControls;

public sealed partial class TagsControl : UserControl
{
    public static readonly DependencyProperty TagsProperty =
        DependencyProperty.Register(
            nameof(Tags),
            typeof(IEnumerable<Tag>),
            typeof(TagsControl),
            new PropertyMetadata(null));

    public TagsControl()
    {
        this.InitializeComponent();
    }

    public IEnumerable<Tag> Tags
    {
        get => (IEnumerable<Tag>)GetValue(TagsProperty);
        set => SetValue(TagsProperty, value);
    }
}
