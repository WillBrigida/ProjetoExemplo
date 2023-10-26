namespace Apps.Modules.Views;

public partial class ConfirmationPage : MyContentPage
{
    public ConfirmationPage()
    {
        InitializeComponent();
        this.BindingContext = VM =
            Core.CoreHelpers.ServiceProvider.GetRequiredService<Core.Modules.ViewModels.AccountPageViewModel>();
    }

    protected override void OnAppearing() => base.OnAppearing();

}