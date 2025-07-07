using System.Collections.Generic;
using System.Threading.Tasks;
using CrossMediaPlayer.Database.Entities;

namespace CrossMediaPlayer.Database.Repositories.Song;

public interface ISongRepository
{
    public Task<List<SongEntity>> GetAllSongsForListView();
    public Task<List<SongEntity>> GetAllSongsForAlbum(int albumId);
    public Task<List<SongEntity>> GetAllSongsForArtistUnknownAlbum();
    public IAsyncEnumerable<SongEntity> StreamGetAllSongs();
    public Task DeleteSongs(List<int> songsToDeleteIds);
}