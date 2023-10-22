namespace Apps.Modules.Views;

public partial class LoginPage : MyContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        this.BindingContext = VM =
                Core.CoreHelpers.ServiceProvider.GetRequiredService<Core.Modules.ViewModels.LoginPageViewModel>();
    }
}