using ClientManagement.Application.DataTemplate;
using ClientManagement.Domain.Entity;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Application.Features.Command
{
    public class UpdateWorkDetailsCommandHandler(ClientManagementDbContext dbContext) : IRequestHandler<UpdateWorkDetailsCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateWorkDetailsCommand request, CancellationToken cancellationToken)
        {
            Work? updateWork = await dbContext.Works
                .Where(sequenceId => sequenceId.Id == request.workId)
                .FirstOrDefaultAsync(cancellationToken);
            if (updateWork == null)
                return Result<string>.Failure("Work not found", 204);

            string returnMessage = "";
            if(request.Cost > 0)
            {
                updateWork.Cost = request.Cost;
                returnMessage = $"cost updated for the work: {updateWork.Id} to {updateWork.Cost}";
            }
            else if(request.IsCompleted)
            {
                updateWork.IsCompleted = true;
                returnMessage = $"work: {updateWork.Id} has been completed";
            }
            else if(request.IsPaid)
            {
                updateWork.IsPaid = true;
                returnMessage = $"payment done for the work: {updateWork.Id}";
                //task Generate invoice
            }

            await dbContext.SaveChangesAsync();
            return Result<string>.Success(returnMessage, "", 200);
        }
    }
}
