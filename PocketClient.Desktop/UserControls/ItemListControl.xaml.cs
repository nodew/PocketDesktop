using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Core.Models;
using CommunityToolkit.WinUI.UI.Controls;

namespace PocketClient.Desktop.UserControls;

public sealed partial class ItemListControl : UserControl
{
    #region DependencyProperties
    public static readonly DependencyProperty ListHeaderProperty =
           DependencyProperty.Register(
              nameof(ListHeader),
              typeof(object),
              typeof(ItemListControl),
              new PropertyMetadata(null));

    public static readonly DependencyProperty ItemsSourceProperty =
           DependencyProperty.Register(
              nameof(ItemsSource),
              typeof(IEnumerable<PocketItem>),
              typeof(ItemListControl),
              new PropertyMetadata(null));

    public static readonly DependencyProperty SelectedItemProperty =
       DependencyProperty.Register(
          nameof(SelectedItem),
          typeof(PocketItem),
          typeof(ItemListControl),
          new PropertyMetadata(null));

    public static readonly DependencyProperty DetailsTemplateProperty =
       DependencyProperty.Register(
          nameof(DetailsTemplate),
          typeof(DataTemplate),
          typeof(ItemListControl),
          new PropertyMetadata(null));
    #endregion

    #region Events
    public event EventHandler<ListDetailsViewState>? ViewStateChanged;
    #endregion

    public ItemListControl()
    {
        this.InitializeComponent();
    }

    #region Properties
    public object ListHeader
    {
        get => GetValue(ListHeaderProperty);
        set => SetValue(ListHeaderProperty, value);
    }

    public IEnumerable<PocketItem> ItemsSource
    {
        get => (IEnumerable<PocketItem>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public PocketItem SelectedItem
    {
        get => (PocketItem)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public DataTemplate DetailsTemplate
    {
        get => (DataTemplate)GetValue(DetailsTemplateProperty); 
        set => SetValue(DetailsTemplateProperty, value);
    }
    #endregion

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        ViewStateChanged?.Invoke(this, e);
    }
}
