namespace ClientManagement.Application.Interfaces
{
    public interface IGenerateInvoiceTaskQueue
    {
        Task EnqueueAsync(int workId);
        Task <int> DequeueAsync (CancellationToken cancellationToken);
    }
}
