using Android.Views;
using Microsoft.Maui.Platform;
using Apps.Handlers;

namespace Apps.Droid
{
    public class StatusBarHandler : IStatusBarHandler
    {
        Android.Views.Window window = null;

        public StatusBarHandler()
        {
            window = Platform.CurrentActivity.Window;

        }

        public void SetColor(Color color)
        {
            window.SetStatusBarColor(color.ToPlatform());
            window.ClearFlags(Android.Views.WindowManagerFlags.LayoutNoLimits);
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

            //window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);
            //window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
            //window.SetStatusBarColor(Android.Graphics.Color.Transparent);

            //window.DecorView.SetBackgroundColor(Android.Graphics.Color.Transparent); //Usado para evitar que a tela fique piscando quando feito navegaçãp para outra tela
        }

        public void SetStyle(eStatusBarStyle statusBarStyle)
        {
            int statusBarUiVisibility = (int)window.DecorView.SystemUiVisibility;

            switch (statusBarStyle)
            {
                case eStatusBarStyle.LightContent:
                    statusBarUiVisibility &= ~(int)SystemUiFlags.LightStatusBar;
                    break;

                case eStatusBarStyle.DarkContent:
                    statusBarUiVisibility |= (int)SystemUiFlags.LightStatusBar;
                    break;

                default:
                    if (AppInfo.RequestedTheme == AppTheme.Light)
                        //Dark Text to show up on your light status bar
                        statusBarUiVisibility |= (int)SystemUiFlags.LightStatusBar;

                    else if (AppInfo.RequestedTheme == AppTheme.Dark)
                        //Light Text to show up on your dark status bar
                        statusBarUiVisibility &= ~(int)SystemUiFlags.LightStatusBar;

                    break;
            }

            Platform.CurrentActivity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)statusBarUiVisibility;

        }
    }
}

