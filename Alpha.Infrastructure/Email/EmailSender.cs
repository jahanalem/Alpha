using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Alpha.Infrastructure.Email
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private IHostingEnvironment _environment;
        public EmailSender(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public Task SendEmailAsync(string empfaengerEmail, string absenderEmail, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("Email");
            SmtpClient client = new SmtpClient(emailSettings["SmtpServer"]);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(emailSettings["SMTPUserName"], emailSettings["SMTPPassword"], emailSettings["SMTPDomain"]);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress(absenderEmail);
            mailMessage.To.Add(empfaengerEmail);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            client.Send(mailMessage);

            return Task.CompletedTask;
        }
    }
}
