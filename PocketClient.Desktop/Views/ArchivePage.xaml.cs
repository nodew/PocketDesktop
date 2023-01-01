using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using PocketClient.Desktop.ViewModels;

namespace PocketClient.Desktop.Views;

public sealed partial class ArchivePage : ItemsPage<ArchiveViewModel>
{
    public ArchivePage() : base()
    {
        InitializeComponent();
    }
}
