namespace Apps.Modules.Views;

public partial class HomePage : MyContentPage
{
    public HomePage()
    {
        InitializeComponent();
        this.BindingContext = VM =
            Core.CoreHelpers.ServiceProvider.GetRequiredService<Core.Modules.ViewModels.HomePageViewModel>();


    }

    protected override void OnAppearing() => base.OnAppearing();
}