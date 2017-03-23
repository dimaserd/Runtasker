﻿using Runtasker.Logic.Attributes;
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
        [Display(Name = "StartDate", ResourceType = typeof(OnlineOrderRes))]
        public DateTime StartDate { get; set; }

        [Display(Name = "Description", ResourceType = typeof(OnlineOrderRes))]
        public string Description { get; set; }

        [Display(Name = "Subject", ResourceType = typeof(OnlineOrderRes))]
        public Subject Subject { get; set; }

        [Display(Name = "FileUpload", ResourceType = typeof(OnlineOrderRes))]
        [PopoverInfo(typeof(OnlineOrderRes), resourceName: "FilesUploadInfo")]
        public IEnumerable<HttpPostedFileBase> FileUpload { get; set; }

        /// <summary>
        /// ПОдсказка взята из другого ресурса
        /// </summary>
        [PopoverInfo(typeof(CreateOrder), resourceName: "OtherSubjectInfo")]
        [Display(ResourceType = typeof(CreateOrder), Name = "OtherSubject")]
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
