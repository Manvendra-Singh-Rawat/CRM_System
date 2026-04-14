using ClientManagement.Application.Interfaces;
using ClientManagement.Domain.Entity;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Application.Features.Command
{
    public class CreateWorkCommandHandler(ClientManagementDbContext dbContext, ICurrentUserService currentUserService) : IRequestHandler<CreateWorkCommand, string>
    {
        public async Task<string> Handle(CreateWorkCommand request, CancellationToken cancellationToken)
        {
            // to do code
            bool isExist = await dbContext.Users.AnyAsync(x => x.Id == currentUserService.clientId);
            if (!isExist) return "client doesn't exist";

            UserDetail? userDetail = await dbContext.UserDetails.Where(x => x.Id == currentUserService.clientId).FirstOrDefaultAsync(cancellationToken);
            if (userDetail == null) return "internal server error";
            if (userDetail.Name == "" || userDetail.Phone == "" || userDetail.Company == "")
                return "Please update your details first.";

            Work newWork = new Work
            {
                ClientId = currentUserService.clientId,
                ProjectName = request.projName,
                ProjectDesc = request.description,
                Cost = 0
            };

            await dbContext.Works.AddAsync(newWork);
            int result = await dbContext.SaveChangesAsync();

            return (result < 1) ? "work not added" : "work added";
        }
    }
}
