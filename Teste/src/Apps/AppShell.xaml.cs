using Core;
using Core.Modules.Models;

namespace Apps
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            SetEvents();
            RegisterRoute();
        }

        private void RegisterRoute()
        {
            Routing.RegisterRoute(nameof(Modules.Views.RegisterPage), typeof(Modules.Views.RegisterPage));
            Routing.RegisterRoute(nameof(Modules.Views.LoginPage), typeof(Modules.Views.LoginPage));
            Routing.RegisterRoute(nameof(Modules.Views.ConfirmationPage), typeof(Modules.Views.ConfirmationPage));
            Routing.RegisterRoute(nameof(Modules.Views.ForgotPasswordPage), typeof(Modules.Views.ForgotPasswordPage));
            Routing.RegisterRoute(nameof(Modules.Views.AccountManagerPage), typeof(Modules.Views.AccountManagerPage));
            Routing.RegisterRoute(nameof(Modules.Views.AccountPage), typeof(Modules.Views.AccountPage));

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

            BuildLayout();
        }

        private void SetEvents()
        {
            Application.Current.RequestedThemeChanged += (o, e) =>
            {
                var grid = gridFlyout.Where(x => x is Label).ToList();

                for (int i = 0; i < grid.Count; i++)
                    (grid[i] as Label).TextColor = AppsColors.TEXT_COLOR;
            };
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            bool useSpecificpage = false;

            var login = new ShellContent
            {
                FlyoutItemIsVisible = false,
                Title = nameof(Modules.Views.LoginPage),
                Route = nameof(Modules.Views.LoginPage),
                ContentTemplate = new DataTemplate(typeof(Modules.Views.LoginPage))
            };

            if (useSpecificpage)
            {
                Items.Add(new ShellContent
                {
                    Title = nameof(Modules.Views.HomePage),
                    Route = nameof(Modules.Views.HomePage),
                    ContentTemplate = new DataTemplate(typeof(Modules.Views.HomePage))
                });
                return;
            }

            UserDTO userDTO = CoreHelpers.PrincipalUser;

            if (!userDTO.RememberMe)
                Items.Add(login);

            AppsHelpers.TabItems.Select(x => new FlyoutItem
            {
                Title = x.Title,
                Route = x.Route,
                Items =
                {
                    new Tab{
                        Title = x.Title,
                        Items = {
                            new ShellContent
                            {
                                Title = x.Title,
                                Route = x.Route,
                                ContentTemplate = x.Template
                            }
                        }
                    }
                }
            }).ForEach(Items.Add);

            if (userDTO.RememberMe)
                Items.Add(login);
        }

        void CV_SelectionChanged(System.Object v, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
        {
            ShellItem previous = e.PreviousSelection.FirstOrDefault() as ShellItem;
            ShellItem current = e.CurrentSelection.FirstOrDefault() as ShellItem;

            var stack = Shell.Current.Navigation.NavigationStack.ToArray();

            if (stack.Length > 1)
                Shell.Current.Navigation.RemovePage(stack[stack.Length - 1]);
        }

        async void TapGestureRecognizer_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
        {
            int tabPosition = 0;
            Label obj = (Label)sender;

            if (obj.GestureRecognizers.Count > 0)
            {
                var gesture = (TapGestureRecognizer)obj.GestureRecognizers[0];
                int.TryParse(gesture.CommandParameter.ToString(), out tabPosition);
            }

            await Shell.Current.GoToAsync($"//{AppsHelpers.TabItems[tabPosition].Route}", animate: false);

            SelectItem(3 * tabPosition);

            Shell.Current.FlyoutIsPresented = false;
        }

        private void SelectItem(int position)
        {
            for (int i = 0; i < gridFlyout.Count; i++)
            {
                if (gridFlyout[i] is Label)
                    (gridFlyout[i] as Label).TextColor = AppsColors.TEXT_COLOR;
                if (gridFlyout[i] is BoxView)
                    (gridFlyout[i] as BoxView).Opacity = 0;
            }

            (gridFlyout[position] as BoxView).FadeTo(.10);
            (gridFlyout[position + 1] as Label).TextColor = AppsColors.ACCENT_COLOR;
            (gridFlyout[position + 2] as Label).TextColor = AppsColors.ACCENT_COLOR;
        }

        void BuildLayout()
        {
            for (int i = 0; i < AppsHelpers.TabItems.Count; i++)
            {
                gridFlyout.AddRowDefinition(new RowDefinition { Height = 50 });

                var selectView = new BoxView()
                {
                    BackgroundColor = Colors.Transparent,
                    Color = AppsColors.ACCENT_COLOR,
                    Opacity = i == 0 ? .10 : 0,
                    Margin = new Thickness(0, 0, 20, 0),
                    HeightRequest = 50,
                    CornerRadius = new CornerRadius(0, 25, 0, 25),
                };

                var icon = new Label()
                {
                    TextColor = i == 0 ? AppsColors.ACCENT_COLOR : AppsColors.TEXT_COLOR,
                    Margin = new Thickness(20, 0, 0, 0),
                    Text = AppsHelpers.TabItems[i].Icon,
                    FontSize = 25,
                    FontFamily = "Icomoon",
                    HorizontalOptions = LayoutOptions.Fill,
                    HeightRequest = 50,
                    VerticalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                var title = new Label()
                {
                    TextColor = i == 0 ? AppsColors.ACCENT_COLOR : AppsColors.TEXT_COLOR,
                    Margin = new Thickness(60, 0, 0, 0),
                    Text = AppsHelpers.TabItems[i].Title,
                    HorizontalOptions = LayoutOptions.Fill,
                    HeightRequest = 50,
                    VerticalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.CommandParameter = i;
                tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;

                title.GestureRecognizers.Add(tapGestureRecognizer);
                icon.GestureRecognizers.Add(tapGestureRecognizer);


                gridFlyout.SetColumnSpan(selectView, 2);
                gridFlyout.Add(selectView, 0, i);
                gridFlyout.Add(icon, 0, i);
                gridFlyout.Add(title, 0, i);
            }
        }
    }
}