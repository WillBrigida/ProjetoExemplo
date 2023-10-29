using Core.Modules.Services;

namespace Apps.Modules.Services
{
    public class LauncherService : ILauncherService
    {
        public async Task OpenAsync(string url)
        {
            if (url.Contains("&amp;"))
                url = url.Replace("&amp;", "&");

            if (AppsHelpers.IsSimulator)
            {
                if (url.Contains("localhost"))
                {
                    url = url.Replace("localhost", "10.0.2.2");
                    //Test Deep link
                    //url = url.Replace("http://localhost:5225", "https://willbrigida.com.br");
                    //await Clipboard.Default.SetTextAsync(url);
                }
            }
            await Launcher.OpenAsync(url);
        }
    }
}
