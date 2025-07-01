using System.Collections.Generic;
using System.Threading.Tasks;
using CrossMediaPlayer.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrossMediaPlayer.Database.Repositories.Song;

public class SongRepository : ISongRepository
{
    private readonly CrossMediaPlayerDbContext _dbContext;
    
    public SongRepository(CrossMediaPlayerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<SongEntity>> GetAllSongsForListView()
    {
        return await _dbContext.Songs
            .AsNoTracking()
            .ToListAsync();
    }
}