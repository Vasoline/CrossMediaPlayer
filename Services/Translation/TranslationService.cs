using System.Collections.Generic;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CrossMediaPlayer.Enums;

namespace CrossMediaPlayer.Services.Translation;

public class TranslationService : ObservableObject, ITranslationService
{
    public string this[string key] => GetTranslatedTermFromKey(key);

    private LanguageOption _currentLanguage = LanguageOption.En;
    private Dictionary<string, Dictionary<string, string>> _translationsDictionary = new();

    public TranslationService()
    {
        LoadTranslationsFromFile();
    }
    
    public void SetLanguage(LanguageOption languageOption)
    {
        _currentLanguage = languageOption;
        
        OnPropertyChanged(string.Empty);
    }
    
    private void LoadTranslationsFromFile()
    {
        var assembly = typeof(TranslationService).Assembly;
        var resourceName = "CrossMediaPlayer.Resources.Translations.json";
    
        using var stream = assembly.GetManifestResourceStream(resourceName);

        if (stream is not null)
        {
            _translationsDictionary = JsonSerializer
                .Deserialize<Dictionary<string, Dictionary<string, string>>>(stream) ?? new();
        }
    }
    
    private string GetTranslatedTermFromKey(string key)
    {
        var langKey = _currentLanguage.ToString().ToLower();
        
        if (_translationsDictionary.ContainsKey(key) && _translationsDictionary[key].ContainsKey(langKey))
        {
            return _translationsDictionary[key][langKey];
        }
            
        return "Unmapped";
    }
}