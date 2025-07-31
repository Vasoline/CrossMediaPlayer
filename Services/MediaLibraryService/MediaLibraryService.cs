using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Threading;
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

        _ = PopulateLibraryFromDatabase();
    }

    public event EventHandler<MediaSyncStatus>? MediaSyncStatusChanged;
    public event EventHandler<int>? NewSongsAddedCountChanged;
    
    public event EventHandler? ArtistsUpdated;
    public event EventHandler? AlbumsUpdated;
    public event EventHandler? SongsUpdated;
    
    private CancellationTokenSource _mediaSyncCancellationToken;
    private MediaSyncStatus _mediaSyncStatus = MediaSyncStatus.NotRunning;
    private int _newSongsAddedCount;

    public List<ArtistEntity> ArtistsInLibrary { get; private set; } = new();
    public List<AlbumEntity> AlbumsInLibrary { get; private set; } = new();
    public List<SongEntity> SongsInLibrary { get; private set; } = new();
    
    public MediaSyncStatus GetMediaSyncStatus()
    {
        return _mediaSyncStatus;
    }

    public async Task CancelMediaSyncing()
    {
        if (_mediaSyncStatus != MediaSyncStatus.NotRunning)
        {
            await _mediaSyncCancellationToken.CancelAsync();
        }
    }
    
    public async Task SyncMediaLibrary()
    {
        if (_mediaSyncStatus != MediaSyncStatus.NotRunning || !_userSettingsService.UserSettings.MediaFolders.Any())
        {
            // Already running or no media folders set
            
            return;
        }
        
        _mediaSyncCancellationToken = new CancellationTokenSource();

        try
        {
            await CheckExistingMedia();
            await AddNewMedia();
            
            _userSettingsService.UserSettings.SetMediaFoldersLastSynced();
        }
        catch (OperationCanceledException)
        {
            // handle cancelled if needed
        }
        catch (Exception exception)
        {
            // log error
            throw;
        }
        finally
        {
            _mediaSyncCancellationToken.Dispose();
            
            _mediaSyncStatus = MediaSyncStatus.NotRunning;
            
            _newSongsAddedCount = 0;
            NewSongsAddedCountChanged?.Invoke(this, _newSongsAddedCount);
            MediaSyncStatusChanged?.Invoke(this, _mediaSyncStatus);
        }
    }

    private async Task CheckExistingMedia()
    {
        _mediaSyncStatus = MediaSyncStatus.CheckingExistingMedia;
        MediaSyncStatusChanged?.Invoke(this, _mediaSyncStatus);

        var allSongsInDb = _songRepository.StreamGetAllSongs();

        var songsToRemove = new ConcurrentBag<int>();

        const int maxParallelWorkers = 8;
        
        await Parallel.ForEachAsync(
        allSongsInDb, 
        new ParallelOptions
        {
            MaxDegreeOfParallelism = Math.Min(Math.Max(1, Environment.ProcessorCount - 1), maxParallelWorkers),
            CancellationToken = _mediaSyncCancellationToken.Token
        },
        (song, _) =>
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
            MediaSyncStatusChanged?.Invoke(this, _mediaSyncStatus);
            
            await _songRepository.DeleteSongs(songsToRemove.ToList());
        }
    }

    private async Task AddNewMedia()
    {
        _mediaSyncStatus = MediaSyncStatus.AddingNewMedia;
        MediaSyncStatusChanged?.Invoke(this, _mediaSyncStatus);

        var songsToAdd = new List<SongEntity>();
        
        var existingMediaFiles = new HashSet<string>(
            await _songRepository.GetAllSongLocations(),
            StringComparer.OrdinalIgnoreCase);
        
        var existingArtists = await _artistRepository.GetAllArtists();
        var existingAlbums = await _albumRepository.GetAllAlbums();
        
        var artistLookup = existingArtists.ToDictionary(
            x => x.Name, 
            x => x, 
            StringComparer.OrdinalIgnoreCase);

        var albumLookup = existingAlbums.ToDictionary(
            x => x.Name, 
            x => x, 
            StringComparer.OrdinalIgnoreCase);
        
        var mediaFolders = _userSettingsService.UserSettings.MediaFolders;

        foreach (var folder in mediaFolders)
        {
            _mediaSyncCancellationToken.Token.ThrowIfCancellationRequested();
            
            var foundFileLocationsInFolder = new FileSystemEnumerable<string>(
                folder,
                (ref FileSystemEntry entry) => entry.ToFullPath(),
                new EnumerationOptions { RecurseSubdirectories = true })
            {
                ShouldIncludePredicate = (ref FileSystemEntry entry) => !entry.IsDirectory
            };
            
            foreach(var fileLocation in foundFileLocationsInFolder)
            {
                _mediaSyncCancellationToken.Token.ThrowIfCancellationRequested();
                
                var isAudioFile = await _mediaPlayService.IsAudioFile(fileLocation);

                if (!isAudioFile)
                {
                    continue;
                }
                
                var fileInfo = new FileInfo(fileLocation);
                
                if (!existingMediaFiles.Contains(fileLocation))
                {
                    TagLib.File? songMetaData;

                    try
                    { 
                        songMetaData = TagLib.File.Create(fileLocation);
                    }
                    catch (Exception exception)
                    {
                        // log exception
                        
                        continue;
                    }

                    var artistName = songMetaData.Tag.AlbumArtists.FirstOrDefault()?.Trim() ?? "Unknown Artist";
                    var albumName = (songMetaData.Tag.Album?.Trim() ?? "Unknown Album");
                    
                    if (!artistLookup.TryGetValue(artistName, out var songArtist))
                    {
                        songArtist = await _artistRepository.AddNewArtist(new ArtistEntity
                        {
                            Name = artistName
                        });
                    
                        artistLookup[artistName] = songArtist;
                    }

                    if (!albumLookup.TryGetValue(albumName, out var songAlbum))
                    {
                        songAlbum = await _albumRepository.AddNewAlbum(new AlbumEntity
                        {
                            ArtistId = songArtist.Id,
                            Name = albumName,
                        });
                    
                        albumLookup[albumName] = songAlbum;
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

                    _newSongsAddedCount++;
                    NewSongsAddedCountChanged?.Invoke(this, _newSongsAddedCount);
                }
            }
        }
        
        if (songsToAdd.Any())
        {
            await _songRepository.AddNewSongs(songsToAdd);
            
            await PopulateLibraryFromDatabase();
        }
    }

    private async Task PopulateLibraryFromDatabase()
    {
        ArtistsInLibrary = await _artistRepository.GetAllArtists();
        AlbumsInLibrary = await _albumRepository.GetAllAlbums();
        SongsInLibrary = await _songRepository.GetAllSongs();

        ArtistsInLibrary = ArtistsInLibrary.OrderBy(x => x.Name).ToList();
        AlbumsInLibrary = AlbumsInLibrary.OrderBy(x => x.Name).ToList();
        SongsInLibrary = SongsInLibrary.OrderBy(x => x.Title).ToList();

        ArtistsUpdated?.Invoke(this, EventArgs.Empty);
        AlbumsUpdated?.Invoke(this, EventArgs.Empty);
        SongsUpdated?.Invoke(this, EventArgs.Empty);
    }
}