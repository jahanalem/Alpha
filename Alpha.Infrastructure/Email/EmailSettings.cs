﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Alpha.Infrastructure.Email
{
    public static class EmailSettings
    {
        public const string HtmlTemplateImpressum = "Impressum.html";
        public const string EmailSenderDomain = @"gmail.com";
        public static class AccountActivation
        {
            public const string SenderAddressPrefix = "test19application";
            public const string Subject = @"FE-Portal: Bitte aktivieren Sie Ihren Zugang";
            public const string HtmlTemplateName = "Confirm_Account_Registration.html";
        }

        public static class PasswordForgot
        {
            public const string SenderAddressPrefix = "nicht-antworten";
            public const string Subject = @"Alpha: Password reset";
            public const string HtmlTemplateName = "Reset_Password.html";
        }

        public static class EmailConfirm
        {
            public const string SenderAddressPrefix = "nicht-antworten";
            public const string Subject = @"Alpha: Bitte bestätigen Sie Ihre E-Mail-Adresse";
            public const string HtmlTemplateName = "Confirm_Email.html";
        }

        public static class ReplaceToken
        {
            public const string ImagesUrl = "{{imagesUrl}}";
            public const string UserName = "{{userName}}";
            public const string Link = "{{link}}";
            public const string Impressum = "{{impressum}}";
            public const string RequestId = "{{RequestId}}";
            public const string TimeOfError = "{{TimeOfError}}";
            public const string DetailOfError = "{{DetailOfError}}";
            public const string UserEmail = "{{UserEmail}}";
        }
    }
}
