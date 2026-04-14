using ClientManagement.Application.DTO;
using ClientManagement.Application.Interfaces;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Application.Features.Query
{
    public class GetWorkQueryHandler(ClientManagementDbContext dbContext, ICurrentUserService currentUserService)
        : IRequestHandler<GetWorkQuery, List<GetWorkDTO>>
    {
        public async Task<List<GetWorkDTO>> Handle(GetWorkQuery request, CancellationToken cancellationToken)
        {
            var retrievedWork = await dbContext.Works
            .Where(w =>
                dbContext.Users
                    .Where(u => u.Id == currentUserService.clientId)
                    .Select(u => u.Role)
                    .FirstOrDefault() == "Admin"
                || w.ClientId == currentUserService.clientId
            )
            .Select(w => new GetWorkDTO
            {
                ProjectName = w.ProjectName,
                ProjectDesc = w.ProjectDesc,
                Cost = w.Cost,
                ClientId = w.ClientId
            })
            .ToListAsync(cancellationToken);

            return retrievedWork;
        }
    }
}
