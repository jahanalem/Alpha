﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Web.App.Resources.Constants
{
    public static class Messages
    {
        public const string SendingMessageSuccessfully = "Your message has been successfully sent.I will contact you very soon!";
        public const string SendingMessageFailed = "Sending message failed!";
        public const string SuccessStatus = "success";
        public const string FailedStatus = "failed";

        /// <summary>
        /// You can use as 'alert-success' class in bootstrap .
        /// </summary>
        public const string SuccessAlertType = "success";
        /// <summary>
        /// You can use as 'alert-danger' class in bootstrap .
        /// </summary>
        public const string ErrorAlertType = "danger";
    }
}