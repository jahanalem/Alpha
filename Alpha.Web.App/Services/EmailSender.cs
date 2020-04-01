using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Alpha.Infrastructure.Email;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Alpha.Web.App.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _environment;
        public EmailSender(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public Task SendEmailAsync(string recipientEmail, string senderEmail, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("Email");
            SmtpClient client = new SmtpClient(emailSettings["SmtpServer"]);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(emailSettings["SMTPUserName"], emailSettings["SMTPPassword"]);//,emailSettings["SMTPDomain"]
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Port = 587; // port 587it is valid for gmail
            client.EnableSsl = true;
                
            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress(senderEmail);
            mailMessage.To.Add(recipientEmail);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            client.Send(mailMessage);

            return Task.CompletedTask;
        }
    }
}
