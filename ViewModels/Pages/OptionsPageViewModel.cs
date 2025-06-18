using CrossMediaPlayer.Enums;
using CrossMediaPlayer.Services.Translation;

namespace CrossMediaPlayer.ViewModels.Pages;

public partial class OptionsPageViewModel : ViewModelBase
{
    public ITranslationService TranslationService { get; }
    
    public OptionsPageViewModel(ITranslationService translationService)
    {
        TranslationService = translationService;

        TranslationService.SetLanguage(LanguageOption.Pl);
    }
}