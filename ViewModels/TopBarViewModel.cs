using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CrossMediaPlayer.Services.MediaLibraryService;

namespace CrossMediaPlayer.ViewModels;

public partial class TopBarViewModel : ViewModelBase
{
    private readonly IMediaLibraryService _mediaLibraryService;
    
    public TopBarViewModel(IMediaLibraryService mediaLibraryService)
    {
        _mediaLibraryService = mediaLibraryService;
        
        _mediaLibraryService.NewSongsAddedCountChanged += OnNewSongsAddedCountChanged;
    }
    
    [ObservableProperty]
    private int _newSongsAddedCount;
    
    private void OnNewSongsAddedCountChanged(object? sender, int newSongsAddedCount)
    {
        NewSongsAddedCount = newSongsAddedCount;
    }
}