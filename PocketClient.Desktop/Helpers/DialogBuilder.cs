using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace PocketClient.Desktop.Helpers;

public class DialogBuilder
{
    private readonly XamlRoot _root;

    private string _title;
    private ElementTheme _theme;
    private object _content;
    private string _primaryButtonLabel;
    private string? _secondaryButtonLabel;
    private string? _closeButtonLabel;

    public DialogBuilder(XamlRoot xmalRoot)
    {
        _root = xmalRoot;
        _title = string.Empty;
        _content = string.Empty;
        _theme = ElementTheme.Default;
        _primaryButtonLabel = "PrimaryButton_OK".Format();
    }

    public DialogBuilder AddTitle(string title)
    {
        _title = title;
        return this;
    }

    public DialogBuilder AddTextMessage(string text)
    {
        _content = new TextBlock { Text = text, TextWrapping = TextWrapping.Wrap };
        return this;
    }

    public DialogBuilder AddContent(object content)
    {
        _content = content;
        return this;
    }

    public DialogBuilder SetTheme(ElementTheme theme)
    {
        _theme = theme;
        return this;
    }

    public DialogBuilder SetCommands(string primary, string? secondary = null, string? close = null)
    {
        _primaryButtonLabel = primary;
        _secondaryButtonLabel = secondary;
        _closeButtonLabel = close;
        return this;
    }

    public ContentDialog Build()
    {
        var dialog = new ContentDialog
        {
            XamlRoot = _root,
            Title = _title,
            Content = _content,
            RequestedTheme = _theme,
            PrimaryButtonText = _primaryButtonLabel,
            SecondaryButtonText = _secondaryButtonLabel,
            CloseButtonText = _closeButtonLabel,
            DefaultButton = ContentDialogButton.Primary
        };

        return dialog;
    }
}
