using Common.JavascriptValidation.Attributes;
using Extensions.Attributes;
using Runtasker.Logic.Attributes;
using Runtasker.Logic.Entities;
using Runtasker.Resources.Models.OrderModels.CreateOrder;
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
        [PopoverInfo(resourceType: typeof(RegAndCreateRes), resourceName: "NamePopoverInfo")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email", ResourceType = typeof(RegAndCreateRes))]
        [PopoverInfo(resourceType: typeof(RegAndCreateRes), resourceName: "EmailPopoverInfo")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PhoneNumber", ResourceType = typeof(RegAndCreateRes))]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "WorkType", ResourceType = typeof(RegAndCreateRes))]
        public OrderWorkType WorkType { get; set; }

        [Required]
        [Display(Name = "Subject", ResourceType = typeof(CreateOrder))]
        [PopoverInfo(typeof(CreateOrder), resourceName: "SubjectPopoverInfo")]
        [JsOnValueWithElse(Value = "0",
            OnValueScript = " ClearPropertyInput(\"OtherSubject\"); ShowObject(\"OtherSubjectForm\"); ",
            OnElseScript = " HideObject(\"OtherSubjectForm\"); SetValueForInput(\"OtherSubject\", \"selected\")")]
        public Subject Subject { get; set; }

        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "OtherSubjectError")]
        [JsRequired(resourceType: typeof(CreateOrder), resourceName: "OtherSubjectError")]
        [PopoverInfo(typeof(CreateOrder), resourceName: "OtherSubjectInfo")]
        [Display(ResourceType = typeof(CreateOrder), Name = "OtherSubject")]
        [JsHideByDefault]
        [JsDefaultValue(DefaultValue = "\"selected\"")]
        public string OtherSubject { get; set; }

        [Display(Name = "CompletionDate", ResourceType = typeof(RegAndCreateRes))]
        [JsNotValidate]
        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "FinishDateError")]
        [PopoverInfo(resourceType: typeof(CreateOrder), resourceName: "FinishDateInfo")]
        public DateTime CompletionDate { get; set; }

        [JsNotValidate]
        [Display(Name = "Files", ResourceType = typeof(RegAndCreateRes))]
        [PopoverInfo(typeof(CreateOrder), resourceName: "FileUploadInfo")]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        [Display(ResourceType = typeof(CreateOrder), Name = "Description")]
        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "NeedDescriptionError")]
        [JsRequired(resourceType: typeof(CreateOrder), resourceName: "NeedDescriptionError")]
        [Placeholder(resourceName: "DescriptionPlaceholder", resourceType: typeof(CreateOrder))]
        public string Description { get; set; }
    }

    public static class AnonumousKnowThePriceExtensions
    {
        public static RegisterModel ToRegisterModel(this AnonymousKnowThePrice model, string pass)
        {
            return new RegisterModel
            {
                Email = model.Email,
                Password = pass,
                ConfirmPassword = pass,
                Name = model.Name
            };
        }

        public static OrderCreateModel ToOrderCreateModel(this AnonymousKnowThePrice model)
        {
            return new OrderCreateModel
            {
                WorkType = model.WorkType,
                Description = model.Description,
                FinishDate = model.CompletionDate,
                FileUpload = model.Files,
                OtherSubject = model.OtherSubject,
                Subject = model.Subject
            };
        }
    }
}
