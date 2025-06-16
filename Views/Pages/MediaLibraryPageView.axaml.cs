using Avalonia.Controls;
using CrossMediaPlayer.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace CrossMediaPlayer.Views.Pages;

public partial class MediaLibraryPageView : UserControl
{
    public MediaLibraryPageView()
    {
        InitializeComponent();
        
        DataContext = App.ServiceProvider?.GetRequiredService<MediaLibraryPageViewModel>();
    }
}