using ClientManagement.Application.DTO;

namespace ClientManagement.Application.Interfaces
{
    public interface ISendEmailTaskQueue
    {
        Task EnqueueAsync(InvoiceDataDTO data);
        Task<InvoiceDataDTO> DequeueAsync(CancellationToken cancellationToken);
    }
}
