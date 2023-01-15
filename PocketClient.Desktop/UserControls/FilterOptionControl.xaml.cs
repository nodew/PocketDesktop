using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.UserControls;

public sealed partial class FilterOptionControl : UserControl
{
    #region DependencyProperties
    public static readonly DependencyProperty FilterOptionProperty =
           DependencyProperty.Register(
              nameof(FilterOption),
              typeof(PocketItemFilterOption),
              typeof(ItemListControl),
              new PropertyMetadata(null));

    public static readonly DependencyProperty CommandProperty =
           DependencyProperty.Register(
              nameof(Command),
              typeof(PocketItemFilterOption),
              typeof(ICommand),
              new PropertyMetadata(null));

    public static readonly DependencyProperty HiddenOptionsProperty =
           DependencyProperty.Register(
              nameof(HiddenOptions),
              typeof(PocketItemFilterOption),
              typeof(List<PocketItemFilterOption>),
              new PropertyMetadata(null));
    #endregion

    public PocketItemFilterOption FilterOptionAll => PocketItemFilterOption.All;
    
    public PocketItemFilterOption FilterOptionMyList => PocketItemFilterOption.UnArchived;
    
    public PocketItemFilterOption FilterOptionArchived => PocketItemFilterOption.Archived;

    public PocketItemFilterOption FilterOptionFavorited => PocketItemFilterOption.Favorited;

    public FilterOptionControl()
    {
        this.InitializeComponent();
    }

    public PocketItemFilterOption FilterOption
    {
        get => (PocketItemFilterOption)GetValue(FilterOptionProperty);
        set => SetValue(FilterOptionProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public List<PocketItemFilterOption>? HiddenOptions
    {
        get => (List<PocketItemFilterOption>)GetValue(HiddenOptionsProperty);
        set => SetValue(HiddenOptionsProperty, value);
    }

    public Visibility ShowFilterOption(PocketItemFilterOption bindingOption, PocketItemFilterOption currentOption)
    {
        if (HiddenOptions != null && HiddenOptions.Contains(currentOption))
        {
            return Visibility.Collapsed;
        }

        if (bindingOption == currentOption)
        {
            return Visibility.Collapsed;
        }

        return Visibility.Visible;
    }
}