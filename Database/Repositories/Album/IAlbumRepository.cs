using System.Collections.Generic;
using System.Threading.Tasks;
using CrossMediaPlayer.Database.Entities;

namespace CrossMediaPlayer.Database.Repositories.Album;

public interface IAlbumRepository
{
    public Task<List<AlbumEntity>> GetAllAlbumsForListView();
    public Task<List<AlbumEntity>> GetAllAlbumsForArtist(int artistId);
}