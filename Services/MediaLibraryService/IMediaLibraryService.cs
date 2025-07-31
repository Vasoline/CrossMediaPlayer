using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrossMediaPlayer.Database.Entities;
using CrossMediaPlayer.Enums;

namespace CrossMediaPlayer.Services.MediaLibraryService;

public interface IMediaLibraryService
{
    public event EventHandler<MediaSyncStatus>? MediaSyncStatusChanged;
    public event EventHandler<int>? NewSongsAddedCountChanged;
    
    public event EventHandler? ArtistsUpdated;
    public event EventHandler? AlbumsUpdated;
    public event EventHandler? SongsUpdated;
    public List<ArtistEntity> ArtistsInLibrary { get; }
    public List<AlbumEntity> AlbumsInLibrary { get; }
    public List<SongEntity> SongsInLibrary { get; }
    
    public MediaSyncStatus GetMediaSyncStatus();
    public Task CancelMediaSyncing();
    public Task SyncMediaLibrary();
}