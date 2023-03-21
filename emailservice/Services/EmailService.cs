using emailservice.Dto;
using emailservice.Resources;
using emailservice.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace emailservice.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendVerificationAsync(VerifyEmailDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            using (var client = new SmtpClient(_configuration["SmtpHost"], _configuration.GetValue<int>("SmtpPort")))
            {
                client.Credentials = new NetworkCredential(_configuration["SmtpFrom"], _configuration["SmtpPwd"]);
                var from = new MailAddress(_configuration["SmtpFrom"]);
                var to = new MailAddress(dto.To);
                var message = new MailMessage(from, to);
                message.Body = EmailResources.VerificationBody;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = EmailResources.VerificationSubject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                await client.SendMailAsync(message);
                return true;
            }
        }
    }
}
