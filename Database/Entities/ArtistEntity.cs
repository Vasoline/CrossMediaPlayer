using System.ComponentModel.DataAnnotations;

namespace CrossMediaPlayer.Database.Entities;

public class ArtistEntity
{
    public int Id { get; set; }
    
    [Required] [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
}