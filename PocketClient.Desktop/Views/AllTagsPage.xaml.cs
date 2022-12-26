// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.UI.Xaml.Controls;
using PocketClient.Desktop.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PocketClient.Desktop.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AllTagsPage : Page
{
    public AllTagsViewModel ViewModel
    {
        get;
    }

    public AllTagsPage()
    {
        ViewModel = App.GetService<AllTagsViewModel>();
        this.InitializeComponent();
    }

    private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        ViewModel.SearchTagsCommand.Execute(args.QueryText);
    }

    private void OnSelectItem(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var btn = (Button)e.OriginalSource;
        var tag = btn.Tag;

        if (ViewModel.SelectTagCommand.CanExecute(tag))
        {
            ViewModel.SelectTagCommand.Execute(tag);
        }
    }
}
