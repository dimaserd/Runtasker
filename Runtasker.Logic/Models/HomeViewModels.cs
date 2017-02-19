using Runtasker.Resources.Models.HomeModels.Contact;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Runtasker.Logic.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ContactRes), ErrorMessageResourceName = "NameError")]
        [Display(ResourceType = typeof(ContactRes), Name = "FullName")]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ContactRes), ErrorMessageResourceName = "EmailReqError")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(ContactRes), ErrorMessageResourceName = "EmailTypeError")]
        [Display(ResourceType = typeof(ContactRes), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(ContactRes), Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(ResourceType = typeof(ContactRes), Name = "Subject")]
        public string Subject { get; set; }

        [Display(ResourceType = typeof(ContactRes), Name = "Message")]
        [Required(ErrorMessageResourceType = typeof(ContactRes), ErrorMessageResourceName = "MessageReqError")]
        public string Message { get; set; }

        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}
