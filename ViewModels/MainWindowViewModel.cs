using System;
using CrossMediaPlayer.Services.MediaPlayService;

namespace CrossMediaPlayer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IDisposable
{
    public BottomBarViewModel BottomBar { get; }
    
    private readonly IMediaPlayService _mediaPlayService;
    
    public MainWindowViewModel(BottomBarViewModel bottomBar, IMediaPlayService mediaPlayService)
    {
        BottomBar = bottomBar;
        _mediaPlayService = mediaPlayService;
    }
    
    public void Dispose()
    {
        _mediaPlayService.Dispose();
    }
}
