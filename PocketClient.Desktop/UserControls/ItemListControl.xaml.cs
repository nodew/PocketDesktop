using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Core.Models;

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

    public static readonly DependencyProperty ListHeaderTemplateProperty =
           DependencyProperty.Register(
              nameof(ListHeaderTemplate),
              typeof(DataTemplate),
              typeof(ItemListControl),
              new PropertyMetadata(null));

    public static readonly DependencyProperty ItemsSourceProperty =
           DependencyProperty.Register(
              nameof(ItemsSource),
              typeof(ICollection<PocketItem>),
              typeof(ItemListControl),
              new PropertyMetadata(null));
    
    public static readonly DependencyProperty HasItemsProperty =
       DependencyProperty.Register(
          nameof(HasItems),
          typeof(bool),
          typeof(ItemListControl),
          new PropertyMetadata(true));

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
    public event EventHandler<bool>? ViewStateChanged;
    #endregion

    private bool? _showListAndDetails = null;

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

    public DataTemplate ListHeaderTemplate
    {
        get => (DataTemplate)GetValue(ListHeaderTemplateProperty);
        set => SetValue(ListHeaderTemplateProperty, value);
    }

    public ICollection<PocketItem> ItemsSource
    {
        get => (ICollection<PocketItem>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public bool HasItems
    {
        get => (bool)GetValue(HasItemsProperty);
        set => SetValue(HasItemsProperty, value);
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

    private void OnListDetailsViewSizeChanged(object sender, SizeChangedEventArgs e)
    {
        var newState = e.NewSize.Width > 641;

        if (_showListAndDetails != newState)
        {
            _showListAndDetails = newState;
            ViewStateChanged?.Invoke(this, newState);
        }
    }
}
