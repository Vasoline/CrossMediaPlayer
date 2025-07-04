using System.Linq;
using System.Threading.Tasks;
using CrossMediaPlayer.Database.Repositories.Album;
using CrossMediaPlayer.Database.Repositories.Artist;
using CrossMediaPlayer.Database.Repositories.Song;
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

    private static bool _mediaLibraryIsSyncing = false;

    public async Task SyncMediaLibrary()
    {
        if (_mediaLibraryIsSyncing || !_userSettingsService.UserSettings.MediaFolders.Any())
        {
            // Already running or no media folders set
            
            return;
        }

        await CheckExistingMedia();
    }

    private async Task CheckExistingMedia()
    {
        
    }
}