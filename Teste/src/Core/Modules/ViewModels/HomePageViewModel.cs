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
        const string ACCOUNT_ROUTE = "api/v1/Account";

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
                CoreHelpers.ClearPrincipalUser();

                IsBusy = true;
                //var response = await _apiService!.PostAsync<GenericResponse>($"{ACCOUNT_ROUTE}/logout/{userId}", null);
                //if (!response.Successful)
                //{
                //    //Logica de erro
                //    await _alertService!.ShowAlert("Error!", $"Descrição: {response.Message}\n\n{response.Error}", "Ok");
                //    return;
                //}

                //await _alertService!.ShowAlert("", $"{response.Message}", "Ok");
                CoreHelpers.ClearPrincipalUser();
                await _navigationService!.NavigateTo(nameof(LoginPageViewModel));
            }

            finally { IsBusy = false; }
        }

        [RelayCommand]
        async Task OnNavToAccoutPage()
        {
            await _navigationService!.NavigateTo("AccountPage", "AccountEditor", eAccountEditor.ChangePersonalData);
        }
    }
}