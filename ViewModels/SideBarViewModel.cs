using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CrossMediaPlayer.ViewModels;

public partial class SideBarViewModel : ViewModelBase
{
    public SideBarViewModel()
    {
        _mediaLibraryButtonSelected = true;
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
    }
    
    [RelayCommand]
    public void ArtistsButtonClick()
    {
        ResetButtonsSelected();
        
        ArtistsButtonSelected = true;
    }
    
    [RelayCommand]
    public void AlbumsButtonClick()
    {
        ResetButtonsSelected();
        
        AlbumsButtonSelected = true;
    }
    
    [RelayCommand]
    public void PlaylistsButtonClick()
    {
        ResetButtonsSelected();
        
        PlaylistsButtonSelected = true;
    }
    
    [RelayCommand]
    public void OptionsButtonClick()
    {
        ResetButtonsSelected();
        
        OptionsButtonSelected = true;
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