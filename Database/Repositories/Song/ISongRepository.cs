using System.Collections.Generic;
using System.Threading.Tasks;
using CrossMediaPlayer.Database.Entities;

namespace CrossMediaPlayer.Database.Repositories.Song;

public interface ISongRepository
{
    public Task<List<SongEntity>> GetAllSongsForListView();
}