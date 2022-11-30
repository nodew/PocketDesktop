using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppLifecycle;
using Pocket.Client.Activation;
using Pocket.Client.Contracts.Services;
using Pocket.Client.Views;
using Windows.ApplicationModel.Activation;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace Pocket.Client.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IPocketDbService _pocketDbService;
    private readonly IAuthService _authService;

    private UIElement? _shell = null;

    public ActivationService(
        ActivationHandler<LaunchActivatedEventArgs> defaultHandler, 
        IEnumerable<IActivationHandler> activationHandlers, 
        IThemeSelectorService themeSelectorService,
        IPocketDbService pocketDbService,
        IAuthService authService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _themeSelectorService = themeSelectorService;
        _pocketDbService = pocketDbService;
        _authService = authService;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await InitializeAsync();

        // Handle auth callback
        var activatedEventArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
        if (activatedEventArgs != null && activatedEventArgs.Kind == ExtendedActivationKind.Protocol)
        {
            var protocolArgs = (ProtocolActivatedEventArgs)activatedEventArgs.Data;
            switch (protocolArgs.Uri.Host)
            {
                case "oauth-callback":
                    try
                    {
                        await _authService.AuthorizeAsync();
                        var instances = AppInstance.GetInstances();
                        foreach(var instance in instances)
                        {
                            if (!instance.IsCurrent)
                            {
                                System.Diagnostics.Process.GetProcessById((int)instance.ProcessId).Kill();
                            }
                        }
                    }
                    catch { }
                    break;
                default:
                    break;
            }
        }

        // Set the MainWindow Content.
        if (App.MainWindow.Content == null)
        {
            UIElement? content;

            if (_authService.IsAuthorized())
            {
                _shell = App.GetService<ShellPage>();
                content = _shell;
            }
            else
            {
                content = App.GetService<LoginPage>();
            }

            App.MainWindow.Content = content ?? new Frame();
        }

        //AppInstance.GetCurrent().Activated += OnActivated;

        // Handle activation via ActivationHandlers.
        await HandleActivationAsync(activationArgs);

        // Activate the MainWindow.
        App.MainWindow.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    //private async void OnActivated(object? sender, AppActivationArguments args)
    //{
    //    if (args.Kind == ExtendedActivationKind.Protocol)
    //    {
    //        var protocolArgs = (ProtocolActivatedEventArgs)args.Data;

    //        switch (protocolArgs.Uri.Host)
    //        {
    //            case "oauth-callback":
    //                try
    //                {
    //                    await App.GetService<IAuthService>().AuthorizeAsync();
    //                    _shell = App.GetService<ShellPage>();
    //                    App.MainWindow.Content = _shell ?? new Frame();
    //                }
    //                catch { }
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //    var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
    //    PInvoke.User32.ShowWindow(hwnd, PInvoke.User32.WindowShowStyle.SW_RESTORE);
    //    PInvoke.User32.SetForegroundWindow(hwnd);
    //}

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await _pocketDbService.InitializeAsync().ConfigureAwait(false);
        await _authService.InitializeAsync().ConfigureAwait(false);

        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        await _themeSelectorService.SetRequestedThemeAsync();
        if (_authService.IsAuthorized())
        {
            await _pocketDbService.SyncItemsAsync(false);
        }
        await Task.CompletedTask;
    }
}
