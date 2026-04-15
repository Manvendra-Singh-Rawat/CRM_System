using ClientManagement.Application.DTO;
using ClientManagement.Application.Interfaces;
using ClientManagement.Domain.Entity;

namespace ClientManagement.Application.Service
{
    public class AuthService : IAuthService
    {
        public Task<string?> LoginAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> RegisterAsync(UserRegisterDTO client)
        {
            throw new NotImplementedException();
        }
    }
}
