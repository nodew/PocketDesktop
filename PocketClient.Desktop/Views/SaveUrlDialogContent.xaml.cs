using System.Text.RegularExpressions;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Desktop.ViewModels;

namespace PocketClient.Desktop.Views;

public sealed partial class SaveUrlDialogContent : Page
{
    private readonly ContentDialog _parentDialog;
    private readonly Regex _urlRegex = new("^(https?:\\/\\/).+\\..+$");

    public readonly SaveUrlDialogContentViewModel ViewModel;


    public SaveUrlDialogContent(ContentDialog dialog)
    {
        _parentDialog = dialog;
        ViewModel = new SaveUrlDialogContentViewModel();

        this.InitializeComponent();
    }

    private void OnUrlChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.Url = UrlTextBox.Text;

        if (_urlRegex.Match(ViewModel.Url).Success)
        {
            _parentDialog.IsPrimaryButtonEnabled = true;
        } 
        else
        {
            _parentDialog.IsPrimaryButtonEnabled = false;
        }
    }
}
