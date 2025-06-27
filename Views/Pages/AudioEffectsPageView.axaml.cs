using Avalonia.Controls;
using CrossMediaPlayer.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace CrossMediaPlayer.Views.Pages;

public partial class AudioEffectsPageView : UserControl
{
    public AudioEffectsPageView()
    {
        InitializeComponent();
        
        DataContext = App.ServiceProvider?.GetRequiredService<AudioEffectsPageViewModel>();
    }
}