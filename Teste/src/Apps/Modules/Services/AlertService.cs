using Core.Modules.Services;

namespace Apps.Modules.Services
{
    public class AlertService : IAlertService
    {
        public async Task ShowAlert(string title, string message, string cancel = "Cancelar")
        {
            await Shell.Current.DisplayAlert(title, message, cancel);
        }

        public async Task ShowAlert(string title, string message, string accept = "Sim", string cancel = "Não", Action action = null)
        {
            await Shell.Current.DisplayAlert(title, message, accept, cancel);
        }

        public async Task ShowPrompt(string title, string message, string accept = "Sim", string cancel = "Não", string placeholder = "Informe um valor", object keyboard = null, Action<object> action = null)
        {
            await Shell.Current.DisplayPromptAsync(title, message, accept, cancel, placeholder);
        }
    }
}

