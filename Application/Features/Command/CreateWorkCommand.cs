using ClientManagement.Application.DataTemplate;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Application.Features.Command
{
    public class CreateWorkCommand : IRequest<Result<int>>
    {
        [Required]
        [MinLength(5)]
        public string projName { get; set; } = string.Empty;
        [Required]
        [MinLength(20)]
        public string description {  get; set; } = string.Empty;
    }
}
