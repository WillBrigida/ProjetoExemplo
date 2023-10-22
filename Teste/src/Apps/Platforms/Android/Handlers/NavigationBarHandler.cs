using Android.Views;
using Microsoft.Maui.Platform;
using Apps.Handlers;

namespace Apps.Droid
{
    public class NavigationBarHandler : INavigationBarHandler
    {
        Android.Views.Window window = null;

        public NavigationBarHandler()
        {
            window = Platform.CurrentActivity.Window;
        }

        public void SetColor(Color color)
        {
            if (color is not null)
                window.SetNavigationBarColor(color.ToPlatform());
        }

        public void SetStyle(eNavigationBarStyle navigationBarStyle)
        {
            int navigationBarBarUiVisibility = (int)window.DecorView.SystemUiVisibility;

            switch (navigationBarStyle)
            {
                case eNavigationBarStyle.LightContent:
                    navigationBarBarUiVisibility &= ~(int)SystemUiFlags.LightNavigationBar;

                    break;

                case eNavigationBarStyle.DarkContent:
                    navigationBarBarUiVisibility |= (int)SystemUiFlags.LightNavigationBar;

                    break;

                default:
                    if (AppInfo.RequestedTheme == AppTheme.Light)
                        //Dark Text to show up on your light status bar
                        navigationBarBarUiVisibility |= (int)SystemUiFlags.LightNavigationBar;

                    else if (AppInfo.RequestedTheme == AppTheme.Dark)
                        //Light Text to show up on your dark status bar
                        navigationBarBarUiVisibility &= ~(int)SystemUiFlags.LightNavigationBar;

                    break;
            }

            Platform.CurrentActivity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)navigationBarBarUiVisibility;

        }
    }
}

