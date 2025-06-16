using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CrossMediaPlayer.Services.AppNavigation;
using CrossMediaPlayer.Services.MediaPlay;
using CrossMediaPlayer.Views.Pages;

namespace CrossMediaPlayer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IDisposable
{
    public BottomBarViewModel BottomBar { get; }
    
    private readonly IMediaPlayService _mediaPlayService;
    private readonly IAppNavigationService _appNavigationService;
    
    public MainWindowViewModel(
        BottomBarViewModel bottomBar,
        IMediaPlayService mediaPlayService,
        IAppNavigationService appNavigationService)
    {
        BottomBar = bottomBar;
        
        _mediaPlayService = mediaPlayService;
        _appNavigationService = appNavigationService;
        
        _appNavigationService.ContentsPageChanged += (_, newlySelectedPage) => CurrentlySelectedContentsPage = newlySelectedPage;
        
        _appNavigationService.SetContentsPage(new ArtistsPageView());
    }
    
    [ObservableProperty]
    private UserControl? _currentlySelectedContentsPage = new ArtistsPageView();
    
    public void Dispose()
    {
        _mediaPlayService.Dispose();
    }
}
