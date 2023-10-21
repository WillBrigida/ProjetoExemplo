namespace Core.Modules.Services
{
    public interface INavigationService
    {
        object GetNavigationParameter(string key);
        T GetNavigationParameter<T>(string key);
        void SetPageInNavigationStack(string viewModelName);
        Task NavigateTo(string viewModelName, Dictionary<string, object>? parameters, bool animate = true);
        Task NavigateTo(string viewModelName, string keyParameter, object? ValueParameter, bool animate = true);
        Task NavigateTo(string viewModelName, bool animate = true);
        Task NavigateToRoot();
        Task NavigateToHome();
        Task BackTo(string viewModelName);
    }
}

