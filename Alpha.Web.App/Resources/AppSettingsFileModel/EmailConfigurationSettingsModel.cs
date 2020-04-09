using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Web.App.Resources.AppSettingsFileModel
{
    public class EmailConfigurationSettingsModel
    {
        public string SMTPServer { get; set; }
        /// <summary>
        /// Sender Provider
        /// </summary>
        public string From { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string SMTPPassword { get; set; }
        public string SMTPDomain { get; set; }
        public string ForwardMessageTo { get; set; }
    }
}
