using System;
using System.Threading.Tasks;
using CrossMediaPlayer.Enums;

namespace CrossMediaPlayer.Services.MediaLibraryService;

public interface IMediaLibraryService
{
    public event EventHandler<int>? NewSongsAddedCountChanged;
    public MediaSyncStatus GetMediaSyncStatus();
    public Task SyncMediaLibrary();
}