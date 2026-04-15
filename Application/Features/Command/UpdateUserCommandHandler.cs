using ClientManagement.Application.DataTemplate;
using ClientManagement.Application.Interfaces;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Application.Features.Command
{
    public class UpdateUserCommandHandler(ClientManagementDbContext dbContext, ICurrentUserService currentUserService) : IRequestHandler<UpdateUserCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            bool status = await dbContext.Works
                .Where(id => id.ClientId == currentUserService.clientId)
                .AnyAsync(paid => paid.IsPaid == false, cancellationToken);
            if (status == true)
                return Result<string>.Failure("Complete all payments before updating details.", 500);

            var userDetails = await dbContext.UserDetails
                .Where(x => x.Id == currentUserService.clientId)
                .FirstOrDefaultAsync(cancellationToken);
            if (userDetails == null)
                return Result<string>.Failure("Internal Server Error", 500);

            userDetails.Name = userDetails.Name ?? request.Name;
            userDetails.Phone = request.Phone ?? userDetails.Phone;
            userDetails.Company = request.Company ?? userDetails.Company;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result<string>.Success("Details updated successfully", "Details updated successfully", 200);
        }
    }
}
