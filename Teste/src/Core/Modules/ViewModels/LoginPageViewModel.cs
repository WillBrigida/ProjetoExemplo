using CommunityToolkit.Mvvm.ComponentModel;
using Core.Modules.Base;
using Core.Modules.Models;
using Core.Modules.Services;

namespace Core.Modules.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        const string ROTA_ACESSO = "api/v1/Account";

        private readonly INavigationService? _navigationService;
        private readonly IApiService? _apiService;
        private readonly IAlertService? _alertService;
        private readonly IAuthService? _authService;
        private readonly ILocalStorageService? _localStorageService;

        [ObservableProperty]
        private LoginInputModel? _loginInputModel = new();

        public LoginPageViewModel() { }

        public LoginPageViewModel(IApiService apiService,
                                  IAlertService? alertService,
                                  INavigationService? navigationService,
                                  ILocalStorageService? localStorageService)
        {
            _apiService = apiService;
            _alertService = alertService;
            _navigationService = navigationService;
            _localStorageService = localStorageService;
        }

    }

    public class LoginModel : ObservableObject
    {
        private string? _login;
        public string? Login
        {
            get => _login;
            set
            {
                if (SetProperty(ref _login, value, "Login"))
                    IsValid = !string.IsNullOrEmpty(_senha) && !string.IsNullOrEmpty(_login);
            }
        }

        private string? _senha;
        public string? Senha
        {
            get => _senha;
            set
            {
                if (SetProperty(ref _senha, value, "Senha"))
                    IsValid = !string.IsNullOrEmpty(_senha) && !string.IsNullOrEmpty(_login);
            }
        }

        private bool _isValid = false;
        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        private bool IsCompleted()
        {
            return (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Senha));
        }

        private bool IsValidated()
        {
            return (string.IsNullOrEmpty(Login) && string.IsNullOrEmpty(Senha)) ||
                (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Senha) &&
                Senha.Length > 5);
        }
    }
}