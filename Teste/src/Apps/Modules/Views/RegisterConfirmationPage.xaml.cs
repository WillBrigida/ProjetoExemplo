namespace Apps.Modules.Views;

public partial class RegisterConfirmationPage : MyContentPage
{
    public RegisterConfirmationPage()
    {
        InitializeComponent();
        this.BindingContext = VM =
            Core.CoreHelpers.ServiceProvider.GetRequiredService<Core.Modules.ViewModels.RegisterPageViewModel>();
    }
}