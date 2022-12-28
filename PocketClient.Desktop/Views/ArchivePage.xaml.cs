﻿using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using PocketClient.Desktop.ViewModels;

namespace PocketClient.Desktop.Views;

public sealed partial class ArchivePage : Page
{
    public ArchiveViewModel ViewModel
    {
        get;
    }

    public ArchivePage()
    {
        ViewModel = App.GetService<ArchiveViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.ShowListAndDetails = true;
        }
        else
        {
            ViewModel.ShowListAndDetails = false;
        }

        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
