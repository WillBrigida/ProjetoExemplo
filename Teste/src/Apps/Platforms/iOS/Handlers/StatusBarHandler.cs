using Foundation;
using Microsoft.Maui.Platform;
using Apps.Handlers;
using UIKit;

namespace Apps.Platforms.iOS.Handlers
{
    public class StatusBarHandler : IStatusBarHandler
    {
        public void SetColor(Color color)
        {
            //ref: https://blog.verslu.is/maui/change-maui-ios-status-bar-color/

            UIView statusBar;
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                int tag = 4567890;

                UIWindow window = UIApplication.SharedApplication.Delegate.GetWindow();
                statusBar = window.ViewWithTag(tag);

                if (statusBar == null || statusBar.Frame != UIApplication.SharedApplication.StatusBarFrame)
                {
                    statusBar = statusBar ?? new(UIApplication.SharedApplication.StatusBarFrame);
                    statusBar.Frame = UIApplication.SharedApplication.StatusBarFrame;
                    statusBar.Tag = tag;

                    window.AddSubview(statusBar);
                }
            }
            else
                statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;

            statusBar.BackgroundColor = color.ToPlatform();
        }

        public void SetStyle(eStatusBarStyle StatusBarStyle)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                switch (StatusBarStyle)
                {
                    case eStatusBarStyle.LightContent:
                        UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true); break;

                    case eStatusBarStyle.DarkContent:
                        UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.DarkContent, true); break;

                    default: break;
                }
            }
        }
    }
}

