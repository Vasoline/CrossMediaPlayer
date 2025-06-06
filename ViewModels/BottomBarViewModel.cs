using CommunityToolkit.Mvvm.Input;
using CrossMediaPlayer.Services;

namespace CrossMediaPlayer.ViewModels;

public partial class BottomBarViewModel : ViewModelBase
{
    private readonly IMediaPlayService _mediaPlayService;
    
    public BottomBarViewModel(IMediaPlayService mediaPlayService)
    {
        _mediaPlayService = mediaPlayService;
    }

    [RelayCommand]
    public void PlayButton()
    {
        _mediaPlayService.PlayAudioTest();
    }
}