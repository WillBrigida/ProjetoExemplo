using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Modules.Base;
using Core.Modules.Models;
using Core.Modules.Services;
using System.Text.Json;

namespace Core.Modules.ViewModels
{
    public partial class HomePageViewModel : BaseViewModel
    {
        const string ROTA_ACESSO = "api/v1/Access";

        private readonly INavigationService? _navigationService;
        private readonly IApiService? _apiService;
        private readonly IAlertService? _alertService;
        private readonly IAuthService? _authService;
        private readonly ILocalStorageService? _localStorageService;

        [ObservableProperty]
        private LoginInputModel? _inputMode = new();

        [ObservableProperty]
        private UserDTO? _principalUser = new();

        public HomePageViewModel() { }

        public HomePageViewModel(IApiService apiService, IAlertService? alertService, INavigationService navigationService, ILocalStorageService? localStorageService)
        {
            _apiService = apiService;
            _alertService = alertService;
            _navigationService = navigationService;
            _localStorageService = localStorageService;
        }

        public override Task OnAppearingAsync()
        {
            var json = _localStorageService!.Get("PrincipalUser", "");

            PrincipalUser = JsonSerializer.Deserialize<UserDTO>(json.ToString());
            return base.OnAppearingAsync();
        }

        [RelayCommand]
        public async Task OnLogout()
        {
            try
            {
                IsBusy = true;
                var response = await _apiService!.PostAsync<GenericResponse>($"{ROTA_ACESSO}/logout", null);
                if (!response.Successful)
                {
                    //Logica de erro
                    await _alertService!.ShowAlert("Error!", $"Descrição: {response.Message}\n\n{response.Error}", "Ok");
                    return;
                }

                _localStorageService!.Remove("PrincipalUser");
                await _alertService!.ShowAlert("", $"{response.Message}", "Ok");
                await _navigationService!.NavigateTo(nameof(LoginPageViewModel));
            }

            finally { IsBusy = false; }

            //await _authService!.AuthLogout();
            //_apiService!.CleanDefaultRequestHeaders();
            //await _navigationService!.NavigateTo("LoginPage");
        }
    }
}