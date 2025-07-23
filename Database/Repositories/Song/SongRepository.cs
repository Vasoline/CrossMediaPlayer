using System.Collections.Generic;
using System.Linq;
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

    public async Task<List<SongEntity>> GetAllSongsForAlbum(int albumId)
    {
        return await _dbContext.Songs
            .AsNoTracking()
            .Where(x => x.AlbumId == albumId)
            .ToListAsync();
    }

    public async Task<List<SongEntity>> GetAllSongsForArtistUnknownAlbum()
    {
        return await _dbContext.Songs
            .AsNoTracking()
            .Where(x => x.AlbumId == null)
            .ToListAsync();
    }
    
    public IAsyncEnumerable<SongEntity> StreamGetAllSongs()
    {
        return _dbContext.Songs
            .AsNoTracking()
            .AsAsyncEnumerable();
    }

    public async Task DeleteSongs(List<int> songsToDeleteIds)
    {
        await _dbContext.Songs
            .Where(x => songsToDeleteIds.Contains(x.Id))
            .ExecuteDeleteAsync();
    }

    public async Task<List<string>> GetAllSongLocations()
    {
        return await _dbContext.Songs.AsNoTracking().Select(x => x.FileLocation).ToListAsync();
    }

    public async Task AddNewSongs(List<SongEntity> newSongs)
    {
        await _dbContext.Songs.AddRangeAsync(newSongs);
        
        await _dbContext.SaveChangesAsync();
    }
}