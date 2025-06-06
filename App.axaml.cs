using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using CrossMediaPlayer.Services;
using CrossMediaPlayer.ViewModels;
using CrossMediaPlayer.Views;
using LibVLCSharp.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace CrossMediaPlayer;

public partial class App : Application
{
    public static ServiceProvider? ServiceProvider { get; private set; }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            Core.Initialize();
            
            var services = new ServiceCollection();
            
            InitialiseViews(services);
            InitialiseServices(services);

            ServiceProvider = services.BuildServiceProvider();

            desktop.MainWindow = new MainWindow
            {
                DataContext = ServiceProvider.GetService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private void InitialiseViews(ServiceCollection services)
    {
        services.AddTransient<BottomBarViewModel>();
        services.AddTransient<ContentViewModel>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<SideBarViewModel>();
        services.AddTransient<TopBarViewModel>();
    }
    
    private void InitialiseServices(ServiceCollection services)
    {
        services.AddSingleton<IAppNavigationService, AppNavigationService>();
        services.AddSingleton<IMediaPlayService, MediaPlayService>();
        services.AddSingleton<IDatabaseService, DatabaseService>();
    }
}