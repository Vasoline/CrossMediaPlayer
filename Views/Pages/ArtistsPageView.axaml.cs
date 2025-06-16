using Avalonia.Controls;
using CrossMediaPlayer.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace CrossMediaPlayer.Views.Pages;

public partial class ArtistsPageView : UserControl
{
    public ArtistsPageView()
    {
        InitializeComponent();
        
        DataContext = App.ServiceProvider?.GetRequiredService<ArtistsPageViewModel>();
    }
}