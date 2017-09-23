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
        [Required]
        [JsRequired(resourceName: "NameRequiredError", resourceType: typeof(RegAndCreateRes))]
        [JsMinLength(minLength: 2, resourceType: typeof(RegAndCreateRes), resourceName: "NameLengthError")]
        [Display(Name = "Name", ResourceType = typeof(RegAndCreateRes))]
        [Placeholder(resourceType: typeof(RegAndCreateRes), resourceName: "NamePlaceholder")]
        [PopoverInfo(resourceType: typeof(RegAndCreateRes), resourceName: "NamePopoverInfo")]
        [Tooltip(resourceType: typeof(RegAndCreateRes), resourceName: "NamePopoverInfo")]
        public string Name { get; set; }

        [Required]
        [JsRequired(resourceType: typeof(RegAndCreateRes), resourceName: "EmailRequiredError")]
        [JsEmail(resourceType: typeof(RegAndCreateRes), resourceName: "NotValidEmailError")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email", ResourceType = typeof(RegAndCreateRes))]
        [Placeholder(resourceType: typeof(RegAndCreateRes), resourceName: "EmailPlaceholder")]
        [PopoverInfo(resourceType: typeof(RegAndCreateRes), resourceName: "EmailPopoverInfo")]
        [Tooltip(resourceType: typeof(RegAndCreateRes), resourceName: "EmailPopoverInfo")]
        public string Email { get; set; }

        //[DataType(DataType.PhoneNumber)]
        //[Display(Name = "PhoneNumber", ResourceType = typeof(RegAndCreateRes))]
        //[PopoverInfo(resourceName: "PhoneNumberPopoverInfo", resourceType: typeof(RegAndCreateRes))]
        //public string PhoneNumber { get; set; }

        [JsNotValidate]
        [Required]
        [Display(Name = "WorkType", ResourceType = typeof(RegAndCreateRes))]
        [PopoverInfo(resourceName: "WorkTypePopoverInfo", resourceType: typeof(RegAndCreateRes))]
        [Tooltip(resourceName: "WorkTypePopoverInfo", resourceType: typeof(RegAndCreateRes))]
        public OrderWorkType WorkType { get; set; }

        [Required]
        [Display(Name = "Subject", ResourceType = typeof(CreateOrder))]
        [PopoverInfo(typeof(CreateOrder), resourceName: "SubjectPopoverInfo")]
        [Tooltip(typeof(CreateOrder), resourceName: "SubjectPopoverInfo")]
        [JsOnValueWithElse(Value = "0",
            OnValueScript = " ClearPropertyInput(\"OtherSubject\"); ShowObject(\"OtherSubjectForm\"); ",
            OnElseScript = " HideObject(\"OtherSubjectForm\"); SetValueForInput(\"OtherSubject\", \"selected\")")]
        public Subject Subject { get; set; }

        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "OtherSubjectError")]
        [JsRequired(resourceType: typeof(CreateOrder), resourceName: "OtherSubjectError")]
        [PopoverInfo(typeof(CreateOrder), resourceName: "OtherSubjectInfo")]
        [Tooltip(typeof(CreateOrder), resourceName: "OtherSubjectInfo")]
        [Placeholder(typeof(CreateOrder), resourceName: "OtherSubjectPlaceholder")]
        [Display(ResourceType = typeof(CreateOrder), Name = "OtherSubject")]
        [JsHideByDefault]
        [JsDefaultValue(DefaultValue = "\"selected\"")]

        public string OtherSubject { get; set; }


        [JsNotValidate]
        [Display(Name = "CompletionDate", ResourceType = typeof(RegAndCreateRes))]
        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "FinishDateError")]
        [PopoverInfo(resourceType: typeof(CreateOrder), resourceName: "FinishDateInfo")]
        [Tooltip(resourceType: typeof(CreateOrder), resourceName: "FinishDateInfo")]
        public DateTime CompletionDate { get; set; }

        [JsNotValidate]
        [Display(Name = "Files", ResourceType = typeof(RegAndCreateRes))]
        [PopoverInfo(typeof(CreateOrder), resourceName: "FileUploadInfo")]
        [Tooltip(typeof(CreateOrder), resourceName: "FileUploadInfo")]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        [Display(ResourceType = typeof(CreateOrder), Name = "Description")]
        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "NeedDescriptionError")]
        [JsRequired(resourceType: typeof(CreateOrder), resourceName: "NeedDescriptionError")]
        [Placeholder(resourceName: "DescriptionPlaceholder", resourceType: typeof(CreateOrder))]
        [Tooltip(resourceName: "DescriptionPlaceholder", resourceType: typeof(CreateOrder))]
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
