using System;
using System.Collections.Generic;
using System.Text;

namespace Alpha.Infrastructure.Email
{
    public static class EmailTemplatesSettings
    {
        public const string HtmlTemplateImpressum = "Impressum.html";
        public const string EmailSenderDomain = @"gmail.com";
        public static class AccountActivation
        {
            public const string Subject = @"Green Codes: Please activate your access";
            public const string HtmlTemplateName = "Confirm_Account_Registration.html";
        }

        public static class PasswordForgot
        {
            public const string Subject = @"Green Codes: Password reset";
            public const string HtmlTemplateName = "Reset_Password.html";
        }

        public static class EmailConfirmation
        {
            public const string SenderAddressPrefix = "test19application";
            public const string Subject = @"Green Codes: Please confirm the Email Address";
            public const string HtmlTemplateName = "Confirm_Email.html";
        }

        public static class ReplaceToken
        {
            public const string ImagesUrl = "{{imagesUrl}}";
            public const string UserName = "{{name}}";
            public const string ActionUrl = "{{actionUrl}}";
            public const string Impressum = "{{impressum}}";
            public const string RequestId = "{{RequestId}}";
            public const string TimeOfError = "{{TimeOfError}}";
            public const string DetailOfError = "{{DetailOfError}}";
            public const string UserEmail = "{{UserEmail}}";
            public const string WebsiteUrl = "{{websiteUrl}}";
            public const string WebsiteName = "{{websiteName}}";
            public const string SupportUrl = "{{supportUrl}}";
        }
    }
}