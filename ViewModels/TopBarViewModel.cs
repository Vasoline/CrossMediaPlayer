using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CrossMediaPlayer.Enums;
using CrossMediaPlayer.Services.MediaLibraryService;

namespace CrossMediaPlayer.ViewModels;

public partial class TopBarViewModel : ViewModelBase
{
    private readonly IMediaLibraryService _mediaLibraryService;
    
    public TopBarViewModel(IMediaLibraryService mediaLibraryService)
    {
        _mediaLibraryService = mediaLibraryService;

        SyncStatus = "Media Folders Last Synced: Never";
        
        _mediaLibraryService.MediaSyncStatusChanged += OnMediaSyncStatusChanged;
        _mediaLibraryService.NewSongsAddedCountChanged += OnNewSongsAddedCountChanged;
    }
    
    [ObservableProperty]
    private string _syncStatus;
    
    [ObservableProperty]
    private int _newSongsAddedCount;
    
    [ObservableProperty]
    private bool _showNewSongsAdded;
    
    private void OnNewSongsAddedCountChanged(object? sender, int newSongsAddedCount)
    {
        NewSongsAddedCount = newSongsAddedCount;
    }
    
    private void OnMediaSyncStatusChanged(object? sender, MediaSyncStatus mediaSyncStatus)
    {
        ShowNewSongsAdded = false;
        
        switch (mediaSyncStatus)
        {
            default:
            case MediaSyncStatus.NotRunning:
            {
                SyncStatus = "Media Folders Last Synced: Never";
                break;
            }

            case MediaSyncStatus.CheckingExistingMedia:
            {
                SyncStatus = " - Syncing Media Folders";
                break;
            }
            
            case MediaSyncStatus.RemovingMissingMedia:
            {
                SyncStatus = " - Syncing Media Folders";
                break;
            }
            
            case MediaSyncStatus.AddingNewMedia:
            {
                SyncStatus = " - Syncing Media Folders";
                ShowNewSongsAdded = true;
                
                break;
            }
        }
    }
}