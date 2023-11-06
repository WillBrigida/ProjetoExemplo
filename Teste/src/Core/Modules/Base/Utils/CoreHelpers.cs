using Core.Modules.Models;
using Core.Modules.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Core
{
    public class CoreHelpers
    {
        public static UserDTO? PrincipalUser
        {
            get
            {
                var _localStorageService = ServiceProvider!.GetRequiredService<ILocalStorageService>();

                var json = _localStorageService!.Get("PrincipalUser", string.Empty);

                if (string.IsNullOrEmpty(json.ToString()))
                    return new();

                return JsonSerializer.Deserialize<UserDTO>(json.ToString()!);
            }
        }

        public static void ClearPrincipalUser()
        {
            var _localStorageService = ServiceProvider!.GetRequiredService<ILocalStorageService>();
            _localStorageService!.Remove("PrincipalUser");
        }

        public static IServiceProvider? ServiceProvider { get; set; }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static T GetSection<T>(string key)
            => Config.GetSection(key).Get<T>() ??
                throw new ArgumentNullException("Não foi encontrado o valor com a chave informada. Verifique o arquivo appsettings");


        public static string GetSection(string key)
            => Config.GetSection(key).Value ??
                throw new ArgumentNullException("Não foi encontrado o valor com a chave informada. Verifique o arquivo appsettings");


        private static IConfigurationRoot Config
        {
            //ref:https://github.dev/jamesmontemagno/dotnet-maui-configuration
            get
            {
                var a = System.Reflection.Assembly.GetExecutingAssembly();
                using var stream = a.GetManifestResourceStream("Core.appsettings.json");
                return new ConfigurationBuilder().AddJsonStream(stream!).Build();
            }
        }

        public static string? BaseUri => GetSection("BaseUri");
    }
}
