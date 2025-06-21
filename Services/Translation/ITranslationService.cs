using CrossMediaPlayer.Enums;

namespace CrossMediaPlayer.Services.Translation;

public interface ITranslationService
{
    string this[string key] { get; }
    
    public void SetLanguage(LanguageOption languageOption);
}