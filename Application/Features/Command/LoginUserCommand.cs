using ClientManagement.Application.DTO;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Application.Features.Command
{
    public class LoginUserCommand : IRequest<UserLoginDTO>
    {
        [Required]
        [MinLength(10)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}
