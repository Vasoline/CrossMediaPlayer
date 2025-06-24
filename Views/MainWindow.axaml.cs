using System;
using Avalonia.Controls;

namespace CrossMediaPlayer.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    protected override async void OnClosing(WindowClosingEventArgs e)
    {
        try
        {
            base.OnClosing(e);
        
            if (DataContext is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
            else
            {
                (DataContext as IDisposable)?.Dispose();
            }
        }
        catch (Exception exception)
        {
            // handle exception
        }
    }
}