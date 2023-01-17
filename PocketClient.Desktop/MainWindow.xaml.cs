using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Helpers;

namespace PocketClient.Desktop;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".Format();
    }

    public async new Task ShowMessageDialogAsync(string content, string title = "")
    {
        if (Content != null)
        {
            var dialog = new DialogBuilder(Content.XamlRoot)
                .AddTitle(title)
                .AddTextMessage(content)
                .SetTheme(App.GetService<IThemeSelectorService>().Theme)
                .Build();

            await dialog.ShowAsync();
        }
    }
}
