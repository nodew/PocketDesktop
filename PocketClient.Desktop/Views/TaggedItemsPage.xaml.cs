// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;
using PocketClient.Desktop.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PocketClient.Desktop.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TaggedItemsPage : ItemsPage<TaggedItemsViewModel>
{
    public TaggedItemsPage(): base()
    {
        this.InitializeComponent();
    }
}
