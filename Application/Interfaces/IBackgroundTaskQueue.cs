using ClientManagement.Application.DTO;

namespace ClientManagement.Application.Interfaces
{
    public interface IBackgroundTaskQueue
    {
        Task EnqueueAsync(int workId);
        Task <int> DequeueAsync (CancellationToken cancellationToken);
    }
}
