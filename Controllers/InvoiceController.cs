using ClientManagement.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Controllers
{
    [ApiController]
    [Route("invoice/[controller]")]
    public class InvoiceController : Controller
    {
        [HttpPost]
        public Task CreateInvoice(Invoice invoice)
        {
            return Task.CompletedTask;
        }

        [HttpDelete]
        public Task DeleteInvoice(Invoice invoice)
        {
            return Task.CompletedTask;
        }

        [HttpPost]
        public Task UpdateInvoice(Invoice invoice)
        {
            return Task.CompletedTask;
        }
    }
}
