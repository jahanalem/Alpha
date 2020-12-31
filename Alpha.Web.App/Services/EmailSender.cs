using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Alpha.Infrastructure;
using Alpha.Infrastructure.Email;
using Alpha.Web.App.Helpers;
using Alpha.Web.App.Models;
using Alpha.Web.App.Resources.AppSettingsFileModel;
using Alpha.Web.App.Resources.AppSettingsFileModel.EmailTemplates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Alpha.Web.App.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _environment;
        private IHttpContextAccessor _httpContextAccessor;
        private IOptions<DomainAndUrlSettingsModel> _domainAndUrlSettings;
        private IOptions<EmailConfigurationSettingsModel> _emailConfigurationSettings;
        private IOptions<EmailTemplatesSettingsModel> _emailTemplatesSettings;

        public EmailSender(IConfiguration configuration,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            IOptions<DomainAndUrlSettingsModel> domainAndUrlSettings,
            IOptions<EmailConfigurationSettingsModel> emailConfigurationSettings,
            IOptions<EmailTemplatesSettingsModel> emailTemplatesSettings)
        {
            _configuration = configuration;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _domainAndUrlSettings = domainAndUrlSettings;
            _emailConfigurationSettings = emailConfigurationSettings;
            _emailTemplatesSettings = emailTemplatesSettings;
        }

        public Task SendEmailAsync(string recipientEmail, string senderEmail, string subject, string message)
        {
            using (SmtpClient client = new SmtpClient(_emailConfigurationSettings.Value.SMTPServer))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_emailConfigurationSettings.Value.From,
                                            _emailConfigurationSettings.Value.SMTPPassword);//,emailSettings["SMTPDomain"]
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Port = _emailConfigurationSettings.Value.Port; // port 587it is valid for gmail
                client.EnableSsl = true;

                using (MailMessage mailMessage = new MailMessage())
                {
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
                }
            }

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

        public Task ForwardIncomingMessageToAdmin(string email, string senderName, string emailSubject, string message)
        {
            var forwardMessageTo = _emailConfigurationSettings.Value.ForwardMessageTo;
            string messageBody = EmailHelper.GetEmailTemplate(_environment,
                _emailTemplatesSettings.Value.IncomingMessage.HtmlTemplateName);
            messageBody = messageBody.Replace(EmailTemplatesSettings.ReplaceToken.UserEmail, email);
            messageBody = messageBody.Replace(EmailTemplatesSettings.ReplaceToken.UserName, senderName);
            messageBody = messageBody.Replace(EmailTemplatesSettings.ReplaceToken.MessageBody, message);

            var senderProvider = _emailConfigurationSettings.Value.From;

            return SendEmailAsync(forwardMessageTo,
                senderProvider,
                emailSubject,
                messageBody);

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

        private Task CreateAndSendMessageBody(string messageBody,
            string userName,
            string emailAddress,
            string activationLink,
            string emailSubject)
        {
            //var imageUrl = EmailHelper.GetImagesUrl(_configuration, _httpContextAccessor.HttpContext.Request);
            var websiteUrl = _domainAndUrlSettings.Value.WebsiteUrl;
            var websiteName = _domainAndUrlSettings.Value.WebsiteName;
            var supportUrl = _domainAndUrlSettings.Value.SupportUrl;
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

            var senderProvider = _emailConfigurationSettings.Value.From;// EmailTemplatesSettings.AccountActivation.SenderAddressPrefix + "@" + EmailTemplatesSettings.EmailSenderDomain;

            return SendEmailAsync(emailAddress,
                senderProvider,
                emailSubject,
                messageBody);
        }

        public Task SendErrorMessageToSupportTeam(IErrorViewModel errorModel)
        {
            var forwardMessageTo = _emailConfigurationSettings.Value.SupportTeamEmail;
            string messageBody = EmailHelper.GetEmailTemplate(_environment,
                _emailTemplatesSettings.Value.ErrorMessage.HtmlTemplateName);
            messageBody = messageBody.Replace(EmailTemplatesSettings.ReplaceToken.UserEmail, errorModel.ReporterEmail)
                .Replace(EmailTemplatesSettings.ReplaceToken.RequestId, errorModel.RequestId)
                .Replace(EmailTemplatesSettings.ReplaceToken.TimeOfError, errorModel.TimeOfError.ToLongDateString());

            var senderProvider = _emailConfigurationSettings.Value.From;

            return SendEmailAsync(forwardMessageTo,
                senderProvider,
                EmailTemplatesSettings.SendErrorMessage.Subject,
                messageBody);
        }
    }
}