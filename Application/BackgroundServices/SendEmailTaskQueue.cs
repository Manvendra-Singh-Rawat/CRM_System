using ClientManagement.Application.DTO;
using ClientManagement.Application.Interfaces;
using System.Threading.Channels;

namespace ClientManagement.Application.BackgroundServices
{
    public class SendEmailTaskQueue : ISendEmailTaskQueue
    {
        private Channel<InvoiceDataDTO> _queue = Channel.CreateUnbounded<InvoiceDataDTO>();

        public async Task<InvoiceDataDTO> DequeueAsync(CancellationToken cancellationToken) => await _queue.Reader.ReadAsync(cancellationToken);
        public async Task EnqueueAsync(InvoiceDataDTO data) => await _queue.Writer.WriteAsync(data);
    }
}
