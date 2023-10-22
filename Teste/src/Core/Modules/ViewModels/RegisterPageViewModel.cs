using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Modules.Base;
using Core.Modules.Models;
using Core.Modules.Services;

namespace Core.Modules.ViewModels
{
    public partial class RegisterPageViewModel : BaseViewModel
    {
        const string ROTA_ACESSO = "api/v1/Access";

        private readonly INavigationService? _navigationService;
        private readonly IApiService? _apiService;
        private readonly IAlertService? _alertService;

        [ObservableProperty]
        private RegisterInputModel? _registerInputModel = new();

        public RegisterPageViewModel() { }

        public RegisterPageViewModel(IApiService apiService, IAlertService? alertService, INavigationService navigationService)
        {
            _apiService = apiService;
            _alertService = alertService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        async Task OnRegister()
        {
            try
            {
                IsBusy = true;
                var response = await _apiService!.PostAsync<GenericResponse<ConfirmEmailModel>>($"{ROTA_ACESSO}/register", RegisterInputModel!);
                if (!response.Successful)
                {
                    await _alertService!.ShowAlert("Error!", $"Descrição: {response.Message}", "Ok");
                    return;
                }

                var confirmEmailModel = response.Data;
                confirmEmailModel!.Email = RegisterInputModel!.Email;

                await _alertService!.ShowAlert("", $"{response.Message}", "Ok");
                await _navigationService!.NavigateTo("RegisterConfirmationPage", "ConfirmEmailModel", confirmEmailModel);
            }

            finally { IsBusy = false; }
        }

        [RelayCommand]
        async Task OnRegisterConfirmation()
        {
            ConfirmEmailModel confirmEmailModel = _navigationService!.GetNavigationParameter<ConfirmEmailModel>("ConfirmEmailModel");
            try
            {
                IsBusy = true;
                var response = await _apiService!.PostAsync<GenericResponse>($"{ROTA_ACESSO}/registerconfirmation", confirmEmailModel);
                if (!response.Successful)
                {
                    await _alertService!.ShowAlert("Error!", $"Descrição: {response.Message}", "Ok");
                    return;
                }

                await _alertService!.ShowAlert("", $"{response.Message}", "Ok");
                await _navigationService!.NavigateTo(nameof(HomePageViewModel));
            }

            finally { IsBusy = false; }
        }
    }

    public class RegisterModel : ObservableObject
    {
        private string? _phone;
        public string? Phone
        {
            get => _phone;
            set
            {
                if (SetProperty(ref _phone, value, "Phone"))
                    IsValid = !string.IsNullOrEmpty(_phone) &&
                              !string.IsNullOrEmpty(_fullName) &&
                              !string.IsNullOrEmpty(_password) &&
                              !string.IsNullOrEmpty(_confirmPassword);
            }
        }

        private string? _fullName;
        public string? FullName
        {
            get => _fullName;
            set
            {
                if (SetProperty(ref _fullName, value, "Email"))
                    IsValid = !string.IsNullOrEmpty(_phone) &&
                             !string.IsNullOrEmpty(_fullName) &&
                             !string.IsNullOrEmpty(_password) &&
                             !string.IsNullOrEmpty(_confirmPassword);
            }
        }


        private string? _password;
        public string? Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value, "Password"))
                    IsValid = !string.IsNullOrEmpty(_phone) &&
                              !string.IsNullOrEmpty(_fullName) &&
                              !string.IsNullOrEmpty(_password) &&
                              !string.IsNullOrEmpty(_confirmPassword);
            }
        }

        private string? _confirmPassword;
        public string? ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (SetProperty(ref _confirmPassword, value, "ConfirmPassword"))
                    IsValid = !string.IsNullOrEmpty(_phone) &&
                             !string.IsNullOrEmpty(_fullName) &&
                             !string.IsNullOrEmpty(_password) &&
                             !string.IsNullOrEmpty(_confirmPassword);
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
            return !string.IsNullOrEmpty(_phone) &&
                    !string.IsNullOrEmpty(_fullName) &&
                    !string.IsNullOrEmpty(_password) &&
                    !string.IsNullOrEmpty(_confirmPassword);
        }

        private bool IsValidated()
        {
            return IsValid;
        }

    }
}