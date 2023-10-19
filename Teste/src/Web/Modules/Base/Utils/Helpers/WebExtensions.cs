using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Web
{
    public static class WebExtensions
    {
        public static WebAssemblyHostBuilder RegisterServices(this WebAssemblyHostBuilder builder)
        {
            var jwtSettinsSection = builder.Configuration.GetSection("URISettings:UriBase").Value;

            Console.WriteLine($"=====> {jwtSettinsSection}");

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("") });

            //builder.Services.AddBlazoredLocalStorage();
            //builder.Services.AddAuthorizationCore();

            //builder.Services.AddScoped<Core.Modules.Services.INavigationService, Web.Modules.Services.NavigationService>();
            //builder.Services.AddScoped<Core.Modules.Services.IAlertService, Web.Modules.Services.AlertService>();
            //builder.Services.AddScoped<Core.Modules.Services.IApiService, Core.Modules.Services.ApiService>();
            //builder.Services.AddScoped<Core.Modules.Services.IFileUploadService, Web.Modules.Services.FileUploadService>();
            //builder.Services.AddScoped<Core.Modules.Services.IAuthService, Web.Modules.Services.AuthService>();
            //builder.Services.AddScoped<AuthenticationStateProvider, Web.Modules.Services.TokenAuthenticationStateProvider>();
            //builder.Services.AddScoped<IConnectivityService, Web.Modules.Services.ConnectivityService>();

            return builder;
        }

    }
}
