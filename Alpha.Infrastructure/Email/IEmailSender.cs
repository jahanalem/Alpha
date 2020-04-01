using System.Threading.Tasks;

namespace Alpha.Infrastructure.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string empfaengerEmail, string absenderEmail, string subject, string message);
    }
}