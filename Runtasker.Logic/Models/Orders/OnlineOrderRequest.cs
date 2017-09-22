using Common.JavascriptValidation.Attributes;
using Extensions.Attributes;
using Runtasker.Logic.Attributes;
using Runtasker.Logic.Entities;
using Runtasker.Resources.Models.OrderModels.CreateOrder;
using Runtasker.Resources.Models.OrderModels.OnlineOrder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Runtasker.Logic.Models.Orders
{
    public class OnlineOrderRequest
    {
        public OrderWorkType OnlineHelpWorkType = OrderWorkType.OnlineHelp;
        /// <summary>
        /// Точная дата по московскому времени во сколько нужно оказать помощь
        /// </summary>
        [PopoverInfo(typeof(OnlineOrderRes), resourceName: "StartDatePopoverInfo")]
        [Tooltip(typeof(OnlineOrderRes), resourceName: "StartDatePopoverInfo")]
        [Display(Name = "StartDate", ResourceType = typeof(OnlineOrderRes))]
        public DateTime StartDate { get; set; }

        [Display(Name = "Description", ResourceType = typeof(OnlineOrderRes))]
        [JsRequired(resourceType: typeof(CreateOrder), resourceName: "NeedDescriptionError")]
        [Placeholder(resourceName: "DescriptionPlaceholder", resourceType: typeof(CreateOrder))]
        [Tooltip(resourceName: "DescriptionPlaceholder", resourceType: typeof(CreateOrder))]
        public string Description { get; set; }

        [Display(Name = "Subject", ResourceType = typeof(OnlineOrderRes))]
        [JsOnValueWithElse(Value = "0",
            OnValueScript = " ClearPropertyInput(\"OtherSubject\"); ShowObject(\"OtherSubjectForm\"); ",
            OnElseScript = " HideObject(\"OtherSubjectForm\"); SetValueForInput(\"OtherSubject\", \"selected\")")]
        [Tooltip(typeof(CreateOrder), resourceName: "SubjectPopoverInfo")]
        public Subject Subject { get; set; }

        [Display(Name = "FileUpload", ResourceType = typeof(OnlineOrderRes))]
        [PopoverInfo(typeof(OnlineOrderRes), resourceName: "FilesUploadInfo")]
        [Tooltip(typeof(OnlineOrderRes), resourceName: "FilesUploadInfo")]
        public IEnumerable<HttpPostedFileBase> FileUpload { get; set; }


        [Display(Name = "PhoneNumber", ResourceType = typeof(OnlineOrderRes))]
        [ErrorText( typeof(OnlineOrderRes), resourceName: "PhoneNumberErrorText")]
        [PopoverInfo(typeof(OnlineOrderRes), resourceName: "PhoneNumberInfo")]
        [Tooltip(typeof(OnlineOrderRes), resourceName: "PhoneNumberInfo")]
        [Placeholder(typeof(OnlineOrderRes), resourceName: "PhoneNumberPlaceholder")]
        public string PhoneNumber { get; set; }

        [Display(ResourceType = typeof(CreateOrder), Name = "OtherSubject")]
        [PopoverInfo(typeof(CreateOrder), resourceName: "OtherSubjectInfo")]
        [Tooltip(typeof(CreateOrder), resourceName: "OtherSubjectInfo")]
        [Placeholder(typeof(CreateOrder), resourceName: "OtherSubjectPlaceholder")]      
        [JsRequired(resourceType: typeof(CreateOrder), resourceName: "OtherSubjectError")]
        [JsHideByDefault]
        [JsDefaultValue(DefaultValue = "\"selected\"")]
        public string OtherSubject { get; set; }
    }

    public static class OnlineOrderRequestExtensions
    {
        public static OrderCreateModel ToOrderCreateModel(this OnlineOrderRequest online)
        {
            return new OrderCreateModel
            {
                Subject = online.Subject,
                OtherSubject = online.OtherSubject,
                FinishDate = online.StartDate,
                FileUpload = online.FileUpload,
                Description = online.Description,
                WorkType = online.OnlineHelpWorkType
            };
        }
    }
}
