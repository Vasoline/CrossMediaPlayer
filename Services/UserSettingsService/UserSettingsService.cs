using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia.Controls;
using CrossMediaPlayer.Enums;
using CrossMediaPlayer.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace CrossMediaPlayer.Services.UserSettingsService;

public class UserSettingsService : IUserSettingsService
{
    public UserSettings UserSettings { get; private set; } = new();
    
    private readonly string _applicationDataPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "CrossMediaPlayer",
        "CrossMediaPlayerSettings.json"
    );
    
    private bool _userSettingsLoadedBool = false;

    public bool UserSettingsLoaded()
    {
        return _userSettingsLoadedBool;
    }
    
    public void LoadUserSettings()
    {
        if (!File.Exists(_applicationDataPath))
        {
            _userSettingsLoadedBool = true;
            
            return;
        }
        
        var userSettingsJson = File.ReadAllText(_applicationDataPath);
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        
        UserSettings = JsonSerializer.Deserialize<UserSettings>(userSettingsJson, options) ?? new UserSettings();
        
        _userSettingsLoadedBool = true;
    }
    
    public async Task SaveUserSettings()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(
            _applicationDataPath) ?? throw new Exception("Application data path not found."));
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };
        
        string userSettingsJson = JsonSerializer.Serialize(UserSettings, options);

        try
        { 
            await File.WriteAllTextAsync(_applicationDataPath, userSettingsJson);
        }
        catch (Exception exception)
        {
            // log error
        }
    }
}