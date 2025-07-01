using System.Collections.Generic;
using System.Threading.Tasks;
using CrossMediaPlayer.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrossMediaPlayer.Database.Repositories.Artist;

public class ArtistRepository : IArtistRepository
{
    private readonly CrossMediaPlayerDbContext _dbContext;
    
    public ArtistRepository(CrossMediaPlayerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<ArtistEntity>> GetAllArtistsForListView()
    {
        return await _dbContext.Artists
            .AsNoTracking()
            .ToListAsync();
    }
}