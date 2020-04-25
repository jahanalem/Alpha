using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Web.App.Resources.AppSettingsFileModel.EmailTemplates
{
    public class EmailTemplatesSettingsModel
    {
        public IncomingMessage IncomingMessage { get; set; }
        public ErrorMessage ErrorMessage { get; set; }
    }

    public class IncomingMessage
    {
        public string HtmlTemplateName { get; set; }
    }

    public class ErrorMessage
    {
        public string HtmlTemplateName { get; set; }
    }
}
