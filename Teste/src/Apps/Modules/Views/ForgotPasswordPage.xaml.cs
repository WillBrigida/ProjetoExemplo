namespace Apps.Modules.Views;

public partial class ForgotPasswordPage : MyContentPage
{
    public ForgotPasswordPage()
    {
        InitializeComponent();
        this.BindingContext = VM =
            Core.CoreHelpers.ServiceProvider.GetRequiredService<Core.Modules.ViewModels.RegisterPageViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}