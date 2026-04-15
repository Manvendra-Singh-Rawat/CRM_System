using ClientManagement.Application.DataTemplate;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Application.Features.Command
{
    public class UpdateWorkDetailsCommand : IRequest<Result<string>>
    {
        [Required]
        [MinLength(1)]
        public int workId { get; set; }
        public double Cost { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsPaid { get; set; }
    }
}
