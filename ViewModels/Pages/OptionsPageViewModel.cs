using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrossMediaPlayer.Enums;
using CrossMediaPlayer.Services.MediaLibraryService;
using CrossMediaPlayer.Services.Translation;
using CrossMediaPlayer.Services.UserSettingsService;

namespace CrossMediaPlayer.ViewModels.Pages;

public partial class OptionsPageViewModel : ViewModelBase
{
    public ITranslationService TranslationService { get; }
    
    private readonly IUserSettingsService _userSettingsService;
    private readonly IMediaLibraryService _mediaLibraryService;
    
    public OptionsPageViewModel(
        ITranslationService translationService,
        IUserSettingsService userSettingsService,
        IMediaLibraryService mediaLibraryService)
    {
        TranslationService = translationService;
        _userSettingsService = userSettingsService;
        _mediaLibraryService = mediaLibraryService;

        _selectedLanguageOption = _userSettingsService.UserSettings.Language;
        _selectedMinimizeBehaviourOption = _userSettingsService.UserSettings.MinimizeBehaviour;
        _selectedDefaultTabOption = _userSettingsService.UserSettings.DefaultStartupTab;
        _selectedThemeOption = _userSettingsService.UserSettings.Theme;

        foreach (var mediaFolder in _userSettingsService.UserSettings.MediaFolders)
        {
            MediaFoldersList.Add(mediaFolder);
        }

        _mediaLibraryService.MediaSyncStatusChanged += OnMediaSyncStatusChanged;
    }
    
    [ObservableProperty]
    private LanguageOption _selectedLanguageOption;
    
    [ObservableProperty]
    private MinimizeBehaviourOption _selectedMinimizeBehaviourOption;
    
    [ObservableProperty]
    private SideMenuTab _selectedDefaultTabOption;
    
    [ObservableProperty]
    private ThemeOption _selectedThemeOption;
    
    [ObservableProperty]
    private ObservableCollection<string> _mediaFoldersList = new();
    
    [ObservableProperty]
    private string? _selectedMediaFolder;

    [ObservableProperty] 
    private bool _mediaSyncing;


    private void OnMediaSyncStatusChanged(object? sender, MediaSyncStatus mediaSyncStatus)
    {
        MediaSyncing = mediaSyncStatus != MediaSyncStatus.NotRunning;
    }
    
    partial void OnSelectedLanguageOptionChanged(LanguageOption value)
    {
        _userSettingsService.UserSettings.SaveLanguageOption(value);
        
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
        _userSettingsService.UserSettings.SaveMinimizeBehaviourOption(value);
    }

    partial void OnSelectedDefaultTabOptionChanged(SideMenuTab value)
    {
        _userSettingsService.UserSettings.SaveDefaultStartupTab(value);
    }

    partial void OnSelectedThemeOptionChanged(ThemeOption value)
    {
        _userSettingsService.UserSettings.SaveThemeOption(value);
    }

    [RelayCommand]
    public async Task MediaFoldersAddButtonClick(Button mediaFoldersAddButton)
    {
        var topLevel = TopLevel.GetTopLevel(mediaFoldersAddButton);
    
        if (topLevel != null)
        {
            var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Select Media Folder",
                AllowMultiple = false
            });

            if (folders.Count > 0)
            {
                if (!MediaFoldersList.Contains(folders.First().Path.LocalPath))
                {
                    MediaFoldersList.Add(folders.First().Path.LocalPath);
                    
                    _userSettingsService.UserSettings.SaveMediaFolders(MediaFoldersList.ToList());
                }
            }
            
            await _mediaLibraryService.SyncMediaLibrary();
        }
    }
    
    [RelayCommand]
    public void MediaFoldersRemoveButtonClick()
    {
        if (SelectedMediaFolder != null)
        {
            MediaFoldersList.Remove(SelectedMediaFolder);
            
            _userSettingsService.UserSettings.SaveMediaFolders(MediaFoldersList.ToList());
        }
    }
}