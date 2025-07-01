using System.Collections.Generic;
using System.Threading.Tasks;
using CrossMediaPlayer.Database.Entities;

namespace CrossMediaPlayer.Database.Repositories.Artist;

public interface IArtistRepository
{
    public Task<List<ArtistEntity>> GetAllArtistsForListView();
}