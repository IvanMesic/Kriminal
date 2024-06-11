using DAL.Model.DTO;

namespace WebShop.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginDTO login);
        Task LogoutAsync();
        Task<bool> RegisterAsync(RegisterDTO model);
        bool UserExists(string username);
        bool CanCreate(RegisterDTO model);
        Task<string?> GetAuthenticatedUserIdAsync();
        bool IsAuthenticated();
    }
}
