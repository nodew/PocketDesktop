// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;
using PocketClient.Desktop.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PocketClient.Desktop.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TaggedItemsPage : Page
{
    public TaggedItemsViewModel ViewModel
    {
        get;
    }

    public TaggedItemsPage()
    {
        ViewModel = App.GetService<TaggedItemsViewModel>();
        this.InitializeComponent();
    }

    private void OnViewStateChanged(object sender, bool e)
    {
        ViewModel.ShowListAndDetails = e;
        ViewModel.EnsureItemSelected();
    }
}
