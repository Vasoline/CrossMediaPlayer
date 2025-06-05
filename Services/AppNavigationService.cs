using LibVLCSharp.Shared;

namespace CrossMediaPlayer.Services;

public class AppNavigationService : IAppNavigationService
{
    public AppNavigationService()
    {
        Core.Initialize();
        var libVlc = new LibVLC();
    }
    
    
}