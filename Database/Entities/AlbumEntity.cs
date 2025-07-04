using System.ComponentModel.DataAnnotations;

namespace CrossMediaPlayer.Database.Entities;

public class AlbumEntity
{
    [Required]
    public int Id { get; set; }
    
    [Required] [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public ushort? ReleaseYear { get; set; }
    
    public int? LengthInSeconds { get; set; }
}