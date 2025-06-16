using System;
using Avalonia.Controls;

namespace CrossMediaPlayer.Services.AppNavigation;

public interface IAppNavigationService
{
    UserControl? CurrentlySelectedContentsPage { get; }
    event EventHandler<UserControl>? ContentsPageChanged;
    void SetContentsPage(UserControl newlySelectedPage);
}