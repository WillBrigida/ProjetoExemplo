namespace Apps
{
    internal class AppsHelpers
    {
        public static bool IsiOS => DeviceInfo.Platform == DevicePlatform.iOS;
        public static bool IsDroid => DeviceInfo.Platform == DevicePlatform.Android;
        public static bool HasThemeLight => AppInfo.RequestedTheme == AppTheme.Light;
        public static bool IsSimulator => DeviceInfo.DeviceType == DeviceType.Virtual;
    }
}
