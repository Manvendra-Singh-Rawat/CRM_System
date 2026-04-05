using MediatR;

namespace ClientManagement.Application.Features.Command
{
    public class UpdateUserCommand : IRequest<string>
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }
}
