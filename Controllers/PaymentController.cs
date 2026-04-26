using ClientManagement.Application.DataTemplate;
using ClientManagement.Application.Features.Command;
using ClientManagement.Application.Interfaces;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Controllers
{
    [ApiController]
    [Route("[Controller]Payment")]
    public class PaymentController(ClientManagementDbContext dbContext, ISender sender, IGenerateInvoiceTaskQueue _queue) : Controller
    {
        [Authorize]
        [HttpPost("create-payment-link")]
        public async Task<ActionResult<ApiResponse<string>>> CreatePaymentLink(PaymentLinkCommand paymentRequest)
        {
            var result = await sender.Send(paymentRequest);
            if(result.IsSuccess)
                return Ok(ApiResponse<string>.SuccessResponse(result.Data, result.Message));
            else
                return ApiResponse<string>.FailureResponse(result.Message);
        }

        [Authorize]
        [HttpPost("fake-payment-success")]
        public async Task<ActionResult> FakePaymentSuccess(int workId)
        {
            var work = await dbContext.Works.FindAsync(workId);
            if (work == null)
                return NotFound();

            if (work.IsPaid)
                return Ok("Already paid");

            work.IsPaid = true;
            await dbContext.SaveChangesAsync();

            await _queue.EnqueueAsync(workId);

            return Ok("payment successful simulation");
        }

        [Authorize]
        [HttpPost("fake-payment-failed")]
        public async Task<ActionResult> FakePaymentFailed(int workId)
        {
            var work = await dbContext.Works.FindAsync(workId);
            if (work == null)
                return NotFound();

            if (work.IsPaid)
                return Ok("Already paid");

            work.IsPaid = true;
            await dbContext.SaveChangesAsync();

            return Ok("payment failed simulation");
        }
    }
}
