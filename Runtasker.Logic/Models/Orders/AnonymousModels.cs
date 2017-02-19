using Runtasker.Logic.Entities;
using Runtasker.Resources.Models.OrderModels.RegAndCreate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Runtasker.Logic.Models.Orders
{
    public class AnonymousKnowThePrice
    {
        [Display(Name = "Name", ResourceType = typeof(RegAndCreateRes))]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email", ResourceType = typeof(RegAndCreateRes))]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PhoneNumber", ResourceType = typeof(RegAndCreateRes))]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "WorkType", ResourceType = typeof(RegAndCreateRes))]
        public OrderWorkType WorkType { get; set; }

        [Required]
        [Display(Name = "Subject", ResourceType = typeof(RegAndCreateRes))]
        public Subject Subject { get; set; }

        [Display(Name = "OtherSubject", ResourceType = typeof(RegAndCreateRes))]
        public string OtherSubject { get; set; }

        [Display(Name = "CompletionDate", ResourceType = typeof(RegAndCreateRes))]
        public DateTime CompletionDate { get; set; }

        [Display(Name = "Files", ResourceType = typeof(RegAndCreateRes))]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        [Display(Name = "Description", ResourceType = typeof(RegAndCreateRes))]
        public string Description { get; set; }
    }
}
