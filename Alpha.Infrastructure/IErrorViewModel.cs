using System;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Infrastructure
{
    public interface IErrorViewModel
    {
        public string RequestId { get; set; }

        public DateTime TimeOfError { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        [Display(Name = "Email")]
        [Required(ErrorMessage = "The email address is not valid.")]
        [EmailAddress]
        public string ReporterEmail { get; set; }
    }
}
