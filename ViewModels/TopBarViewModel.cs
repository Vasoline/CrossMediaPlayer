using System;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrossMediaPlayer.Enums;
using CrossMediaPlayer.Services.MediaLibraryService;
using CrossMediaPlayer.Services.UserSettingsService;

namespace CrossMediaPlayer.ViewModels;

public partial class TopBarViewModel : ViewModelBase
{
    private readonly IMediaLibraryService _mediaLibraryService;
    private readonly IUserSettingsService _userSettingsService;
    
    public TopBarViewModel(
        IMediaLibraryService mediaLibraryService,
        IUserSettingsService userSettingsService)
    {
        _mediaLibraryService = mediaLibraryService;
        _userSettingsService = userSettingsService;

        SetMediaFoldersLastSynced();

        ShowSyncNowButton = true;
        
        _mediaLibraryService.MediaSyncStatusChanged += OnMediaSyncStatusChanged;
        _mediaLibraryService.NewSongsAddedCountChanged += OnNewSongsAddedCountChanged;
    }
    
    [ObservableProperty]
    private string? _syncStatus;
    
    [ObservableProperty]
    private int _newSongsAddedCount;
    
    [ObservableProperty]
    private bool _showSyncNowButton;
    
    [ObservableProperty]
    private bool _showNewSongsAdded;


    [RelayCommand]
    public async Task SyncNowButtonClick()
    {
        await _mediaLibraryService.SyncMediaLibrary();
    }
    
    [RelayCommand]
    public async Task CancelSyncButtonClick()
    {
        await _mediaLibraryService.CancelMediaSyncing();
    }
    
    private void OnNewSongsAddedCountChanged(object? sender, int newSongsAddedCount)
    {
        NewSongsAddedCount = newSongsAddedCount;
    }
    
    private void OnMediaSyncStatusChanged(object? sender, MediaSyncStatus mediaSyncStatus)
    {
        ShowSyncNowButton = false;
        ShowNewSongsAdded = false;
        
        switch (mediaSyncStatus)
        {
            default:
            case MediaSyncStatus.NotRunning:
            {
                SetMediaFoldersLastSynced();
                ShowSyncNowButton = true;
                
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

    private void SetMediaFoldersLastSynced()
    {
        var mediaFoldersLastSynced = _userSettingsService.UserSettings.MediaFoldersLastSynced is not null 
            ? _userSettingsService.UserSettings.MediaFoldersLastSynced.Value
                .ToString("dd MMM yyyy - HH:mm", CultureInfo.InvariantCulture)
            : "Never";
        
        SyncStatus = $"Media Folders Last Synced: {mediaFoldersLastSynced}";
    }
}