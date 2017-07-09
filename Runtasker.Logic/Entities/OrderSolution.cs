using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Runtasker.Logic.Entities
{

    /// <summary>
    /// Сущность описывающая решение заказа
    /// </summary>
    public class OrderSolution
    {
        public string Id { get; set; }

        /// <summary>
        /// Дата создания решения 
        /// </summary>
        public DateTime CreationDate { get; set; }

        #region Свойства отношений
        [ForeignKey("ToOrder")]
        public int OrderId { get; set; }
        /// <summary>
        /// Указывает на заказ для которого данная сущность является решением
        /// </summary>
        public Order ToOrder { get; set; }


        
        

        [ForeignKey("Performer")]
        public string PerformerId { get; set; }

        /// <summary>
        /// Исполнитель тот кто выполнил работу
        /// </summary>
        public ApplicationUser Performer { get; set; }

        #endregion
    }

    /// <summary>
    /// Статический класс инкапслирующий методы для работы с сущностью решения заказа
    /// </summary>
    public static class OrderSolutionExtensions
    {
        //public static OrderSolution CreateSolution(int orderId, string performerId, IEnumerable<HttpPostedFileBase> solutionFiles)
        //{
        //    Attachment attachment = AttachmentExtensions.GetAttachmentFromFiles(solutionFiles);

        //    attachment.Type = AttachmentType.OrderSolution;

        //    return new OrderSolution
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        OrderId = orderId,
        //        CreationDate = DateTime.Now,
        //        AttachmentId = attachment.Id,
        //        Solution = attachment,
        //        PerformerId = performerId,
        //    };
        //}
    }
}
