using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Pocket.Client.Activation;
using Pocket.Client.Contracts.Services;
using Pocket.Client.Core.Contracts.Services;
using Pocket.Client.Core.Data;
using Pocket.Client.Core.Services;
using Pocket.Client.Models;
using Pocket.Client.Notifications;
using Pocket.Client.Services;
using Pocket.Client.ViewModels;
using Pocket.Client.Views;
using Pocket.Core;
using Windows.ApplicationModel.Activation;

using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace Pocket.Client;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<IWebViewService, WebViewService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<ISampleDataService, SampleDataService>();
            services.AddSingleton<IFileService, FileService>();

            services.AddSingleton<HttpClient>();
            services.AddSingleton<PocketClient>();
            services.AddSingleton<IPocketDbService, PocketDbService>();
            services.AddSingleton((provider) =>
            {
                var dbFilePath = provider.GetService<IPocketDbService>()?.GetPocketDbPath();
                
                var options = new DbContextOptionsBuilder<PocketDbContext>()
                    .UseSqlite($"Data Source={dbFilePath}")
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .Options;

                return new PocketDbContext(options);
            });
            services.AddSingleton<IPocketDataPersistenceService, PocketDataPersistenceService>();
            services.AddSingleton<IPocketDataService, PocketDataService>();
            services.AddSingleton<IAuthService, AuthService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<LoginPage>();
            services.AddTransient<DetailViewModel>();
            services.AddTransient<DetailPage>();
            services.AddTransient<ArchiveViewModel>();
            services.AddTransient<ArchivePage>();
            services.AddTransient<MyListViewModel>();
            services.AddTransient<MyListPage>();
            services.AddTransient<FavoritesViewModel>();
            services.AddTransient<FavoritesPage>();
            services.AddTransient<ArticlesViewModel>();
            services.AddTransient<ArticlesPage>();
            services.AddTransient<VideosViewModel>();
            services.AddTransient<VideosPage>();
            services.AddTransient<AllTagsViewModel>();
            services.AddTransient<AllTagsPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<ShellPage>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        App.GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        
        var mainInstance = AppInstance.FindOrRegisterForKey("pocket-desktop-app-main");

        if (!mainInstance.IsCurrent)
        {
            var activatedEventArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
            await mainInstance.RedirectActivationToAsync(activatedEventArgs);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            return;
        }

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
