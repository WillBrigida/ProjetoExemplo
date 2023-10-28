using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Modules.Base;
using Core.Modules.Models;
using Core.Modules.Services;
using System.Text.Json;

namespace Core.Modules.ViewModels
{
    public enum eAccountEditor { None, RegisterNewUser, ForgotPassword, ChangePassword, ChangeEmail }

    public partial class AccountPageViewModel : BaseViewModel
    {
        const string ACCOUNT_ROUTE = "api/v1/Account";

        private readonly INavigationService? _navigationService;
        private readonly IApiService? _apiService;
        private readonly IAlertService? _alertService;
        private readonly ILocalStorageService? _localStorageService;
        private readonly ILauncherService? _launcherService;

        [ObservableProperty]
        private UserDTO? _principalUser = new();

        [ObservableProperty]
        private RegisterInputModel? _registerInputModel = new();

        [ObservableProperty]
        private LoginInputModel? _loginInputModel = new();

        [ObservableProperty]
        private NewEmailInputModel? _newEmailInputModel = new();

        [ObservableProperty]
        private ChangePasswordInputModel? _changePasswordInputModel = new();

        [ObservableProperty]
        private bool _registerNewPassword;

        [ObservableProperty]
        private eAccountEditor _accountEditor;

        [ObservableProperty]
        private string? _htmlMessage;

        public AccountPageViewModel() { }

        public AccountPageViewModel(IApiService apiService,
                                    IAlertService? alertService,
                                    INavigationService? navigationService,
                                    ILocalStorageService? localStorageService,
                                    ILauncherService launcherService)
        {
            _apiService = apiService;
            _alertService = alertService;
            _navigationService = navigationService;
            _localStorageService = localStorageService;
            _launcherService = launcherService;
        }

        public override Task OnAppearingAsync()
        {
            AccountEditor = _navigationService!.GetNavigationParameter<eAccountEditor>("AccountEditor");

            if (AccountEditor is not eAccountEditor.None)
            {
                PrincipalUser = CoreHelpers.PrincipalUser;
                HtmlMessage = _navigationService!.GetNavigationParameter<string>("HtmlMessage");
            }

            return base.OnAppearingAsync();
        }

        [RelayCommand]
        async Task OnRegisterConfirmation(string url)
        {
            await _launcherService!.OpenAsync(url);
            await _navigationService!.NavigateTo("LoginPage");

            //ConfirmEmailModel confirmEmailModel = _navigationService!.GetNavigationParameter<ConfirmEmailModel>("ConfirmEmailModel");
            //try
            //{
            //    IsBusy = true;
            //    var response = await _apiService!.PostAsync<GenericResponse>($"{ACCOUNT_ROUTE}/registerconfirmation", confirmEmailModel);
            //    if (!response.Successful)
            //    {
            //        await _alertService!.ShowAlert("Error!", $"Descrição: {response.Message}", "Ok");
            //        return;
            //    }

            //    await _alertService!.ShowAlert("", $"{response.Message}", "Ok");
            //    await _navigationService!.NavigateTo("LoginPage");
            //}

            //finally { IsBusy = false; }
        }

        [RelayCommand]
        async Task OnRegister()
        {
            try
            {
                IsBusy = true;
                var response = await _apiService!.PostAsync<GenericResponse<string>>($"{ACCOUNT_ROUTE}/register", RegisterInputModel!);
                if (!response.Successful)
                {
                    await _alertService!.ShowAlert("Error!", $"Descrição: {response.Message}\n\n{response.Error}", "Ok");
                    return;
                }

                await _alertService!.ShowAlert("", $"{response.Message}", "Ok");

                Dictionary<string, object> parameters = new()
                {
                    { "HtmlMessage", response.Data! },
                    { "AccountEditor", eAccountEditor.RegisterNewUser }
                };
                await _navigationService!.NavigateTo("ConfirmationPage", parameters);
            }

            finally { IsBusy = false; }
        }

