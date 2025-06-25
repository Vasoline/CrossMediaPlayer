using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CrossMediaPlayer.Enums;
using CrossMediaPlayer.Services.AppNavigation;
using CrossMediaPlayer.Services.MediaPlay;
using CrossMediaPlayer.Services.UserSettingsService;
using CrossMediaPlayer.Views.Pages;

namespace CrossMediaPlayer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IAsyncDisposable
{
    public BottomBarViewModel BottomBar { get; }
    
    private readonly IMediaPlayService _mediaPlayService;
    private readonly IAppNavigationService _appNavigationService;
    private readonly IUserSettingsService _userSettingsService;
    
    public MainWindowViewModel(
        BottomBarViewModel bottomBar,
        IMediaPlayService mediaPlayService,
        IAppNavigationService appNavigationService,
        IUserSettingsService userSettingsService)
    {
        BottomBar = bottomBar;
        
        _mediaPlayService = mediaPlayService;
        _appNavigationService = appNavigationService;
        _userSettingsService = userSettingsService;
        
        _appNavigationService.ContentsPageChanged += (_, newlySelectedPage) => CurrentlySelectedContentsPage = newlySelectedPage;

        SetDefaultTab();
    }
    
    [ObservableProperty]
    private UserControl? _currentlySelectedContentsPage;

    private void SetDefaultTab()
    {
        switch (_userSettingsService.UserSettings.DefaultStartupTab)
        {
            case SideMenuTab.MediaLibrary:
                _appNavigationService.SetContentsPage(new MediaLibraryPageView());
                break;
            
            case SideMenuTab.Artists: default:
                _appNavigationService.SetContentsPage(new ArtistsPageView());
                break;
            
            case SideMenuTab.Albums:
                _appNavigationService.SetContentsPage(new AlbumsPageView());
                break;
            
            case SideMenuTab.Playlists:
                _appNavigationService.SetContentsPage(new PlaylistsPageView());
                break;
            
            case SideMenuTab.AudioEffects:
                _appNavigationService.SetContentsPage(new AudioEffectsPageView());
                break;
            
            case SideMenuTab.Options:
                _appNavigationService.SetContentsPage(new OptionsPageView());
                break;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _userSettingsService.SaveUserSettings();
        
        _mediaPlayService.Dispose();
    }
}
