using System.Threading.Tasks;
using CrossMediaPlayer.Models;

namespace CrossMediaPlayer.Services.UserSettingsService;

public interface IUserSettingsService
{
    public UserSettings UserSettings { get; }
    public bool UserSettingsLoaded();
    public void LoadUserSettings();
    public Task SaveUserSettings();
}