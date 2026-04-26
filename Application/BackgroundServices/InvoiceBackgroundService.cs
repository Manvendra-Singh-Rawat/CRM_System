using ClientManagement.Application.DTO;
using ClientManagement.Application.Interfaces;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Application.BackgroundServices
{
    public class InvoiceBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IGenerateInvoiceTaskQueue _bgQueue;
        private readonly ISendEmailTaskQueue _emailQueue;

        public InvoiceBackgroundService(IServiceScopeFactory scopeFactory, IGenerateInvoiceTaskQueue bgQueue, ISendEmailTaskQueue _emailQueue)
        {
            _scopeFactory = scopeFactory;
            _bgQueue = bgQueue;
            this._emailQueue = _emailQueue;
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
                        WorkId = w.Id,
                        ClientName = w.User.UserDetail.Name,
                        ClientPhone = w.User.UserDetail.Phone,
                        ClientEmail = w.User.Email,
                        ProjectName = w.ProjectName,
                        ProjectCost = w.Cost,
                    })
                    .FirstOrDefaultAsync(stoppingToken);

                    byte[] invoiceBytes = await invoiceServices.CreateInvoice(workData);
                    await _emailQueue.EnqueueAsync(new InvoiceDataDTO
                    {
                        InvoiceBytes = invoiceBytes,
                        ProjectName = workData.ProjectName,
                        Email = workData.ClientEmail
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

        }
    }
}
