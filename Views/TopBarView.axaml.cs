using Avalonia.Controls;
using CrossMediaPlayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CrossMediaPlayer.Views;

public partial class TopBarView : UserControl
{
    public TopBarView()
    {
        InitializeComponent();
        
        DataContext = App.ServiceProvider?.GetRequiredService<TopBarViewModel>();
    }
}