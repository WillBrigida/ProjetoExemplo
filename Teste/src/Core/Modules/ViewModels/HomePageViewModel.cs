using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Modules.Base;
using Core.Modules.Models;
using Core.Modules.Services;

namespace Core.Modules.ViewModels
{
    public partial class HomePageViewModel : BaseViewModel
    {
        const string ROTA_ACESSO = "api/v1/Access";

        private readonly INavigationService? _navigationService;
        private readonly IApiService? _apiService;
        private readonly IAlertService? _alertService;
        private readonly IAuthService? _authService;

        [ObservableProperty]
        private LoginInputModel? _inputMode = new();

        public HomePageViewModel() { }

        public HomePageViewModel(IApiService apiService, IAlertService? alertService, INavigationService navigationService)
        {
            _apiService = apiService;
            _alertService = alertService;
            _navigationService = navigationService;
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