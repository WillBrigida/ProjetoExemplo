namespace Apps
{
    internal class AppsColors
    {
        internal static Color ACCENT_COLOR => Colors.Orange;//(Color)(App.Current.Resources.MergedDictionaries.First())["AccentColor"] ?? Colors.Orange;
        internal static Color TEXT_COLOR => Colors.White; //(Color)(App.Current.Resources.MergedDictionaries.First())[AppsHelpers.HasThemeLight ? "TextColorThemeLight" : "TextColorThemetDark"] ?? Colors.Orange;
        internal static Color BACKGROUD_COLOR => Colors.Black; //(Color)(App.Current.Resources.MergedDictionaries.First())[AppsHelpers.HasThemeLight ? "BackgroundColorThemeLight" : "BackgroundColorThemeDark"] ?? Colors.Orange;
    }
}
