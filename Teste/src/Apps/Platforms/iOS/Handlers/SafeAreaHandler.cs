using Apps.Handlers;
using UIKit;

namespace Apps.Platforms.iOS.Handlers
{
    public class SafeAreaHandler : ISafeAreaHandler
    {
        public double GetBottomArea()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                UIWindow window = UIApplication.SharedApplication.Delegate.GetWindow();
                var bottomPadding = window.SafeAreaInsets.Bottom;
                return bottomPadding;
            }
            return 0;
        }

        public double GetTopArea()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                UIWindow window = UIApplication.SharedApplication.Delegate.GetWindow();
                var TopPadding = window.SafeAreaInsets.Top;
                return TopPadding;
            }
            return 0;
        }
    }
}


