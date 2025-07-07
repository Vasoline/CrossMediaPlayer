using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossMediaPlayer.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrossMediaPlayer.Database.Repositories.Album;

public class AlbumRepository : IAlbumRepository
{
    private readonly CrossMediaPlayerDbContext _dbContext;
    
    public AlbumRepository(CrossMediaPlayerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<AlbumEntity>> GetAllAlbumsForListView()
    {
        return await _dbContext.Albums
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<List<AlbumEntity>> GetAllAlbumsForArtist(int artistId)
    {
        return await _dbContext.Albums
            .AsNoTracking()
            .Where(x => x.ArtistId == artistId)
            .ToListAsync();
    }
}