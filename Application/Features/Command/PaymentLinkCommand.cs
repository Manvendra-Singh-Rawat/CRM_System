using ClientManagement.Application.DataTemplate;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Application.Features.Command
{
    public class PaymentLinkCommand : IRequest<Result<string>>
    {
        [Required]
        public int workId { get; set; }
    }
}
