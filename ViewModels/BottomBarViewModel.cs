using System;
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
        
        OnVolumeChanged(Volume);
    }
    
    [ObservableProperty]
    private double _volume = 80;

    [RelayCommand]
    public void PlayButton()
    {
        switch (_mediaPlayService.MediaState())
        {
            case VLCState.Playing:
            case VLCState.Paused:
                _mediaPlayService.PauseMedia();
                break;
            default:
                _mediaPlayService.PlayMediaTest();
                break;
        }
    }
    
    partial void OnVolumeChanged(double value)
    {
        var volumeAsInt = (int)Math.Clamp(value, 0, 100);
        
        _mediaPlayService.ChangeVolume(volumeAsInt);
    }
}