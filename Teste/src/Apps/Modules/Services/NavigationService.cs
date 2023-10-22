using Core;
using Core.Modules.Services;

namespace Apps.Modules.Services
{
    public class NavigationService : INavigationService
    {
        private static Dictionary<string, object>? NaigationParameters { get; set; } = new();

        private static Dictionary<string, object>? NaigationStack { get; set; } = new();

        public async Task NavigateTo(string viewModelName, bool animate)
        {
            ClearNavigationParameter();
            await Nav(viewModelName, animate);
        }

        public async Task NavigateTo(string viewModelName, string key, object parameter, bool animate)
        {
            ClearNavigationParameter();
            SetNavigationParamiter(key, parameter);
            await Nav(viewModelName, animate);
        }

        public async Task NavigateTo(string viewModelName, Dictionary<string, object>? parameters, bool animate)
        {
            ClearNavigationParameter();
            foreach (var item in parameters)
                SetNavigationParamiter(item.Key, item.Value);

            await Nav(viewModelName, animate);
        }

        //public static Dictionary<string, Popup> PopupPages { get; set; } = new()
        //{
        //    { "HistoricPopupPage", new Apps.Modules.Historic.HistoricPopupPage()},
        //    { "IncludeMovimentPopupPage", new Apps.Modules.IncludeMoviment.IncludeMovimentPopupPage()},
        //};

        private async Task Nav(string viewModelName, bool animate)
        {
            bool initPage = viewModelName.Contains("Home") || viewModelName.Contains("Main") || viewModelName.Contains("Menu")
            || viewModelName.Contains("Root");
            bool currentPage = App.Current.MainPage.ToString().Contains("Login") || App.Current.MainPage.ToString().Contains("AppShell");

            if (viewModelName.Contains("Login"))
            {
                await Shell.Current.GoToAsync($"//{viewModelName.GetPageName() + "Page"}");
                return;
            }

            if (initPage && currentPage)
            {
                //#if ANDROID
                //                if (Platform.CurrentActivity.CurrentFocus != null)
                //                    Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
                //#endif
                await Shell.Current.GoToAsync($"//{viewModelName.GetPageName() + "Page"}");
                return;
            };


            await Shell.Current.GoToAsync(viewModelName.EndsWith("Page") || viewModelName.EndsWith("..")
                        ? viewModelName
                        : viewModelName.GetPageName() + "Page", animate);
        }

        public async Task NavigateToRoot()
           => await Shell.Current.Navigation.PopToRootAsync();

        public async Task NavigateToHome() => App.Current.MainPage = new AppShell();

        public async Task BackTo(string viewModelName)
        {
            if (viewModelName.EndsWith("."))
                await Shell.Current.GoToAsync(viewModelName);
            else
                await Shell.Current.GoToAsync(viewModelName.GetPageName() + "Page");
        }

        public void SetPageInNavigationStack(string? viewModelName)
        {
            var dictionary = new Dictionary<string, object>();

            if (viewModelName is null) return;

            if ((NaigationStack?.Count ?? 0) > 0 && (NaigationStack ?? new()).ContainsKey(viewModelName))
                (NaigationStack ?? new()).Remove(viewModelName ?? "");

            foreach (var item in NaigationStack ?? throw new ArgumentException(nameof(NaigationStack)))
                dictionary.Add(item.Key, item.Value);

            dictionary?.TryAdd(viewModelName, viewModelName);
            NaigationStack.Clear();
            NaigationStack = dictionary;
        }

        private void SetNavigationParamiter(object value)
        {
            if (value is null) return;
            SetNavigationParamiter(value.GetType().Name, value);
        }

        private void SetNavigationParamiter(string? key, object value)
        {
            if (value is null) return;
            if (string.IsNullOrEmpty(key)) key = value.GetType().Name;
            if ((NaigationParameters?.Count ?? 0) > 0 && (NaigationParameters ?? new()).ContainsKey(key))
                (NaigationParameters ?? new()).Remove(key);

            NaigationParameters?.TryAdd(key, value);
        }

        public T? GetNavigationParameter<T>(string? key = "")
        {
            if (string.IsNullOrEmpty(key)) key = typeof(T).Name;

            (NaigationParameters ?? new()).TryGetValue(key, out object? value);

            return (value is not null) ? (T)value : default(T);
        }

        public object? GetNavigationParameter(string key)
        {
            (NaigationParameters ?? new()).TryGetValue(key, out object? value);

            return (value is not null) ? value : default;
        }

        private void ClearNavigationParameter() => (NaigationParameters ?? new()).Clear();

        private void RemovePreviousPage()
        {
            var stack = Shell.Current.Navigation.NavigationStack.ToArray();
            if (stack.Length > 2)
                Shell.Current.Navigation.RemovePage(stack[stack.Length - 2]);
        }
    }
}


