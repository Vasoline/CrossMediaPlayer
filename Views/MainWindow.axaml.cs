using System;
using Avalonia.Controls;

namespace CrossMediaPlayer.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    protected override void OnClosing(WindowClosingEventArgs e)
    {
        base.OnClosing(e);
        (DataContext as IDisposable)?.Dispose();
    }
}