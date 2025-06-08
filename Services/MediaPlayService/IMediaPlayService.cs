using System;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace CrossMediaPlayer.Services.MediaPlayService;

public interface IMediaPlayService : IDisposable
{
    public event EventHandler<MediaPlayerLengthChangedEventArgs> LengthChanged;
    public event EventHandler<MediaPlayerTimeChangedEventArgs> TimeChanged;
    
    public VLCState MediaState();

    public long GetMediaLength();

    public Task<string> PlayMediaTest();

    public void MediaSeekToPosition(TimeSpan seekTime);
    
    public void PauseMedia();
    
    public void ResumeMedia();
    
    public void ChangeVolume(int volume);
}