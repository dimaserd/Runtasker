using Runtasker.Logic.Attributes;
using Runtasker.Logic.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Runtasker.Resources.Models.CustomerOrderModels;
using Runtasker.Resources.Models.OrderModels.CreateOrder;
using Common.JavascriptValidation.Attributes;
using Extensions.Attributes;

namespace Runtasker.Logic.Models
{    
    public class OrderCreateModel
    {
        [Required(ErrorMessage = "ERROR")]
        [Display(Name = "Subject", ResourceType = typeof(CreateOrder))]
        [PopoverInfo(typeof(CreateOrder), resourceName: "SubjectPopoverInfo")]
        [Tooltip(typeof(CreateOrder), resourceName: "SubjectPopoverInfo")]
        [JsOnValueWithElse(Value = "0",
            OnValueScript = " ClearPropertyInput(\"OtherSubject\"); ShowObject(\"OtherSubjectForm\"); ",
            OnElseScript = " HideObject(\"OtherSubjectForm\"); SetValueForInput(\"OtherSubject\", \"selected\")")]
        public Subject Subject { get; set; }

        [JsNotValidate]
        [Required(ErrorMessage = "WORKTYPE ERROR")]
        [Display(Name = "WorkType", ResourceType = typeof(CreateOrder))]
        [PopoverInfo(resourceName: "WorkTypePopoverInfo", resourceType: typeof(CreateOrder))]
        [Tooltip(resourceName: "WorkTypePopoverInfo", resourceType: typeof(CreateOrder))]
        public OrderWorkType WorkType { get; set; }

        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "OtherSubjectError")]
        [PopoverInfo(typeof(CreateOrder), resourceName: "OtherSubjectInfo")]
        [Tooltip(typeof(CreateOrder), resourceName: "OtherSubjectInfo")]
        [Placeholder(typeof(CreateOrder), resourceName: "OtherSubjectPlaceholder")]
        [Display(ResourceType = typeof(CreateOrder), Name = "OtherSubject")]
        [JsRequired(resourceType: typeof(CreateOrder), resourceName: "OtherSubjectError")]
        [JsHideByDefault]
        [JsDefaultValue(DefaultValue = "\"selected\"")]
        public string OtherSubject { get; set; }


        [Display(ResourceType = typeof(CreateOrder), Name = "Description")]
        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "NeedDescriptionError")]
        [JsRequired(resourceType: typeof(CreateOrder), resourceName : "NeedDescriptionError")]
        [Placeholder(resourceName: "DescriptionPlaceholder", resourceType: typeof(CreateOrder))]
        [Tooltip(resourceName: "DescriptionPlaceholder", resourceType: typeof(CreateOrder))]
        public string Description { get; set; }

        [JsNotValidate]
        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "FinishDateError")]
        [Display(ResourceType = typeof(CreateOrder), Name = "FinishDate")]
        [PopoverInfo(resourceType: typeof(CreateOrder), resourceName: "FinishDateInfo")]
        [Tooltip(resourceType: typeof(CreateOrder), resourceName: "FinishDateInfo")]
        public DateTime FinishDate { get; set; }

        [JsNotValidate]
        [DataType(DataType.Upload, ErrorMessage = "FilesError")]
        [Display(ResourceType = typeof(CreateOrder), Name = "FileUpload")]
        [Tooltip(resourceType: typeof(CreateOrder), resourceName: "FileUploadInfo")]
        [PopoverInfo(typeof(CreateOrder), resourceName: "FileUploadInfo")]
        public IEnumerable<HttpPostedFileBase> FileUpload { get; set; }


    }

    /// <summary>
    /// Статический класс инкапсулирующий некоторые методы для работы с сущностью создания заказа
    /// </summary>
    public static class OrderCreateModelExtensions
    {
        public static Order ToOrder(this OrderCreateModel orderModel, string userGuid)
        {
            //получаю вложение для созданного заказа
            Attachment attachment = AttachmentExtensions.GetAttachmentFromFiles(orderModel.FileUpload);

            if(attachment != null)
            {
                attachment.Type = AttachmentType.OrderFiles;
            }
            

            return new Order
            {
                Description = orderModel.Description,
                FinishDate = orderModel.FinishDate,
                Status = OrderStatus.New,
                PublishDate = DateTime.Now,
                WorkType = orderModel.WorkType,
                Subject = orderModel.Subject,
                //вкладываю в заказ вложения
                Attachments = ( (attachment != null)? new List<Attachment> { attachment} : null),
                HasCustomerFiles = (attachment != null),
                OtherSubject = orderModel.OtherSubject,
                UserGuid = userGuid,
                PerformerGuid = userGuid,
            };
        }
    }

    #region Solve Error models
    public class OrderAddFilesModel
    {
        public int OrderId { get; set; }

        [Required(ErrorMessageResourceType = typeof(CustOrderModelsRes), ErrorMessageResourceName = "FilesError")]
        [Display(Name = "Files", ResourceType = typeof(CustOrderModelsRes))]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }

    public class AddDescriptionModel
    {
        public int OrderId { get; set; }

        [Display(Name = "Description", ResourceType = typeof(CustOrderModelsRes))]
        [Required(ErrorMessageResourceName = "DescriptionError", ErrorMessageResourceType = typeof(CustOrderModelsRes))]
        public string Description { get; set; }
    }
    #endregion

    

    public class RatingOrderModel
    {
        public int OrderId { get; set; }

        public string Text { get; set; }

        [RatingSize(5)]
        public int Rating { get; set; }
    }
}