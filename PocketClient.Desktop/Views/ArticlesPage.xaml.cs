// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Desktop.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PocketClient.Desktop.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ArticlesPage : Page
{
    public ArticlesViewModel ViewModel { get; }

    public ArticlesPage()
    {
        ViewModel = App.GetService<ArticlesViewModel>();
        this.InitializeComponent();
    }

    private void OnViewStateChanged(object sender, bool e)
    {
        ViewModel.ShowListAndDetails = e;
        ViewModel.EnsureItemSelected();
    }
}
