using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Modules.Base;
using Core.Modules.Models;
using Core.Modules.Services;

namespace Core.Modules.ViewModels
{
    public partial class HomePageViewModel : BaseViewModel
    {
        const string ACCOUNT_ROUTE = "api/v1/Account";

        private readonly INavigationService? _navigationService;
        private readonly IApiService? _apiService;
        private readonly IAlertService? _alertService;

        [ObservableProperty]
        private LoginInputModel? _inputMode = new();

        [ObservableProperty]
        private UserDTO? _principalUser = new();

        public HomePageViewModel() { }

        public HomePageViewModel(IApiService apiService,
                                 IAlertService? alertService,
                                 INavigationService navigationService)
        {
            _apiService = apiService;
            _alertService = alertService;
            _navigationService = navigationService;
        }

        public override Task OnAppearingAsync()
        {
            PrincipalUser = CoreHelpers.PrincipalUser;
            return base.OnAppearingAsync();
        }

        [RelayCommand]
        public async Task OnLogout()
        {
            await _navigationService!.NavigateTo("LoginPage");
            CoreHelpers.ClearPrincipalUser();
        }

        [RelayCommand]
        async Task OnNavToAccoutPage()
            => await _navigationService!.NavigateTo("AccountPage", "AccountEditor", eAccountEditor.ChangePersonalData);
    }
}