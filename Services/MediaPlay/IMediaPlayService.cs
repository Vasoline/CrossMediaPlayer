using System;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace CrossMediaPlayer.Services.MediaPlay;

public interface IMediaPlayService : IDisposable
{
    public event EventHandler<MediaPlayerLengthChangedEventArgs> LengthChanged;
    public event EventHandler<MediaPlayerTimeChangedEventArgs> TimeChanged;
    public event EventHandler<EventArgs> MediaEnded;
    
    public VLCState MediaState();

    public Task<string> PlayMediaTest(string audioFilePath);

    public void MediaSeekToPosition(TimeSpan seekTime);
    
    public void PauseMedia();
    
    public void ResumeMedia();
    
    public void ChangeVolume(int volume);
}