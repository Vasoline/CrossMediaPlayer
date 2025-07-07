using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrossMediaPlayer.Database.Entities;
using CrossMediaPlayer.Database.Repositories.Album;
using CrossMediaPlayer.Database.Repositories.Artist;
using CrossMediaPlayer.Database.Repositories.Song;
using CrossMediaPlayer.Enums;
using CrossMediaPlayer.Services.UserSettingsService;

namespace CrossMediaPlayer.Services.MediaLibraryService;

public class MediaLibraryService : IMediaLibraryService
{
    private readonly IArtistRepository _artistRepository;
    private readonly IAlbumRepository _albumRepository;
    private readonly ISongRepository _songRepository;
    private readonly IUserSettingsService _userSettingsService;
    
    public MediaLibraryService(
        IArtistRepository artistRepository,
        IAlbumRepository albumRepository,
        ISongRepository songRepository,
        IUserSettingsService userSettingsService)
    {
        _artistRepository = artistRepository;
        _albumRepository = albumRepository;
        _songRepository = songRepository;
        _userSettingsService = userSettingsService;
    }

    private MediaSyncStatus _mediaSyncStatus = MediaSyncStatus.NotRunning;

    public MediaSyncStatus GetMediaSyncStatus()
    {
        return _mediaSyncStatus;
    }
    
    public async Task SyncMediaLibrary()
    {
        if (_mediaSyncStatus != MediaSyncStatus.NotRunning || !_userSettingsService.UserSettings.MediaFolders.Any())
        {
            // Already running or no media folders set
            
            return;
        }

        await CheckExistingMedia();
        await AddNewMedia();
    }

    private async Task CheckExistingMedia()
    {
        _mediaSyncStatus = MediaSyncStatus.CheckingExistingMedia;

        var allSongsInDb = _songRepository.StreamGetAllSongs();

        var songsToRemove = new ConcurrentBag<int>();

        await Parallel.ForEachAsync(allSongsInDb, 
            new ParallelOptions { MaxDegreeOfParallelism = Math.Min(Math.Max(1, Environment.ProcessorCount - 1), 8) }, 
            async (song, _) =>
        {
            if (!File.Exists(song.FileLocation))
            {
                songsToRemove.Add(song.Id);
            }
        });

        if (songsToRemove.Any())
        {
            _mediaSyncStatus = MediaSyncStatus.RemovingMissingMedia;
            
            await _songRepository.DeleteSongs(songsToRemove.ToList());
        }
    }

    private async Task AddNewMedia()
    {
        _mediaSyncStatus = MediaSyncStatus.AddingNewMedia;
    }
}