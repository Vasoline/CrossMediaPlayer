using Avalonia.Controls;
using CrossMediaPlayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CrossMediaPlayer.Views;

public partial class ContentView : UserControl
{
    public ContentView()
    {
        InitializeComponent();
        
        DataContext = App.ServiceProvider?.GetRequiredService<ContentViewModel>();
    }
}