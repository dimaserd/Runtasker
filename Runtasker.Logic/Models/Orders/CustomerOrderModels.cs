using Runtasker.Logic.Attributes;
using Runtasker.Logic.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Runtasker.Resources.Models.CustomerOrderModels;
using Runtasker.Resources.Models.OrderModels.CreateOrder;

namespace Runtasker.Logic.Models
{    
    public class OrderCreateModel
    {
        [Required(ErrorMessage = "ERROR")]
        [Display(Name = "Subject", ResourceType = typeof(CreateOrder))]
        [PopoverInfo(typeof(CreateOrder), resourceName: "SubjectPopoverInfo")]
        public Subject Subject { get; set; }

        [Required(ErrorMessage = "WORKTYPE ERROR")]
        [Display(Name = "WorkType", ResourceType = typeof(CreateOrder))]
        public OrderWorkType WorkType { get; set; }

        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "OtherSubjectError")]
        [PopoverInfo(typeof(CreateOrder), resourceName: "OtherSubjectInfo")]
        [Display(ResourceType = typeof(CreateOrder), Name = "OtherSubject")]
        public string OtherSubject { get; set; }

        [Display(ResourceType = typeof(CreateOrder), Name = "Description")]
        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "NeedDescriptionError")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "FinishDateError")]
        [Display(ResourceType = typeof(CreateOrder), Name = "FinishDate")]
        public DateTime FinishDate { get; set; }

        [DataType(DataType.Upload, ErrorMessage = "FilesError")]
        [Display(ResourceType = typeof(CreateOrder), Name = "FileUpload")]
        [PopoverInfo(typeof(CreateOrder), resourceName: "FileUploadInfo")]
        public IEnumerable<HttpPostedFileBase> FileUpload { get; set; }


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