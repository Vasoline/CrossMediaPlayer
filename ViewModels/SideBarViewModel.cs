using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrossMediaPlayer.Services.AppNavigation;
using CrossMediaPlayer.Views.Pages;

namespace CrossMediaPlayer.ViewModels;

public partial class SideBarViewModel : ViewModelBase
{
    private readonly IAppNavigationService _appNavigationService;
    
    public SideBarViewModel(IAppNavigationService appNavigationService)
    {
        _appNavigationService = appNavigationService;
        
        _artistsButtonSelected = true;
    }
    
    [ObservableProperty]
    private bool _mediaLibraryButtonSelected;
    
    [ObservableProperty]
    private bool _artistsButtonSelected;
    
    [ObservableProperty]
    private bool _albumsButtonSelected;
    
    [ObservableProperty]
    private bool _playlistsButtonSelected;
    
    [ObservableProperty]
    private bool _optionsButtonSelected;


    [RelayCommand]
    public void MediaLibraryButtonClick()
    {
        ResetButtonsSelected();

        MediaLibraryButtonSelected = true;
        
        _appNavigationService.SetContentsPage(new MediaLibraryPageView());
    }
    
    [RelayCommand]
    public void ArtistsButtonClick()
    {
        ResetButtonsSelected();
        
        ArtistsButtonSelected = true;
        
        _appNavigationService.SetContentsPage(new ArtistsPageView());
    }
    
    [RelayCommand]
    public void AlbumsButtonClick()
    {
        ResetButtonsSelected();
        
        AlbumsButtonSelected = true;
        
        _appNavigationService.SetContentsPage(new AlbumsPageView());
    }
    
    [RelayCommand]
    public void PlaylistsButtonClick()
    {
        ResetButtonsSelected();
        
        PlaylistsButtonSelected = true;
        
        _appNavigationService.SetContentsPage(new PlaylistsPageView());
    }
    
    [RelayCommand]
    public void OptionsButtonClick()
    {
        ResetButtonsSelected();
        
        OptionsButtonSelected = true;
        
        _appNavigationService.SetContentsPage(new OptionsPageView());
    }

    private void ResetButtonsSelected()
    {
        MediaLibraryButtonSelected = false;
        ArtistsButtonSelected = false;
        AlbumsButtonSelected = false;
        PlaylistsButtonSelected = false;
        OptionsButtonSelected = false;
    }
}