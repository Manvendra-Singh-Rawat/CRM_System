using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Application.Features.Command
{
    public class UpdateUserCommand : IRequest<string>
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MinLength(7)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [MinLength(5)]
        public string Company { get; set; } = string.Empty;
    }
}
