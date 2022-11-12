using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Pocket.Client.Core.Models;

namespace Pocket.Client.Views;

public sealed partial class MyListDetailControl : UserControl
{
    public SampleOrder? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as SampleOrder;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(SampleOrder), typeof(MyListDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public MyListDetailControl()
    {
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is MyListDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
