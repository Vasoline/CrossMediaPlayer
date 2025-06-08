using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrossMediaPlayer.Services.MediaPlayService;
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

    [RelayCommand]
    public async Task PlayButton()
    {
        switch (_mediaPlayService.MediaState())
        {
            case VLCState.Playing:
                _mediaPlayService.PauseMedia();
                break;
            case VLCState.Paused:
                _mediaPlayService.ResumeMedia();
                break;
            default:
                CurrentlyPlayingMedia = await _mediaPlayService.PlayMediaTest();
                break;
        }
    }
    
    private void OnLengthChanged(object? sender, MediaPlayerLengthChangedEventArgs newLength)
    {
        var timeSpan = TimeSpan.FromMilliseconds(newLength.Length);
        
        MediaLength = $"{timeSpan.Minutes}:{timeSpan.Seconds:D2}";
    }
    
    private void OnTimeChanged(object? sender, MediaPlayerTimeChangedEventArgs newTime)
    {
        var timeSpan = TimeSpan.FromMilliseconds(newTime.Time);
        
        MediaPlayingTime = $"{timeSpan.Minutes}:{timeSpan.Seconds:D2}";
    }
    
    partial void OnVolumeChanged(double value)
    {
        var volumeAsInt = (int)Math.Clamp(value, 0, 100);
        
        _mediaPlayService.ChangeVolume(volumeAsInt);
    }
}