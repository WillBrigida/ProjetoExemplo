namespace Apps
{
    public static class AppsExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

#if ANDROID

        public static MauiAppBuilder RegisterHandlersAndroid(this MauiAppBuilder builder)
        {
            //EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, v)
            //    => h.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToPlatform()));

            //EditorHandler.Mapper.AppendToMapping("NoUnderline", (h, v)
            //    => h.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToPlatform()));

            //PickerHandler.Mapper.AppendToMapping("NoUnderline", (h, v)
            //   => h.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToPlatform()));

            //DatePickerHandler.Mapper.AppendToMapping("NoUnderline", (h, v)
            //   => h.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToPlatform()));

            //RefreshViewHandler.Mapper.AppendToMapping("RefreshView", (h, v)
            //=> h.PlatformView.SetProgressViewOffset(false, -20, 80));

            //builder.Services.AddSingleton<Handlers.IStatusBarHandler, Droid.StatusBarHandler>();

            //builder.Services.AddSingleton<Handlers.INavigationBarHandler, Droid.NavigationBarHandler>();

            //builder.Services.AddSingleton<Handlers.ISafeAreaHandler, Droid.SafeAreaHandler>();

            //builder.ConfigureMauiHandlers((handlers) =>
            //{
            //    handlers.AddHandler(typeof(Shell), typeof(Platforms.Android.Renderers.CustomShellRenderer));
            //});


            return builder;
        }

#elif IOS || MACCATALYST

        public static MauiAppBuilder RegisterHandlersiOS(this MauiAppBuilder builder)
        {
            //EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
            //{
            //    h.PlatformView.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            //    h.PlatformView.BorderStyle = UITextBorderStyle.None;
            //});

            //EditorHandler.Mapper.AppendToMapping("NoUnderline", (h, v)
            //    => h.PlatformView.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0));

            //PickerHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
            //{
            //    h.PlatformView.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            //    h.PlatformView.BorderStyle = UITextBorderStyle.None;
            //});

            //DatePickerHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
            //{
            //    h.PlatformView.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            //    //h.PlatformView.BorderStyle = UITextBorderStyle.None;
            //});

            //builder.Services.AddSingleton<Handlers.IStatusBarHandler, Platforms.iOS.Handlers.StatusBarHandler>();

            //builder.Services.AddSingleton<Handlers.ISafeAreaHandler, Platforms.iOS.Handlers.SafeAreaHandler>();

            return builder;
        }
#endif

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            var baseUri = Core.CoreHelpers.GetSection("BaseUri");
#if DEBUG
            var host = baseUri.Split("//")[1];
            var port = host.Contains(':') ? baseUri.Split(':')[2] : string.Empty;
            baseUri = AppsHelpers.IsDroid ? $"https://10.0.2.2:{port}" : baseUri;
#endif
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUri) });

            builder.Services.AddSingleton<Core.Modules.Services.IApiService, Core.Modules.Services.ApiService>();

            //builder.Services.AddSingleton<Core.Modules.Services.INavigationService, Modules.Services.NavigationService>();
            //builder.Services.AddSingleton<Core.IDeviceInfoService, Modules.Services.DeviceInfoService>();
            //builder.Services.AddSingleton<Core.Modules.Services.IAlertService, Modules.Services.AlertService>();
            //builder.Services.AddSingleton<Core.Modules.Services.IFileUploadService, Modules.Services.FileUploadService>();
            //builder.Services.AddSingleton<Core.Modules.Services.IConnectivityService, Modules.Services.ConnectivityService>();
            //builder.Services.AddSingleton<Core.Modules.Services.IAuthService, Modules.Services.AuthService>();
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(AppHelpers.BaseAddress) });

            return builder;
        }
    }
}

