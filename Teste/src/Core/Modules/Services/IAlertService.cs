namespace Core.Modules.Services
{
	public interface IAlertService
	{
        Task ShowAlert(string title, string message, string cancel = "Cancelar");
        Task ShowAlert(string title, string messag, string accept = "Sim", string cancel = "Não", Action? action = null);
        Task ShowPrompt(string title, string message, string accept = "Sim", string cancel = "Não", string placeholder = "Informe um valor", object? keyboard = null, Action<object>? action = null);
    }
}

