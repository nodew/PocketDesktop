using Microsoft.UI.Xaml.Controls;

namespace PocketClient.Desktop.UserControls;

/// <summary>
/// Panel that allows for a List/Details pattern.
/// </summary>
/// <seealso cref="ItemsControl" />
public sealed partial class ListDetailsView
{
    /// <summary>
    /// Occurs when the currently selected item changes.
    /// </summary>
    public event SelectionChangedEventHandler? SelectionChanged;

    /// <summary>
    /// Occurs when the view state changes.
    /// </summary>
    public event EventHandler<ListDetailsViewState>? ViewStateChanged;

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectionChanged?.Invoke(this, e);
    }
}
