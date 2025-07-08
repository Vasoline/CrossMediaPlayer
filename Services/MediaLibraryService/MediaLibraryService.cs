using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Threading.Tasks;
using CrossMediaPlayer.Database.Entities;
using CrossMediaPlayer.Database.Repositories.Album;
using CrossMediaPlayer.Database.Repositories.Artist;
using CrossMediaPlayer.Database.Repositories.Song;
using CrossMediaPlayer.Enums;
using CrossMediaPlayer.Services.MediaPlay;
using CrossMediaPlayer.Services.UserSettingsService;

namespace CrossMediaPlayer.Services.MediaLibraryService;

public class MediaLibraryService : IMediaLibraryService
{
    private readonly IArtistRepository _artistRepository;
    private readonly IAlbumRepository _albumRepository;
    private readonly ISongRepository _songRepository;
    private readonly IUserSettingsService _userSettingsService;
    private readonly IMediaPlayService _mediaPlayService;
    
    public MediaLibraryService(
        IArtistRepository artistRepository,
        IAlbumRepository albumRepository,
        ISongRepository songRepository,
        IUserSettingsService userSettingsService,
        IMediaPlayService mediaPlayService)
    {
        _artistRepository = artistRepository;
        _albumRepository = albumRepository;
        _songRepository = songRepository;
        _userSettingsService = userSettingsService;
        _mediaPlayService = mediaPlayService;
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

        try
        {
            await CheckExistingMedia();
            await AddNewMedia();
        }
        catch (Exception exception)
        {
            // log error
            throw;
        }
        finally
        {
            _mediaSyncStatus = MediaSyncStatus.NotRunning;
        }
    }

    private async Task CheckExistingMedia()
    {
        _mediaSyncStatus = MediaSyncStatus.CheckingExistingMedia;

        var allSongsInDb = _songRepository.StreamGetAllSongs();

        var songsToRemove = new ConcurrentBag<int>();

        await Parallel.ForEachAsync(allSongsInDb, 
            new ParallelOptions { MaxDegreeOfParallelism = Math.Min(Math.Max(1, Environment.ProcessorCount - 1), 8) }, (song, _) =>
            {
                if (!File.Exists(song.FileLocation))
                {
                    songsToRemove.Add(song.Id);
                }

                return ValueTask.CompletedTask;
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

        var songsToAdd = new List<SongEntity>();
        
        var existingMediaFiles = await _songRepository.GetAllSongLocations();
        var existingArtists = await _artistRepository.GetAllArtists();
        var existingAlbums = await _albumRepository.GetAllAlbums();
        
        var mediaFolders = _userSettingsService.UserSettings.MediaFolders;

        foreach (var folder in mediaFolders)
        {
            var foundFileLocationsInFolder = new FileSystemEnumerable<string>(
                folder,
                (ref FileSystemEntry entry) => entry.ToFullPath(),
                new EnumerationOptions { RecurseSubdirectories = true })
            {
                ShouldIncludePredicate = (ref FileSystemEntry entry) => !entry.IsDirectory
            };
            
            foreach(var fileLocation in foundFileLocationsInFolder)
            {
                var isAudioFile = await _mediaPlayService.IsAudioFile(fileLocation);

                if (!isAudioFile)
                {
                    continue;
                }
                
                var fileInfo = new FileInfo(fileLocation);
                
                if (!existingMediaFiles.Contains(fileLocation))
                {
                    var songMetaData = TagLib.File.Create(fileLocation);

                    var artistName = songMetaData.Tag.FirstPerformer ?? "Unknown Artist";
                    var albumName = (songMetaData.Tag.Album ?? "Unknown Album");
                    
                    var songArtist = existingArtists
                        .FirstOrDefault(x =>
                            string.Equals(x.Name, artistName.Trim(), StringComparison.OrdinalIgnoreCase));
                    
                    var songAlbum = existingAlbums
                        .FirstOrDefault(x => 
                            string.Equals(x.Name, albumName.Trim(), StringComparison.OrdinalIgnoreCase));
                    
                    if (songArtist is null)
                    {
                        songArtist = await _artistRepository.AddNewArtist(new ArtistEntity
                        {
                            Name = songMetaData.Tag.FirstPerformer?.Trim() ?? "Unknown Artist"
                        });
                        
                        existingArtists.Add(songArtist);
                    }

                    if (songAlbum is null)
                    {
                        songAlbum = await _albumRepository.AddNewAlbum(new AlbumEntity
                        {
                            ArtistId = songArtist.Id,
                            Name = songMetaData.Tag.Album?.Trim() ?? "Unknown Album",
                            // ReleaseYear = null, - need to find best way to do this after, probably using musicbrainz
                            // LengthInSeconds = null - need to calculate this at the end
                        });
                        
                        existingAlbums.Add(songAlbum);
                    }
                    
                    songsToAdd.Add(new SongEntity
                    {
                        ArtistId = songArtist.Id,
                        AlbumId = songAlbum.Id,
                        Title = songMetaData.Tag.Title?.Trim() ?? "Unknown Song",
                        TrackNumber = (ushort)songMetaData.Tag.Track,
                        YearReleased = songMetaData.Tag.Year > 0 ? (ushort)songMetaData.Tag.Year : null,
                        LengthInSeconds = (int?)songMetaData.Properties.Duration.TotalSeconds,
                        Format = songMetaData.Properties.Codecs.FirstOrDefault()?.Description ?? "Unknown Codec",
                        FileLocation = fileLocation,
                        FileSize = fileInfo.Length,
                        LastModified = fileInfo.LastWriteTimeUtc
                    });
                }
            }
        }
        
        if (songsToAdd.Any())
        {
            await _songRepository.AddNewSongs(songsToAdd);
        }
    }
}