using Avalonia.Controls;
using CrossMediaPlayer.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace CrossMediaPlayer.Views.Pages;

public partial class PlaylistsPageView : UserControl
{
    public PlaylistsPageView()
    {
        InitializeComponent();
        
        DataContext = App.ServiceProvider?.GetRequiredService<PlaylistsPageViewModel>();
    }
}