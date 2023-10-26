using Core.Modules.Services;

namespace Apps.Modules.Services
{
    public class LauncherService : ILauncherService
    {
        public async Task OpenAsync(string url)
        {
            var newUrl = url.Replace("&amp;", "&");
            //#if ANDROID

            //            newUrl = url.Replace("localhost", "10.0.2.2");
            //#endif
            await Launcher.OpenAsync(newUrl);
        }
    }
}
