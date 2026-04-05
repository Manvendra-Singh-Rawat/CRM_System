using ClientManagement.Application.DTO;
using ClientManagement.Domain.Entity;

namespace ClientManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserRegisterDTO client);
        Task<string?> LoginAsync(UserLoginDTO client);
    }
}
