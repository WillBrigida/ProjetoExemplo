using Core.Modules.Services;

namespace Apps.Modules.Services
{
    public class LauncherService : ILauncherService
    {
        public async Task OpenAsync(string url)
        {
            if (url.Contains("&amp;"))
                url = url.Replace("&amp;", "&");

            if (AppsHelpers.IsSimulator && AppsHelpers.IsDroid)
                if (url.Contains("localhost"))
                    url = url.Replace("localhost", "10.0.2.2");

            await Launcher.OpenAsync(url);
        }
    }
}
