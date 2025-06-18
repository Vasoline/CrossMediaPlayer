using Avalonia.Controls;
using CrossMediaPlayer.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace CrossMediaPlayer.Views.Pages;

public partial class MediaFoldersPageView : UserControl
{
    public MediaFoldersPageView()
    {
        InitializeComponent();
        
        DataContext = App.ServiceProvider?.GetRequiredService<MediaFoldersPageViewModel>();
    }
}