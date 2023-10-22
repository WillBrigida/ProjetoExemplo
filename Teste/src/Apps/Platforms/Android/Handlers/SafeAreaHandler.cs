using Apps.Handlers;

namespace Apps.Droid
{
    public class SafeAreaHandler : ISafeAreaHandler
    {
        public void SetFullArea()
        {
            var window = Platform.CurrentActivity.Window;

            window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);
        }
    }
}

