// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Helpers;
using PocketClient.Desktop.Services;
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

    private async void ShowRenameTagDialog(object sender, RoutedEventArgs e)
    {
        var textBox = new TextBox
        {
            Text = ViewModel.CurrentTag!.Name,
            HorizontalAlignment = HorizontalAlignment.Left,
            Width = 400,
        };

        var dialog = new DialogBuilder(this.XamlRoot)
            .AddTitle("TagAction_EditTag".Format())
            .AddContent(new StackPanel()
            {
                Children =
                {
                    textBox,
                    new TextBlock()
                    {
                        Text = "EditTag_ConfirmationMessage".Format(ViewModel.CurrentTag!.Name),
                        Width = 400,
                        Margin = new Thickness(0, 12, 0, 0),
                        TextWrapping = TextWrapping.Wrap
                    },
                }
            })
            .SetTheme(App.GetService<IThemeSelectorService>().Theme)
            .SetCommands("PrimaryButton_Save".Format(), "Button_Cancel".Format())
            .Build();

        textBox.TextChanged += (object sender, TextChangedEventArgs e) =>
        {
            dialog.IsPrimaryButtonEnabled = !string.IsNullOrWhiteSpace(textBox.Text);
        };

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            await ViewModel.RenameTagCommand.ExecuteAsync(textBox.Text);
        }
    }

    private async void ShowDeleteTagConfirmDialog(object sender, RoutedEventArgs e)
    {
        var dialog = new DialogBuilder(this.XamlRoot)
            .AddTitle("TagAction_DeleteTag".Format())
            .AddTextMessage("DeleteTag_ConfirmationMessage".Format(ViewModel.CurrentTag!.Name))
            .SetTheme(App.GetService<IThemeSelectorService>().Theme)
            .SetCommands("PrimaryButton_Yes".Format(), "Button_Cancel".Format())
            .Build();

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            await ViewModel.DeleteTagCommand.ExecuteAsync(null);
        }
    }
}
