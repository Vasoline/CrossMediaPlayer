using Avalonia.Controls;
using CrossMediaPlayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CrossMediaPlayer.Views;

public partial class SideBarView : UserControl
{
    public SideBarView()
    {
        InitializeComponent();
        
        DataContext = App.ServiceProvider?.GetRequiredService<SideBarViewModel>();
    }
}