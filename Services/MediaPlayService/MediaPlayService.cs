using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using LibVLCSharp.Shared;

namespace CrossMediaPlayer.Services.MediaPlayService;

public class MediaPlayService : IMediaPlayService
{
    private readonly LibVLC _libVlc = new();
    private readonly MediaPlayer _mediaPlayer;
    
    public MediaPlayService()
    {
        _mediaPlayer = new MediaPlayer(_libVlc);
    }

    public VLCState MediaState()
    {
        return _mediaPlayer.State;
    }
    
    public void PlayMediaTest()
    {
        string audioPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "song.flac");
        var media = new Media(_libVlc, audioPath);
        _mediaPlayer.Play(media);
    }
    
    public void PauseMedia()
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