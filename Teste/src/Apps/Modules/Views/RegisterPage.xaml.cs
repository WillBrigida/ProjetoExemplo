namespace Apps.Modules.Views;

public partial class RegisterPage : MyContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        this.BindingContext = VM =
            Core.CoreHelpers.ServiceProvider.GetRequiredService<Core.Modules.ViewModels.RegisterPageViewModel>();
    }

}