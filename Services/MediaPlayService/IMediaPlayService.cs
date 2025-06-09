using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace CrossMediaPlayer.Services.MediaPlayService;

public interface IMediaPlayService : IDisposable
{
    public event EventHandler<MediaPlayerLengthChangedEventArgs> LengthChanged;
    public event EventHandler<MediaPlayerTimeChangedEventArgs> TimeChanged;
    
    public VLCState MediaState();

    public Task<string> PlayMediaTest(string audioFilePath);

    public void MediaSeekToPosition(TimeSpan seekTime);
    
    public void PauseMedia();
    
    public void ResumeMedia();
    
    public void ChangeVolume(int volume);

    public void ArmPlaylist(Dictionary<int, string> playlist);
}