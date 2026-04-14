using ClientManagement.Application.DTO;
using MediatR;

namespace ClientManagement.Application.Features.Query
{
    public class GetWorkQuery : IRequest<List<GetWorkDTO>>
    {
    }
}
