using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Apps
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]


    //[IntentFilter(new[] { Android.Content.Intent.ActionView },
    //                    DataScheme = "http",
    //                    DataHost = "willbrigida.com.br",
    //                    DataPathPrefix = "/Account",
    //                    AutoVerify = true,
    //                    Categories = new[] { Android.Content.Intent.ActionView, Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]

    //[IntentFilter(new[] { Android.Content.Intent.ActionView },
    //                    DataScheme = "https",
    //                    DataHost = "willbrigida.com.br",
    //                    DataPathPrefix = "/Account",
    //                    AutoVerify = true,
    //                    Categories = new[] { Android.Content.Intent.ActionView, Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]

    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Intent intent = this.Intent;
            var action = intent.Action;
            var strLink = intent.DataString;

            if (Intent.ActionView == action && !string.IsNullOrWhiteSpace(strLink))
            {
                var link = new Uri(strLink);
                App.Current.SendOnAppLinkRequestReceived(link);
            }

        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            var action = intent.Action;
            var strLink = intent.DataString;
            if (Intent.ActionView == action && !string.IsNullOrWhiteSpace(strLink))
            {
                var link = new Uri(strLink);
                App.Current.SendOnAppLinkRequestReceived(link);
            };
        }
    }
}
