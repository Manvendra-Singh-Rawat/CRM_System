using ClientManagement.Application.DTO;
using ClientManagement.Application.Environment;
using ClientManagement.Application.Interfaces;
using ClientManagement.Infrastructure.Persistence.PostgresDB;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
//using System.Net.Mail;

namespace ClientManagement.Application.BackgroundServices
{
    public class SendMailBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ISendEmailTaskQueue _emailQueue;
        private readonly SMTPSettings _settings;

        public SendMailBackgroundService(IServiceScopeFactory scopeFactory, ISendEmailTaskQueue bgQueue, IOptions<SMTPSettings> smtpSettings)
        {
            _scopeFactory = scopeFactory;
            _emailQueue = bgQueue;
            _settings = smtpSettings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Inside SendMailBackgroundService");
                InvoiceDataDTO invoiceData = await _emailQueue.DequeueAsync(stoppingToken);

                string mailSubject = $"INVOICE: {invoiceData.ProjectName}";

                try
                {
                    var memStream = new MemoryStream(invoiceData.InvoiceBytes);

                    var bodyBuilder = new BodyBuilder
                    {
                        TextBody = $"Your payment for {invoiceData.ProjectName} has been successfully completed"
                    };
                    bodyBuilder.Attachments.Add($"Invoice_{invoiceData.ProjectName}.pdf", invoiceData.InvoiceBytes, new ContentType("application", "pdf"));

                    await SendMails(mailSubject, bodyBuilder, invoiceData.Email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private async Task SendMails(string mailSubject, BodyBuilder bodyBuilder, string receiverMail)
        {
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.Email, _settings.Password);

            var email = new MimeMessage();
            email.From.Add(InternetAddress.Parse(_settings.Email));
            email.To.Add(InternetAddress.Parse(receiverMail));
            email.Subject = mailSubject;
            email.Body = bodyBuilder.ToMessageBody();

            await smtp.SendAsync(email);
            Console.WriteLine($"Sent to: 'TEST USER' \nAt Mail: {receiverMail}\n");

            await smtp.DisconnectAsync(true);
            Console.WriteLine("Disconnected from SMTP Server");

            return;
        }
    }
}
