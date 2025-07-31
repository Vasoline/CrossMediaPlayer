using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CrossMediaPlayer.Database.Entities;
using CrossMediaPlayer.Services.MediaLibraryService;
using CrossMediaPlayer.Services.Translation;

namespace CrossMediaPlayer.ViewModels.Pages;

public partial class ArtistsPageViewModel : ViewModelBase
{
    public ITranslationService TranslationService { get; }

    private readonly IMediaLibraryService _mediaLibraryService;

    public ArtistsPageViewModel(
        ITranslationService translationService,
        IMediaLibraryService mediaLibraryService)
    {
        TranslationService = translationService;
        
        _mediaLibraryService = mediaLibraryService;

        _artistsList = _mediaLibraryService.ArtistsInLibrary;
        
        _mediaLibraryService.ArtistsUpdated += OnArtistsUpdated;
    }

    [ObservableProperty]
    public List<ArtistEntity> _artistsList = new();
    
    private void OnArtistsUpdated(object? sender, EventArgs e)
    {
        ArtistsList = _mediaLibraryService.ArtistsInLibrary;
    }
}