using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Modules.Base;
using Core.Modules.Models;
using Core.Modules.Services;

namespace Core.Modules.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        const string ROTA_ACESSO = "api/v1/Access";

        private readonly INavigationService? _navigationService;
        private readonly IApiService? _apiService;
        private readonly IAlertService? _alertService;
        private readonly IAuthService? _authService;

        [ObservableProperty]
        private LoginInputModel? _loginInputModel = new();

        public LoginPageViewModel() { }

        public LoginPageViewModel(IApiService apiService, IAlertService? alertService, INavigationService navigationService)
        {
            _apiService = apiService;
            _alertService = alertService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        async Task OnLogin()
        {
            try
            {
                IsBusy = true;
                var response = await _apiService!.PostAsync<GenericResponse>($"{ROTA_ACESSO}/login", LoginInputModel!);
                if (!response.Successful)
                {
                    //Logica de erro
                    await _alertService!.ShowAlert("Error!", $"Descrição: {response.Message}\n\n{response.Error}", "Ok");
                    return;
                }

                await _alertService!.ShowAlert("", $"{response.Message}", "Ok");
                await _navigationService!.NavigateTo(nameof(HomePageViewModel));

            }

            finally { IsBusy = false; }
        }


        [RelayCommand]
        async Task OnNavToRegisterPage()
        {
            await _navigationService!.NavigateTo(nameof(RegisterPageViewModel));
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