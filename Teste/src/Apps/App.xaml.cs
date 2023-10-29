using CommunityToolkit.Mvvm.Messaging;
using Core;
using Core.Modules.Models;
using System.Web;

namespace Apps
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            WeakReferenceMessenger.Default.Register<NotificationItemMessage>(this, (r, m) =>
            {
                string strLink = m.Value.ToString();
                Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                {
                    var queryDictionary = HttpUtility.ParseQueryString(strLink.Replace("?", "&"));

                    var vm = Core.CoreHelpers.ServiceProvider.GetRequiredService<Core.Modules.ViewModels.AccountPageViewModel>();

                    return false;
                });
            });
            MainPage = new AppShell();
        }


        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);

            Dispatcher.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                App.Current.MainPage.DisplayAlert("Teste", uri.ToString(), "Ok");

                var queryDictionary = HttpUtility.ParseQueryString(uri.ToString().Replace("?", "&"));
                ConfirmEmailModel cm = new()
                {
                    UserId = queryDictionary["userId"],
                    Token = queryDictionary["code"],
                };
                var vm = CoreHelpers.ServiceProvider.GetRequiredService<Core.Modules.ViewModels.AccountPageViewModel>();
                vm.ConfirmEmailCommand.Execute(cm);
                return false;
            });
            //var queryDictionary = HttpUtility.ParseQueryString(uri.ToString().Replace("?", "&"));

            //var vm = Core.CoreHelpers.ServiceProvider.GetRequiredService<Core.Modules.ViewModels.AccountPageViewModel>();



            //MainThread.BeginInvokeOnMainThread(() =>
            //{
            //    _navigationService.NavigateTo("RegisterPage", "ConfirmEmailModel", confirmEmailModel);
            //});
        }
    }
}
