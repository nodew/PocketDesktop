using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using PocketClient.Desktop.Activation;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Core.Contracts.Services;
using PocketClient.Core.Data;
using PocketClient.Desktop.Services;
using PocketClient.Desktop.Models;
using PocketClient.Desktop.Notifications;
using PocketClient.Core.Services;
using PocketClient.Desktop.ViewModels;
using PocketClient.Desktop.Views;
using PocketClient.HttpSdk;

using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace PocketClient.Desktop;

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

    public static MainWindow MainWindow { get; } = new MainWindow();

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
            services.AddSingleton<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            services.AddSingleton<HttpClient>();
            services.AddSingleton<PocketHttpClient>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IPocketDbService, PocketDbService>();

            services.AddTransient((provider) =>
            {
                var dbFilePath = provider.GetService<IPocketDbService>()?.GetPocketDbPath();

                var options = new DbContextOptionsBuilder<PocketDbContext>()
                    .UseSqlite($"Data Source={dbFilePath}")
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .Options;

                return new PocketDbContext(options);
            });
            services.AddTransient<IPocketDataPersistenceService, PocketDataPersistenceService>();
            services.AddTransient<IPocketDataService, PocketDataService>();

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
            services.AddTransient<SearchResultsViewModel>();
            services.AddTransient<SearchResultsPage>();
            services.AddTransient<TaggedItemsViewModel>();
            services.AddTransient<TaggedItemsPage>();
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

        // The following code doesn't work
        // See details at https://github.com/microsoft/WindowsAppSDK/issues/3179
        //
        //var mainInstance = AppInstance.FindOrRegisterForKey("pocket-desktop-app-main-exe");

        //if (!mainInstance.IsCurrent)
        //{
        //    var activatedEventArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
        //    await mainInstance.RedirectActivationToAsync(activatedEventArgs);
        //    System.Diagnostics.Process.GetCurrentProcess().Kill();
        //    return;
        //}

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
