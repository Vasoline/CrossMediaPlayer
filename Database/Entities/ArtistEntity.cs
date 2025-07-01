using System.ComponentModel.DataAnnotations;

namespace CrossMediaPlayer.Database.Entities;

public class ArtistEntity
{
    public required int Id { get; set; }
    
    public required string Name { get; set; }
}