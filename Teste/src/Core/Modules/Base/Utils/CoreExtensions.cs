using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core
{
    public static class CoreExtensions
    {
        public static string GetPageName(this string pageViewModelName)
        {
            return pageViewModelName.EndsWith("PageViewModel")
                ? pageViewModelName.Substring(0, pageViewModelName.Length - "PageViewModel".Length)
                : pageViewModelName;
        }

        public static T? GetRawValue<T>(this Enum value)
        {
            Type enumType = value.GetType();
            FieldInfo? enumField = enumType?.GetField(value.ToString());
            RawValueAttribute[]? rawValue = (RawValueAttribute[])enumField!.GetCustomAttributes<RawValueAttribute>(false);
            var result = rawValue.Length > 0 ? rawValue[0].Value : null;
            return (T)result!;
        }

        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {

            //services.AddTransient<HomePageViewModel>();
            //services.AddTransient<LoginPageViewModel>();
            //services.AddTransient<RegisterPageViewModel>();
            //services.A    ddTransient<MaterialDetailPageViewModel>();
            //services.AddTransient<ShopPageViewModel>();
            //services.AddTransient<QRCodePageViewModel>();
            //services.AddTransient<ShopDetailPageViewModel>();
            //services.AddTransient<HistoricPageViewModel>();
            //services.AddTransient<ConclusionPageViewModel>();
            //services.AddTransient<MapPageViewModel>();
            //services.AddTransient<SortingPointsPageViewModel>();
            //services.AddTransient<RecyclePageViewModel>();

            return services;
        }

        public class RawValueAttribute : Attribute
        {
            public object Value { get; set; }
            public RawValueAttribute(object value) => this.Value = value;
        }
    }
}

