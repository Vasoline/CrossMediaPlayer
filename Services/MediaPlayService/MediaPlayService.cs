using System;
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

    public void PlayAudioTest()
    {
        if (_mediaPlayer.IsPlaying)
        {
            _mediaPlayer.Stop();
            
            return;
        }

        _mediaPlayer.Volume = 30;
        
        string audioPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "song.flac");
        var media = new Media(_libVlc, audioPath);
        _mediaPlayer.Play(media);
    }

    public void Dispose()
    {
        _mediaPlayer.Stop();
        _mediaPlayer.Dispose();
        _libVlc.Dispose();
    }
}