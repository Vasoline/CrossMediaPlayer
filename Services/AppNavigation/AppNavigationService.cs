using System;
using Avalonia.Controls;

namespace CrossMediaPlayer.Services.AppNavigation;

public class AppNavigationService : IAppNavigationService
{
    public UserControl? CurrentlySelectedContentsPage { get; private set; }
    public event EventHandler<UserControl>? ContentsPageChanged;
    
    public void SetContentsPage(UserControl newlySelectedPage)
    {
        if (CurrentlySelectedContentsPage?.GetType() == newlySelectedPage.GetType())
        {
            // We are already on this page
            return;
        }
        
        CurrentlySelectedContentsPage = newlySelectedPage;
        
        ContentsPageChanged?.Invoke(this, newlySelectedPage);
    }
}