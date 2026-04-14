using ClientManagement.Domain.Entity;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Application.Features.Command
{
    public class UpdateWorkDetailsCommandHandler(ClientManagementDbContext dbContext) : IRequestHandler<UpdateWorkDetailsCommand, string>
    {
        public async Task<string> Handle(UpdateWorkDetailsCommand request, CancellationToken cancellationToken)
        {
            Work? updateWork = await dbContext.Works
                .Where(sequenceId => sequenceId.Id == request.workId)
                .FirstOrDefaultAsync(cancellationToken);
            if (updateWork == null)
                return "the work you are trying to update is not available";

            string returnMessage = "";
            if(request.Cost > 0)
            {
                updateWork.Cost = request.Cost;
                returnMessage = $"cost has been updated for the work: {updateWork.Id}";
            }
            else if(request.IsCompleted)
            {
                updateWork.IsCompleted = true;
                returnMessage = $"work: {updateWork.Id} has been completed by the developers & testers";
            }
            else if(request.IsPaid)
            {
                updateWork.IsPaid = true;
                returnMessage = $"payment done for the work: {updateWork.Id}";
                //task Generate invoice
            }

            int rowsAffected = await dbContext.SaveChangesAsync();
            return rowsAffected < 1 ? "not changes were made" : returnMessage;
        }
    }
}
