namespace Core.Modules.Services
{
    public interface IAuthService
    {
        //Task AuthLogin(LoginResponse<UsuarioDTO> Login);
        Task AuthLogout();
        Task<string> GetToken();
    }
}
