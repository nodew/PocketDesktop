using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Helpers;

using Windows.ApplicationModel;
using Windows.Globalization;

namespace PocketClient.Desktop.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IPocketDbService _pocketDbService;

    private ElementTheme _elementTheme;
    private string _versionDescription;
    private string _language;
    private bool _syncing;

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set => SetProperty(ref _elementTheme, value);
    }

    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    public string Language
    {
        get => _language;
        set => SetProperty(ref _language, value);
    }

    public bool Syncing
    {
        get => _syncing;
        set => SetProperty(ref _syncing, value);
    }

    public ICommand SwitchThemeCommand
    {
        get;
    }

    public ICommand SwitchLanguageCommand
    {
        get;
    }

    public ICommand SyncDataCommand
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService, IPocketDbService pocketDbService)
    {
        _themeSelectorService = themeSelectorService;
        _pocketDbService = pocketDbService;

        _elementTheme = _themeSelectorService.Theme;
        _language = GetPreferredLanguage();
        _versionDescription = GetVersionDescription();
        _syncing = false;

        SwitchThemeCommand = new AsyncRelayCommand<ElementTheme>(SwitchTheme);
        SwitchLanguageCommand = new RelayCommand<string>(SwitchLanguage);
        SyncDataCommand = new AsyncRelayCommand(SyncData);
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".Format()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    private static string GetPreferredLanguage()
    {
        if (string.IsNullOrWhiteSpace(ApplicationLanguages.PrimaryLanguageOverride))
        {
            return "Default";
        }

        return ApplicationLanguages.PrimaryLanguageOverride;
    }

    private async Task SwitchTheme(ElementTheme theme)
    {
        if (ElementTheme != theme)
        {
            ElementTheme = theme;
            await _themeSelectorService.SetThemeAsync(theme);
        }
    }

    private void SwitchLanguage(string? lang)
    {
        if (string.IsNullOrEmpty(lang) || Language == lang)
        {
            return;
        }

        Language = lang;

        if (lang == "Default")
        {
            ApplicationLanguages.PrimaryLanguageOverride = "";
        }
        else
        {
            ApplicationLanguages.PrimaryLanguageOverride = lang;
        }
    }

    private async Task SyncData()
    {
        try
        {
            Syncing = true;
            await _pocketDbService.SyncItemsAsync(fullSync: true, force: true);
        }
        catch (Exception ex)
        {
            // TODO: Log exception to event log
            await App.MainWindow.ShowMessageDialogAsync(ex.Message, "Exception_DialogTitle".Format());
        }
        finally
        {
            Syncing = false;
        }
    }
}
