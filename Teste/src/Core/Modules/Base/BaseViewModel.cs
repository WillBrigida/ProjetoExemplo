using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Core.Modules.Base
{
    public class BaseViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        public static IServiceProvider? ServiceProvider { get; set; }

        private bool isBusy; public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (SetProperty(ref isBusy, value, "IsBusy"))
                    IsNotBusy = !isBusy;
            }
        }

        private bool isNotBusy = true; public bool IsNotBusy
        {
            get => isNotBusy;
            set
            {
                if (SetProperty(ref isNotBusy, value, "IsNotBusy"))
                    IsBusy = !isNotBusy;
            }
        }

        private bool _showResult; public bool ShowResult
        {
            get { return _showResult; }
            set { SetProperty(ref _showResult, value); }
        }

        private int _statusCode; public int StatusCode
        {
            get => _statusCode;
            set
            {
                if (SetProperty(ref _statusCode, value, "StatusCode"))
                    ShowResult = _statusCode < 0 || _statusCode > 299;
            }
        }

        public Action<Task>? TryAgain { get; set; }

        public ICommand HiddeResultCommand => new RelayCommand(() =>
        {
            ShowResult = false;
            StatusCode = 0;
        });

        public ICommand TryAgainCommand => new RelayCommand(async () =>
        {
            HiddeResultCommand.Execute(null);
            TryAgain?.Invoke(null);
        });

        public virtual Task OnAppearingAsync() => Task.CompletedTask;

        public virtual Task OnDisappearingAsync() => Task.CompletedTask;

        public virtual bool OnBackButtonPressed()
        {
            if (ShowResult)
            {
                ShowResult = false;
                return true;
            }

            return false;
        }
    }
}

