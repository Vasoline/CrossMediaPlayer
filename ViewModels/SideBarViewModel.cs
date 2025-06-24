using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrossMediaPlayer.Enums;
using CrossMediaPlayer.Services.AppNavigation;
using CrossMediaPlayer.Services.Translation;
using CrossMediaPlayer.Services.UserSettingsService;
using CrossMediaPlayer.Views.Pages;

namespace CrossMediaPlayer.ViewModels;

public partial class SideBarViewModel : ViewModelBase
{
    public ITranslationService TranslationService { get; }
    
    private readonly IAppNavigationService _appNavigationService;
    private readonly IUserSettingsService _userSettingsService;
    
    public SideBarViewModel(
        ITranslationService  translationService,
        IAppNavigationService appNavigationService,
        IUserSettingsService userSettingsService)
    {
        TranslationService = translationService;
        _appNavigationService = appNavigationService;
        _userSettingsService = userSettingsService;
        
        SetDefaultTab();
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
    private bool _mediaFoldersButtonSelected;
    
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
    public void MediaFoldersButtonClick()
    {
        ResetButtonsSelected();
        
        MediaFoldersButtonSelected = true;
        
        _appNavigationService.SetContentsPage(new MediaFoldersPageView());
    }
    
    [RelayCommand]
    public void OptionsButtonClick()
    {
        ResetButtonsSelected();
        
        OptionsButtonSelected = true;
        
        _appNavigationService.SetContentsPage(new OptionsPageView());
    }

    private void SetDefaultTab()
    {
        switch (_userSettingsService.UserSettings.DefaultStartupTab)
        {
            case SideMenuTab.MediaLibrary:
                MediaLibraryButtonSelected = true;
                break;
            
            case SideMenuTab.Artists: default:
                ArtistsButtonSelected = true;
                break;
            
            case SideMenuTab.Albums:
                AlbumsButtonSelected = true;
                break;
            
            case SideMenuTab.Playlists:
                PlaylistsButtonSelected = true;
                break;
            
            case SideMenuTab.MediaFolders:
                MediaFoldersButtonSelected = true;
                break;
            
            case SideMenuTab.Options:
                OptionsButtonSelected = true;
                break;
        }
    }
    
    private void ResetButtonsSelected()
    {
        MediaLibraryButtonSelected = false;
        ArtistsButtonSelected = false;
        AlbumsButtonSelected = false;
        PlaylistsButtonSelected = false;
        MediaFoldersButtonSelected = false;
        OptionsButtonSelected = false;
    }
}