        [RelayCommand]
        async Task OnLogin()
        {
            try
            {
                IsBusy = true;
                var response = await _apiService!.PostAsync<GenericResponse<UserDTO>>($"{ACCOUNT_ROUTE}/login", LoginInputModel!);
                if (!response.Successful)
                {
                    await _alertService!.ShowAlert("Error!", $"Descrição: {response.Message}\n\n{response.Error}", "Ok");
                    return;
                }

                var json = JsonSerializer.Serialize(response.Data);

                string value = (string)_localStorageService!.Get("PrincipalUser", "");
                _localStorageService!.Remove("PrincipalUser");
                _localStorageService!.Set("PrincipalUser", json);

                await _alertService!.ShowAlert("", $"{response.Message}", "Ok");
                await _navigationService!.NavigateTo(nameof(HomePageViewModel));
            }

            finally { IsBusy = false; }
        }

        [RelayCommand]
        async Task OnForgotPassword()
        {
            try
            {
                IsBusy = true;
                var response = await _apiService!.PostAsync<GenericResponse<string>>($"{ACCOUNT_ROUTE}/forgot-password", RegisterInputModel!.Email);
                if (!response.Successful)
                {
                    await _alertService!.ShowAlert("Error!", $"Descrição: {response.Message}\n\n{response.Error}", "Ok");
                    return;
                }

                await _alertService!.ShowAlert("", $"{response.Message}", "Ok");

                Dictionary<string, object> parameters = new()
                {
                    { "HtmlMessage", response.Data! },
                    { "AccountEditor", eAccountEditor.ForgotPassword }
                };

                await _navigationService!.NavigateTo("ConfirmationPage", parameters);
            }

            finally { IsBusy = false; }
        }

        [RelayCommand]
        async Task OnChangeEmail()
        {
            var userId = CoreHelpers.PrincipalUser!.UserID;
            try
            {
                IsBusy = true;
                var response = await _apiService!.PostAsync<GenericResponse<string>>($"{ACCOUNT_ROUTE}/change-email{userId}", NewEmailInputModel!.NewEmail!);
                if (!response.Successful)
                {
                    await _alertService!.ShowAlert("Error!", $"Descrição: {response.Message}\nError: {response.Error}", "Ok");
                    return;
                }

                await _alertService!.ShowAlert("", $"{response.Message}", "Ok");

                Dictionary<string, object> parameters = new()
                {
                    { "HtmlMessage", response.Data! },
                    { "AccountEditor", eAccountEditor.ChangeEmail }
                };
                await _navigationService!.NavigateTo("ConfirmationPage", parameters);
            }

            finally { IsBusy = false; }
        }

        [RelayCommand]
        async Task OnChangePassword()
        {
            var userId = CoreHelpers.PrincipalUser!.UserID;
            try
            {
                IsBusy = true;
                var response = await _apiService!.PostAsync<GenericResponse<string>>($"{ACCOUNT_ROUTE}/change-password{userId}", ChangePasswordInputModel!);
                if (!response.Successful)
                {
                    await _alertService!.ShowAlert("Error!", $"Descrição: {response.Message}\nError: {response.Error}", "Ok");
                    return;
                }

                await _alertService!.ShowAlert("", $"{response.Message}", "Ok");
                await _navigationService!.NavigateTo("LoginPage");
            }

            finally { IsBusy = false; }
        }

        [RelayCommand]
        async Task OnAccountManager()
        {
            switch (AccountEditor)
            {
                case eAccountEditor.None: break;
                case eAccountEditor.RegisterNewUser: break;
                case eAccountEditor.ForgotPassword: await OnForgotPassword(); break;
                case eAccountEditor.ChangeEmail: await OnChangeEmail(); break;
                case eAccountEditor.ChangePassword: await OnChangePassword(); break;
                default: break;
            }
        }

        [RelayCommand]
        async Task OnNavToRegisterPage()
        {
            await _navigationService!.NavigateTo("RegisterPage");
        }

        [RelayCommand]
        async Task OnNavToForgotPasswordPage()
        {
            await _navigationService!.NavigateTo("AccountManagerPage", "AccountEditor", eAccountEditor.ForgotPassword);
        }
    }
}