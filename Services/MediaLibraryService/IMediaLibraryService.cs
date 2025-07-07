using System.Threading.Tasks;
using CrossMediaPlayer.Enums;

namespace CrossMediaPlayer.Services.MediaLibraryService;

public interface IMediaLibraryService
{
    public MediaSyncStatus GetMediaSyncStatus();
    public Task SyncMediaLibrary();
}