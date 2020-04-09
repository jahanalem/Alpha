using System.Threading.Tasks;

namespace Alpha.Infrastructure.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipientEmail, string senderEmail, string subject, string message);

        Task SendResetPasswordLink(string activationLink, string userName, string emailAddress);

        Task SendEmailConfirmationLink(string activationLink, string userName, string emailAddress);

        Task ForwardIncomingMessageToAdmin(string email, string senderName, string emailSubject, string message);
    }
}