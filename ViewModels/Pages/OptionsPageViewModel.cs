using CommunityToolkit.Mvvm.ComponentModel;
using CrossMediaPlayer.Enums;
using CrossMediaPlayer.Services.Translation;

namespace CrossMediaPlayer.ViewModels.Pages;

public partial class OptionsPageViewModel : ViewModelBase
{
    public ITranslationService TranslationService { get; }
    
    public OptionsPageViewModel(ITranslationService translationService)
    {
        TranslationService = translationService;
    }
    
    [ObservableProperty]
    private LanguageOption _selectedLanguageOption = LanguageOption.En;
    
    [ObservableProperty]
    private MinimizeBehaviourOption _selectedMinimizeBehaviourOption = MinimizeBehaviourOption.TaskBar;
    
    [ObservableProperty]
    private SideMenuTab _selectedDefaultTabOption = SideMenuTab.Artists;
    
    [ObservableProperty]
    private ThemeOption _selectedThemeOption = ThemeOption.LightMode;
    
    partial void OnSelectedLanguageOptionChanged(LanguageOption value)
    {
        TranslationService.SetLanguage(value);
        
        // The rest of this function simply forces the values to change to a temp value and then back to the actual
        // value which then forces the UI to refresh so the dropdowns current selections show in the newly selected
        // language.
        
        // Store actual values
        var actualMinimizeBehaviourOption = SelectedMinimizeBehaviourOption;
        var actualDefaultTabOption = SelectedDefaultTabOption;
        var actualThemeOption = SelectedThemeOption;
        
        // Set fake values
        SelectedMinimizeBehaviourOption = (MinimizeBehaviourOption)(-1);
        SelectedDefaultTabOption = (SideMenuTab)(-1);
        SelectedThemeOption = (ThemeOption)(-1);
    
        // Reset to actual values
        SelectedMinimizeBehaviourOption = actualMinimizeBehaviourOption;
        SelectedDefaultTabOption = actualDefaultTabOption;
        SelectedThemeOption = actualThemeOption;
    }

    partial void OnSelectedMinimizeBehaviourOptionChanged(MinimizeBehaviourOption value)
    {
        
    }

    partial void OnSelectedDefaultTabOptionChanged(SideMenuTab value)
    {
        
    }

    partial void OnSelectedThemeOptionChanged(ThemeOption value)
    {
        
    }
}