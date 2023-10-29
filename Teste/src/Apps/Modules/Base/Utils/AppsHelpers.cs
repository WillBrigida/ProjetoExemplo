namespace Apps
{
    internal class AppsHelpers
    {
        public static bool IsiOS => DeviceInfo.Platform == DevicePlatform.iOS;
        public static bool IsDroid => DeviceInfo.Platform == DevicePlatform.Android;
        public static bool HasThemeLight => AppInfo.RequestedTheme == AppTheme.Light;
        public static bool IsSimulator => DeviceInfo.DeviceType == DeviceType.Virtual;
        public static List<NavigationItem> TabItems => new List<NavigationItem>
        {
            new NavigationItem
            {
                Title = "Home",
                Route = nameof(Modules.Views.HomePage),
                Template = new DataTemplate(typeof(Modules.Views.HomePage)),
                Icon = AppsIcons.Home
            },
            new NavigationItem
            {
                Title = "Meus dados",
                Route = nameof(Modules.Views.AccountPage),
                Template = new DataTemplate(typeof(Modules.Views.AccountPage)),
                Icon = AppsIcons.Map
            }
        };
    }

    public class NavigationItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public DataTemplate Template { get; set; }
        public string Route { get; set; }

        //public Color Color { get; set; } = Colors.Red;

        //public ImageSource Image { get; set; }
        //public List<ShellContent> Tabs { get; set; }

        //public bool HasImage => Image != null;
        //public bool HasNavigation => Template != null;
        //public bool HasTabs => Tabs != null;
    }
}
