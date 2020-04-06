using System;

namespace Alpha.Web.App.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public DateTime TimeOfError { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
