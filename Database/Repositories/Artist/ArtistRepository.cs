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
    
    public async Task<List<ArtistEntity>> GetAllArtists()
    {
        return await _dbContext.Artists
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ArtistEntity> AddNewArtist(ArtistEntity artist)
    {
        var newArtist = await _dbContext.Artists.AddAsync(artist);
        
        await _dbContext.SaveChangesAsync();
        
        return newArtist.Entity;
    }
}