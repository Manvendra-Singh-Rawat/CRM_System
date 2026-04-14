using ClientManagement.Application.Interfaces;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Application.Features.Command
{
    public class UpdateUserCommandHandler(ClientManagementDbContext dbContext, ICurrentUserService currentUserService) : IRequestHandler<UpdateUserCommand, string>
    {
        public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            bool status = await dbContext.Works
                .Where(id => id.ClientId == currentUserService.clientId)
                .AnyAsync(paid => paid.IsPaid == false, cancellationToken);
            if (status == true) return "Please complete all the payments before proceeding.";

            var userDetails = await dbContext.UserDetails
                .Where(x => x.Id == currentUserService.clientId)
                .FirstOrDefaultAsync(cancellationToken);
            if (userDetails == null) return "some error occured";

            userDetails.Name = userDetails.Name ?? request.Name;
            userDetails.Phone = request.Phone ?? userDetails.Phone;
            userDetails.Company = request.Company ?? userDetails.Company;

            int linesChanged = await dbContext.SaveChangesAsync(cancellationToken);

            return linesChanged > 0 ? "changes applied" : "no changes applied";
        }
    }
}
