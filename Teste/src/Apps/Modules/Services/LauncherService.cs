using Core.Modules.Services;

namespace Apps.Modules.Services
{
    public class LauncherService : ILauncherService
    {
        public async Task OpenAsync(string url)
        {
            var newUrl = url.Replace("&amp;", "&");
            if (AppsHelpers.IsSimulator && AppsHelpers.IsDroid)
            {
                if (url.Contains("localhost"))
                    newUrl = newUrl.Replace("localhost", "10.0.2.2");
            }
            await Launcher.OpenAsync(newUrl);
        }
    }
}
