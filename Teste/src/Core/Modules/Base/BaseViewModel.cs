namespace Core.Modules.Base
{
    public class BaseViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        public static IServiceProvider? ServiceProvider { get; set; }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (SetProperty(ref isBusy, value, "IsBusy"))
                    IsNotBusy = !isBusy;
            }
        }


        private bool isNotBusy = true;
        public bool IsNotBusy
        {
            get => isNotBusy;
            set
            {
                if (SetProperty(ref isNotBusy, value, "IsNotBusy"))
                    IsBusy = !isNotBusy;
            }
        }

        public virtual Task OnAppearingAsync() => Task.CompletedTask;

        public virtual Task OnDisappearingAsync() => Task.CompletedTask;
    }
}

