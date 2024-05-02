using System.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace PocketClient.Desktop.UserControls;

public sealed partial class ListDetailsView
{
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(ListDetailsView),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ListHeaderProperty =
        DependencyProperty.Register(
            nameof(ListHeader),
            typeof(object),
            typeof(ListDetailsView),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ListHeaderTemplateProperty =
        DependencyProperty.Register(
            nameof(ListHeaderTemplate),
            typeof(DataTemplate),
            typeof(ListDetailsView),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ItemTemplateProperty =
        DependencyProperty.Register(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(ListDetailsView),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ItemTemplateSelectorProperty =
        DependencyProperty.Register(
            nameof(ItemTemplateSelector),
            typeof(DataTemplate),
            typeof(ListDetailsView),
            new PropertyMetadata(null));

    public static readonly DependencyProperty DetailsTemplateProperty =
        DependencyProperty.Register(
            nameof(DetailsTemplate),
            typeof(DataTemplate),
            typeof(ListDetailsView),
            new PropertyMetadata(null));

    public static readonly DependencyProperty NoSelectionContentTemplateProperty =
       DependencyProperty.Register(
          nameof(NoSelectionContentTemplate),
          typeof(DataTemplate),
          typeof(ListDetailsView),
          new PropertyMetadata(null));

    public static readonly DependencyProperty EmptyViewProperty =
       DependencyProperty.Register(
          nameof(EmptyView),
          typeof(ContentControl),
          typeof(ListDetailsView),
          new PropertyMetadata(null));

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(ListDetailsView),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ListPaneWidthProperty = DependencyProperty.Register(
        nameof(ListPaneWidth),
        typeof(double),
        typeof(ListDetailsView),
        new PropertyMetadata(320d));

    public static readonly DependencyProperty CompactModeThresholdWidthProperty = DependencyProperty.Register(
            nameof(CompactModeThresholdWidth),
            typeof(double),
            typeof(ListDetailsView),
            new PropertyMetadata(640d));

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public object ListHeader
    {
        get => GetValue(ListHeaderProperty);
        set => SetValue(ListHeaderProperty, value);
    }

    public DataTemplate ListHeaderTemplate
    {
        get => (DataTemplate)GetValue(ListHeaderTemplateProperty);
        set => SetValue(ListHeaderTemplateProperty, value);
    }

    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public DataTemplateSelector ItemTemplateSelector
    {
        get => (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
        set => SetValue(ItemTemplateSelectorProperty, value);
    }

    public DataTemplate DetailsTemplate
    {
        get => (DataTemplate)GetValue(DetailsTemplateProperty);
        set => SetValue(DetailsTemplateProperty, value);
    }

    public DataTemplate NoSelectionContentTemplate
    {
        get => (DataTemplate)GetValue(NoSelectionContentTemplateProperty);
        set => SetValue(NoSelectionContentTemplateProperty, value);
    }

    public ContentControl EmptyView
    {
        get => (ContentControl)GetValue(EmptyViewProperty);
        set => SetValue(EmptyViewProperty, value);
    }

    public double ListPaneWidth
    {
        get => (double)GetValue(ListPaneWidthProperty);
        set => SetValue(ListPaneWidthProperty, value);
    }

    public double CompactModeThresholdWidth
    {
        get => (double)GetValue(CompactModeThresholdWidthProperty);
        set => SetValue(CompactModeThresholdWidthProperty, value);
    }
}
