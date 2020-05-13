using System;
using System.Collections.Generic;
using System.Text;

namespace Alpha.Infrastructure.Email
{
    public static class EmailUtility
    {
        public static bool IsValidEmail(this string emailAddress)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(emailAddress);
                return addr.Address == emailAddress;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
