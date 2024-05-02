// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Desktop.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PocketClient.Desktop.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ManageTagsDialogContent : Page
{
    public readonly ManageTagsDialogContentViewModel ViewModel;

    public ManageTagsDialogContent(ManageTagsDialogContentViewModel viewModel)
    {
        ViewModel = viewModel;

        this.InitializeComponent();
    }

    private void AddNewTag(TokenizingTextBox sender, TokenItemAddingEventArgs args)
    {
        if (ViewModel.AddNewTagCommand.CanExecute(args.TokenText))
        {
            ViewModel.AddNewTagCommand.Execute(args.TokenText);
        }
    }

    private void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            ViewModel.UpdateSuggestedTagsCommand.Execute(TagsTokenBox.Text);
        }
    }
}
