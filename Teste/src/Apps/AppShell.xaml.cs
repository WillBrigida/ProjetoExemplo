namespace Apps
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoute();
        }

        private void RegisterRoute()
        {
            Routing.RegisterRoute(nameof(Modules.Views.RegisterPage), typeof(Modules.Views.RegisterPage));
            Routing.RegisterRoute(nameof(Modules.Views.LoginPage), typeof(Modules.Views.LoginPage));
            Routing.RegisterRoute(nameof(Modules.Views.ConfirmationPage), typeof(Modules.Views.ConfirmationPage));
            Routing.RegisterRoute(nameof(Modules.Views.ForgotPasswordPage), typeof(Modules.Views.ForgotPasswordPage));

            //Routing.RegisterRoute(nameof(Modules.Views.HomePage), typeof(Modules.Views.HomePage));

            //Routing.RegisterRoute(nameof(Modules.Views.MaterialDetailPage), typeof(Modules.Views.MaterialDetailPage));
            //Routing.RegisterRoute(nameof(Modules.Views.ShopPage), typeof(Modules.Views.ShopPage));
            //Routing.RegisterRoute(nameof(Modules.Views.ShopDetailPage), typeof(Modules.Views.ShopDetailPage));
            //Routing.RegisterRoute(nameof(Modules.Views.AboutPage), typeof(Modules.Views.AboutPage));
            //Routing.RegisterRoute(nameof(Modules.Views.MapPage), typeof(Modules.Views.MapPage));
            //Routing.RegisterRoute(nameof(Modules.Views.QRCodePage), typeof(Modules.Views.QRCodePage));
            //Routing.RegisterRoute(nameof(Modules.Views.HistoricPage), typeof(Modules.Views.HistoricPage));
            //Routing.RegisterRoute(nameof(Modules.Views.ConclusionPage), typeof(Modules.Views.ConclusionPage));
            //Routing.RegisterRoute(nameof(Modules.Views.SortingPointsPage), typeof(Modules.Views.SortingPointsPage));
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            bool useSpecificpage = true;

            if (useSpecificpage)
            {
                Items.Add(new ShellContent
                {
                    FlyoutItemIsVisible = false,
                    Title = nameof(Modules.Views.LoginPage),
                    Route = nameof(Modules.Views.LoginPage),
                    ContentTemplate = new DataTemplate(typeof(Modules.Views.LoginPage))
                });

                Items.Add(new ShellContent
                {
                    Title = nameof(Modules.Views.HomePage),
                    Route = nameof(Modules.Views.HomePage),
                    ContentTemplate = new DataTemplate(typeof(Modules.Views.HomePage))
                });

                return;
            }
        }
    }
}