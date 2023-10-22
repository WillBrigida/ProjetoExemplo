using Core;
using Microsoft.Extensions.Logging;

namespace Apps
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if IOS
        builder.RegisterHandlersiOS();

#elif ANDROID
            builder.RegisterHandlersAndroid();
#endif
            builder.RegisterServices();
            builder.Services.RegisterViewModels();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            Core.CoreHelpers.ServiceProvider = builder.Services.BuildServiceProvider();

            return builder.Build();
        }
    }
}
