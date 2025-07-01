using System;
using System.IO;
using CrossMediaPlayer.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrossMediaPlayer.Database;

public class CrossMediaPlayerDbContext : DbContext
{
    public DbSet<ArtistEntity> Artists { get; set; }
    public DbSet<AlbumEntity> Albums { get; set; }
    public DbSet<SongEntity> Songs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureArtistsTable(modelBuilder);
        ConfigureAlbumsTable(modelBuilder);
        ConfigureSongsTable(modelBuilder);
    }

    private void ConfigureArtistsTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArtistEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);
        });
    }
    
    private void ConfigureAlbumsTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlbumEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
    
    private void ConfigureSongsTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SongEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}