using ClientManagement.Application.DTO;
using ClientManagement.Application.Interfaces;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Application.BackgroundServices
{
    public class InvoiceBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBackgroundTaskQueue _bgQueue;

        public InvoiceBackgroundService(IServiceScopeFactory scopeFactory, IBackgroundTaskQueue bgQueue)
        {
            _scopeFactory = scopeFactory;
            _bgQueue = bgQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workId = await _bgQueue.DequeueAsync(stoppingToken);

                using var scope = _scopeFactory.CreateScope();
                var invoiceServices = scope.ServiceProvider.GetRequiredService<IInvoiceService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<ClientManagementDbContext>();

                try
                {
                    var workData = await dbContext.Works
                    .Where(w => w.Id == workId)
                    .Select(w => new CreateInvoiceDTO
                    {
                        InvoiceId = w.Id,
                        ClientId = w.ClientId,
                        WorkId = 8,
                        ClientName = w.User.UserDetail.Name,
                        ClientPhone = w.User.UserDetail.Phone,
                        ClientEmail = w.User.Email,
                        ProjectName = w.ProjectName,
                        ProjectCost = w.Cost,
                    })
                    .FirstOrDefaultAsync(stoppingToken);

                    await invoiceServices.CreateInvoice(workData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

        }
    }
}
