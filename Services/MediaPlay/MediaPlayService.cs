using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace CrossMediaPlayer.Services.MediaPlay;

public class MediaPlayService : IMediaPlayService
{
    private readonly LibVLC _libVlc = new();
    private readonly MediaPlayer _mediaPlayer;
    
    public MediaPlayService()
    {
        _mediaPlayer = new MediaPlayer(_libVlc);
    }
    
    public event EventHandler<MediaPlayerLengthChangedEventArgs> LengthChanged
    {
        add => _mediaPlayer.LengthChanged += value;
        remove => _mediaPlayer.LengthChanged -= value;
    }
    
    public event EventHandler<MediaPlayerTimeChangedEventArgs> TimeChanged
    {
        add => _mediaPlayer.TimeChanged += value;
        remove => _mediaPlayer.TimeChanged -= value;
    }
    
    public event EventHandler<EventArgs> MediaEnded
    {
        add => _mediaPlayer.EndReached += value;
        remove => _mediaPlayer.EndReached -= value;
    }

    public VLCState MediaState()
    {
        return _mediaPlayer.State;
    }
    
    public async Task<string> PlayMediaTest(string audioFilePath)
    {
        // Once the library is built we would check that a song is loaded before we attempt to play.
        // Loading a song is done elsewhere, such as by double-clicking on the song in your library.
        string audioPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "song.flac");
        var media = new Media(_libVlc, audioPath);
        
        await media.Parse();
        
        var artistName = media.Meta(MetadataType.Artist);
        var trackName = media.Meta(MetadataType.Title);
        
        _mediaPlayer.Play(media);
        
        return $"{artistName} - {trackName}";
    }
    
    public void MediaSeekToPosition(TimeSpan seekTime)
    {
        _mediaPlayer.SeekTo(seekTime);
    }
    
    public void PauseMedia()
    {
        _mediaPlayer.Pause();
    }
    
    public void ResumeMedia()
    {
        _mediaPlayer.Pause();
    }

    public void ChangeVolume(int volume)
    {
        _mediaPlayer.Volume = volume;
    }

    public void Dispose()
    {
        _mediaPlayer.Stop();
        _mediaPlayer.Dispose();
        _libVlc.Dispose();
    }
}