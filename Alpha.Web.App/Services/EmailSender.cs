using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Alpha.Infrastructure.Email;
using Alpha.Web.App.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Alpha.Web.App.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _environment;
        private IHttpContextAccessor _httpContextAccessor;
        public EmailSender(IConfiguration configuration, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task SendEmailAsync(string recipientEmail, string senderEmail, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("EmailConfiguration");
            SmtpClient client = new SmtpClient(emailSettings["SmtpServer"]);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(emailSettings["From"], emailSettings["SMTPPassword"]);//,emailSettings["SMTPDomain"]
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Port = 587; // port 587it is valid for gmail
            client.EnableSsl = true;

            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            try
            {
                mailMessage.From = new MailAddress(senderEmail);
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
            mailMessage.To.Add(recipientEmail);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            client.Send(mailMessage);

            return Task.CompletedTask;
        }

        public Task SendEmailConfirmationLink(string activationLink, string userName, string emailAddress)
        {
            string messageBody = EmailHelper.GetEmailTemplate(_environment,
                EmailTemplatesSettings.EmailConfirmation.HtmlTemplateName);

            return CreateAndSendMessageBody(messageBody,
                userName,
                emailAddress,
                activationLink,
                EmailTemplatesSettings.EmailConfirmation.Subject);
        }

        public Task SendResetPasswordLink(string activationLink, string userName, string emailAddress)
        {
            string messageBody = EmailHelper.GetEmailTemplate(_environment,
                EmailTemplatesSettings.PasswordForgot.HtmlTemplateName);

            return CreateAndSendMessageBody(messageBody,
                userName,
                emailAddress,
                activationLink,
                EmailTemplatesSettings.PasswordForgot.Subject);
        }

        private Task CreateAndSendMessageBody(string messageBody, string userName, string emailAddress, string activationLink, string emailSubject)
        {
            //var imageUrl = EmailHelper.GetImagesUrl(_configuration, _httpContextAccessor.HttpContext.Request);
            var appSettings = _configuration.GetSection("appSettings");
            var websiteUrl = appSettings["WebsiteUrl"].ToString();
            var websiteName = appSettings["WebsiteName"].ToString();
            var supportUrl = appSettings["SupportUrl"].ToString();
            messageBody = messageBody.Replace(EmailTemplatesSettings.ReplaceToken.WebsiteUrl, websiteUrl);
            messageBody = messageBody.Replace(EmailTemplatesSettings.ReplaceToken.WebsiteName, websiteName);
            messageBody = messageBody.Replace(EmailTemplatesSettings.ReplaceToken.SupportUrl, supportUrl);

            var tokenValues = new Dictionary<string, string>
            {
                //{ EmailTemplatesSettings.ReplaceToken.ImagesUrl, imageUrl },
                { EmailTemplatesSettings.ReplaceToken.UserName, userName},
                { EmailTemplatesSettings.ReplaceToken.ActionUrl, HtmlEncoder.Default.Encode(activationLink)}
            };

            foreach (var key in tokenValues.Keys)
            {
                messageBody = messageBody.Replace(key, tokenValues[key]);
            }

            var sender = _configuration.GetSection("EmailConfiguration");

            var senderAddress = sender["From"].ToString();// EmailTemplatesSettings.AccountActivation.SenderAddressPrefix + "@" + EmailTemplatesSettings.EmailSenderDomain;

            return SendEmailAsync(emailAddress,
                senderAddress,
                emailSubject,
                messageBody);
        }
    }
}