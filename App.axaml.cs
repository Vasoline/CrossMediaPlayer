using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using CrossMediaPlayer.Database;
using CrossMediaPlayer.Database.Repositories.Album;
using CrossMediaPlayer.Database.Repositories.Artist;
using CrossMediaPlayer.Database.Repositories.Song;
using CrossMediaPlayer.Services.AppNavigation;
using CrossMediaPlayer.Services.MediaPlay;
using CrossMediaPlayer.Services.Translation;
using CrossMediaPlayer.Services.UserSettingsService;
using CrossMediaPlayer.ViewModels;
using CrossMediaPlayer.ViewModels.Pages;
using CrossMediaPlayer.Views;
using LibVLCSharp.Shared;
using Microsoft.EntityFrameworkCore;
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
            
            using (var db = new CrossMediaPlayerDbContext())
            {
                db.Database.EnsureCreated();
            }
            
            var services = new ServiceCollection();
            
            // Database Setup
            services.AddDbContext<CrossMediaPlayerDbContext>(options =>
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var localDataFolder = Path.Combine(folder, "CrossMediaPlayer");
                var dbFilePath = Path.Combine(localDataFolder, "CrossMediaPlayerLibrary.db");

                if (!Directory.Exists(localDataFolder))
                {
                    Directory.CreateDirectory(localDataFolder);
                }
        
                options.UseSqlite($"Data Source={dbFilePath}");
            });
            
            InitialiseViews(services);
            InitialiseServices(services);
            InitialiseRepositories(services);

            ServiceProvider = services.BuildServiceProvider();

            // We need to load the user settings so the data is ready for use in page constructors
            var userSettingsService = ServiceProvider.GetService<IUserSettingsService>();
            userSettingsService?.LoadUserSettings(); 
            
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
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<SideBarViewModel>();
        services.AddTransient<TopBarViewModel>();
        
        services.AddTransient<MediaLibraryPageViewModel>();
        services.AddTransient<ArtistsPageViewModel>();
        services.AddTransient<AlbumsPageViewModel>();
        services.AddTransient<PlaylistsPageViewModel>();
        services.AddTransient<AudioEffectsPageViewModel>();
        services.AddTransient<OptionsPageViewModel>();
    }
    
    private void InitialiseServices(ServiceCollection services)
    {
        services.AddSingleton<IAppNavigationService, AppNavigationService>();
        services.AddSingleton<IMediaPlayService, MediaPlayService>();
        services.AddSingleton<ITranslationService, TranslationService>();
        services.AddSingleton<IUserSettingsService, UserSettingsService>();
    }

    private void InitialiseRepositories(ServiceCollection services)
    {
        services.AddScoped<IArtistRepository, ArtistRepository>();
        services.AddScoped<IAlbumRepository, AlbumRepository>();
        services.AddScoped<ISongRepository, SongRepository>();
    }
}