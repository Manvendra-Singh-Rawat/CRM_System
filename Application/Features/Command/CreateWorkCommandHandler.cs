using ClientManagement.Application.DataTemplate;
using ClientManagement.Application.Interfaces;
using ClientManagement.Domain.Entity;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Application.Features.Command
{
    public class CreateWorkCommandHandler(ClientManagementDbContext dbContext, ICurrentUserService currentUser)
        : IRequestHandler<CreateWorkCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateWorkCommand request, CancellationToken cancellationToken)
        {
            bool isUserExists = await dbContext.Users.AnyAsync(x => x.Id == currentUser.clientId);
            if (!isUserExists)
                return Result<int>.Failure("Client does not exist", 204);

            UserDetail? userDetail = await dbContext.UserDetails
                .Where(x => x.Id == currentUser.clientId)
                .FirstOrDefaultAsync(cancellationToken);
            if (userDetail == null)
                return Result<int>.Failure("Internal Server Error", 500);
            if (userDetail.Name == "" || userDetail.Phone == "" || userDetail.Company == "")
                return Result<int>.Failure("Update details before creating work");

            Work newWork = new Work
            {
                ClientId = currentUser.clientId,
                ProjectName = request.projName,
                ProjectDesc = request.description,
                Cost = 0
            };

            await dbContext.Works.AddAsync(newWork);
            await dbContext.SaveChangesAsync();

            return Result<int>.Success(newWork.Id, "Work successfully created", 201);
        }
    }
}
