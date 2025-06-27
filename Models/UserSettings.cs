using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CrossMediaPlayer.Enums;

namespace CrossMediaPlayer.Models;

public class UserSettings
{
    [JsonInclude]
    public LanguageOption Language { get; private set; } = LanguageOption.En;
    
    [JsonInclude]
    public MinimizeBehaviourOption MinimizeBehaviour { get; private set; } = MinimizeBehaviourOption.TaskBar;
    
    [JsonInclude]
    public SideMenuTab DefaultStartupTab { get; private set; } = SideMenuTab.Artists;
    
    [JsonInclude]
    public ThemeOption Theme { get; private set; } = ThemeOption.LightMode;
    
    [JsonInclude]
    public int AudioVolume { get; private set; } = 100;
    
    [JsonInclude]
    public PlayModeOption PlayMode { get; private set; } = PlayModeOption.Standard;
    
    [JsonInclude]
    public List<string> MediaFolders { get; private set; } = new();
    
    
    public void SaveLanguageOption(LanguageOption languageOption)
    {
        Language = languageOption;
    }
    
    public void SaveMinimizeBehaviourOption(MinimizeBehaviourOption minimizeBehaviourOption)
    {
        MinimizeBehaviour = minimizeBehaviourOption;
    }
    
    public void SaveDefaultStartupTab(SideMenuTab startupTab)
    {
        DefaultStartupTab = startupTab;
    }
    
    public void SaveThemeOption(ThemeOption themeOption)
    {
        Theme = themeOption;
    }
    
    public void SaveAudioVolume(int audioVolume)
    {
        const int minVolume = 0;
        const int maxVolume = 100;

        AudioVolume = Math.Clamp(audioVolume, minVolume, maxVolume);
    }
    
    public void SavePlayModeOption(PlayModeOption playModeOption)
    {
        PlayMode = playModeOption;
    }
    
    public void SaveMediaFolders(List<string> mediaFolders)
    {
        MediaFolders = mediaFolders;
    }
}