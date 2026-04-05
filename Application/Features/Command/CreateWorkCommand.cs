using MediatR;

namespace ClientManagement.Application.Features.Command
{
    public class CreateWorkCommand : IRequest<string>
    {
        public int clientId { get; set; }
        public string projName { get; set; } = string.Empty;
        public string description {  get; set; } = string.Empty;
    }
}
