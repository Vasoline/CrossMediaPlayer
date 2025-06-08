using System;
using LibVLCSharp.Shared;

namespace CrossMediaPlayer.Services.MediaPlayService;

public interface IMediaPlayService : IDisposable
{
    public VLCState MediaState();
    
    public void PlayMediaTest();
    
    /// <summary>
    /// Toggles Pause On/Off
    /// </summary>
    public void PauseMedia();
    
    public void ChangeVolume(int volume);
}