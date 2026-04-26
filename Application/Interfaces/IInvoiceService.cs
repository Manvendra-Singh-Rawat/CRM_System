using ClientManagement.Application.DTO;

namespace ClientManagement.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<byte[]> CreateInvoice(CreateInvoiceDTO invoiceData);
    }
}
