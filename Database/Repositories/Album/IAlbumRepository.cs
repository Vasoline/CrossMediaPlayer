using System.Collections.Generic;
using System.Threading.Tasks;
using CrossMediaPlayer.Database.Entities;

namespace CrossMediaPlayer.Database.Repositories.Album;

public interface IAlbumRepository
{
    public Task<List<AlbumEntity>> GetAllAlbumsForListView();
}