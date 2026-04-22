using ClientManagement.Application.DataTemplate;
using ClientManagement.Domain.Entity;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace ClientManagement.Application.Features.Command
{
    public class PaymentLinkCommandHandler(ClientManagementDbContext dbContext, IConfiguration config) 
        : IRequestHandler<PaymentLinkCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(PaymentLinkCommand request, CancellationToken cancellationToken)
        {
            var razorpayClient = new RazorpayClient(
                config.GetValue<string>(
                    "RazorpaySettings:TestApiKey")!, config.GetValue<string>("RazorpaySettings:TestKeySecret")!);

            // Get work details through db after fake payment success testing
            Work? work = await dbContext.Works.Where(work => work.Id == request.workId).FirstOrDefaultAsync();
            if (work == null)
                return Result<string>.Failure($"No data regarding work id: {request.workId}");

            var paymentOptions = new Dictionary<string, object>
            {
                {"amount", work.Cost * 100 },
                {"currency", "INR" },
                {"description", $"Payment for '{work.ProjectName}'" },
                {"notify", new Dictionary<string, object>
                    {
                        {"sms", false },
                        {"email", false }
                    }
                }
            };

            var paymentLink = razorpayClient.PaymentLink.Create(paymentOptions);

            // Save in db
            var linkId = paymentLink["id"].ToString();
            var shortUrl = paymentLink["short_url"].ToString();

            return Result<string>.Success(shortUrl, "Payment link created. Please use 'RAZORPAY TEST CARDS'", 200);
        }
    }
}
