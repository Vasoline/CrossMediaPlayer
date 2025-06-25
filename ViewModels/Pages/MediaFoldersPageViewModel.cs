using CrossMediaPlayer.Services.Translation;

namespace CrossMediaPlayer.ViewModels.Pages;

public partial class MediaFoldersPageViewModel : ViewModelBase
{
    public ITranslationService TranslationService { get; }
    public MediaFoldersPageViewModel(ITranslationService translationService)
    {
        TranslationService = translationService;
    }
}