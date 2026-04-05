using ClientManagement.Application.DTO;
using MediatR;

namespace ClientManagement.Application.Features.Command
{
    public class LoginUserCommand : IRequest<UserLoginDTO>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
