using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrossMediaPlayer.Services.MediaPlay;
using LibVLCSharp.Shared;

namespace CrossMediaPlayer.ViewModels;

public partial class BottomBarViewModel : ViewModelBase
{
    private readonly IMediaPlayService _mediaPlayService;
    
    public BottomBarViewModel(IMediaPlayService mediaPlayService)
    {
        _mediaPlayService = mediaPlayService;
        
        _mediaPlayService.LengthChanged += OnLengthChanged;
        _mediaPlayService.TimeChanged += OnTimeChanged;
        _mediaPlayService.MediaEnded += OnMediaEnded;
        
        OnVolumeChanged(Volume);
    }
    
    [ObservableProperty]
    private double _volume = 80;
    
    [ObservableProperty]
    private string _mediaLength = "--:--";
    
    [ObservableProperty]
    private string _mediaPlayingTime = "--:--";
    
    [ObservableProperty]
    private string _currentlyPlayingMedia = String.Empty;
    
    [ObservableProperty]
    private bool _enableSeeking;
    
    [ObservableProperty]
    private double _mediaSeekPosition;
    
    [ObservableProperty]
    private string _playButtonIconPathData = PlayIconPatchData;
    
    private const string PlayIconPatchData = "M 0,0 L 0,10 L 10,5 Z";
    private const string PauseIconPatchData = "M 0,0 L 4,0 L 4,10 L 0,10 Z M 6,0 L 10,0 L 10,10 L 6,10 Z";
    
    private double _currentMediaLengthInMilliseconds;
    private bool _automaticSeekUpdate;
    
    [RelayCommand]
    public async Task PlayButton()
    {
        switch (_mediaPlayService.MediaState())
        {
            case VLCState.Playing:
                _mediaPlayService.PauseMedia();
                PlayButtonIconPathData = PlayIconPatchData;
                break;
            case VLCState.Paused:
                _mediaPlayService.ResumeMedia();
                PlayButtonIconPathData = PauseIconPatchData;
                break;
            default:
                CurrentlyPlayingMedia = await _mediaPlayService.PlayMediaTest(String.Empty);
                MediaSeekPosition = 0;
                PlayButtonIconPathData = PauseIconPatchData;
                EnableSeeking = true;
                break;
        }
    }
    
    private void OnLengthChanged(object? sender, MediaPlayerLengthChangedEventArgs newLength)
    {
        _currentMediaLengthInMilliseconds = newLength.Length;
        
        var timeSpan = TimeSpan.FromMilliseconds(newLength.Length);
        
        MediaLength = $"{timeSpan.Minutes}:{timeSpan.Seconds:D2}";
    }
    
    private void OnTimeChanged(object? sender, MediaPlayerTimeChangedEventArgs newTime)
    {
        var currentMediaTimeInMilliseconds = newTime.Time;
        
        var timeSpan = TimeSpan.FromMilliseconds(newTime.Time);
        
        MediaPlayingTime = $"{timeSpan.Minutes}:{timeSpan.Seconds:D2}";
        
        _automaticSeekUpdate = true;
        MediaSeekPosition = (currentMediaTimeInMilliseconds / _currentMediaLengthInMilliseconds) * 100.0;
        _automaticSeekUpdate = false;
    }

    private void OnMediaEnded(object? sender, EventArgs eventArgs)
    {
        _automaticSeekUpdate = true;
        
        MediaPlayingTime = MediaLength = "--:--";
        CurrentlyPlayingMedia = String.Empty;
        MediaSeekPosition = 0;
        PlayButtonIconPathData = PlayIconPatchData;
        EnableSeeking = false;
        
        _automaticSeekUpdate = false;
    }
    
    partial void OnVolumeChanged(double value)
    {
        var volumeAsInt = (int)Math.Clamp(value, 0, 100);
        
        _mediaPlayService.ChangeVolume(volumeAsInt);
    }
    
    partial void OnMediaSeekPositionChanged(double value)
    {
        // We have to check this so we can distinguish when the user actually clicks on the seek bar
        // only seek if the user changed the value.
        if (!_automaticSeekUpdate && EnableSeeking && _mediaPlayService.MediaState() != VLCState.Ended)
        {
            var newTimeInMilliseconds = (value / 100.0) * _currentMediaLengthInMilliseconds;
            var newTimeSpan = TimeSpan.FromMilliseconds(newTimeInMilliseconds);
        
            _mediaPlayService.MediaSeekToPosition(newTimeSpan);
            
            MediaPlayingTime = $"{newTimeSpan.Minutes}:{newTimeSpan.Seconds:D2}";
        }
    }
}