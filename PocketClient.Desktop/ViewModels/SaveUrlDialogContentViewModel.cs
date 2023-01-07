using CommunityToolkit.Mvvm.ComponentModel;

namespace PocketClient.Desktop.ViewModels;

public class SaveUrlDialogContentViewModel : ObservableObject
{
    private string _url;

    public SaveUrlDialogContentViewModel()
    {
        _url = string.Empty;
    }

    public string Url
    {
        get => _url;
        set => SetProperty(ref _url, value);
    }
}
