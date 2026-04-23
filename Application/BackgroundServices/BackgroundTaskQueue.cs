using ClientManagement.Application.Interfaces;
using System.Threading.Channels;

namespace ClientManagement.Application.BackgroundServices
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<int> _queue = Channel.CreateUnbounded<int>();

        public async Task EnqueueAsync(int job) => await _queue.Writer.WriteAsync(job);
        public async Task<int> DequeueAsync(CancellationToken cancellationToken) => await _queue.Reader.ReadAsync(cancellationToken);
    }
}
