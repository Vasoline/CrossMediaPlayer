using CrossMediaPlayer.Services.Translation;

namespace CrossMediaPlayer.ViewModels.Pages;

public partial class AudioEffectsPageViewModel : ViewModelBase
{
    public ITranslationService TranslationService { get; }
    public AudioEffectsPageViewModel(ITranslationService translationService)
    {
        TranslationService = translationService;
    }
}