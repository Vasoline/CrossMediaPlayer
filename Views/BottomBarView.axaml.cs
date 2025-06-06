using Avalonia.Controls;
using CrossMediaPlayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CrossMediaPlayer.Views;

public partial class BottomBarView : UserControl
{
    public BottomBarView()
    {
        InitializeComponent();
        
        DataContext = App.ServiceProvider?.GetRequiredService<BottomBarViewModel>();
    }
}