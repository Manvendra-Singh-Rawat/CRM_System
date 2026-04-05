using MediatR;

namespace ClientManagement.Application.Features.Command
{
    public class RegisterUserCommand : IRequest<int>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
