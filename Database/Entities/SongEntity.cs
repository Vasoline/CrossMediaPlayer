using System;
using System.ComponentModel.DataAnnotations;

namespace CrossMediaPlayer.Database.Entities;

public class SongEntity
{
    [Required]
    public int Id { get; set; }
    
    public int? ArtistId { get; set; }
    
    public int? AlbumId { get; set; }
    
    [Required] [MaxLength(256)]
    public string Title { get; set; } = String.Empty;
    
    [Required]
    public ushort TrackNumber { get; set; }
    
    public ushort? YearReleased { get; set; }
    
    public int? LengthInSeconds { get; set; }

    [Required] [MaxLength(64)]
    public string Format { get; set; } = String.Empty;
    
    [Required] [MaxLength(4096)]
    public string FileLocation { get; set; } = string.Empty;
    
    [Required]
    public long FileSize { get; set; }
    
    [Required]
    public DateTime LastModified { get; set; }
}