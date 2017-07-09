using Runtasker.Logic.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Runtasker.Resources.Entities.Order;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Newtonsoft.Json;
using Extensions.Attributes;
using Extensions.Enumerations;

namespace Runtasker.Logic.Entities
{
    #region Перечисления
    
    public enum OrderStatus
    {
        [Display(Name = "Новый")]
        New,
        [Display(Name = "Оценен")]
        Estimated,
        [Display(Name = "Оплачен наполовину")]
        HalfPaid,
        [Display(Name = "Выполняется")]
        Executing,
        [Display(Name = "Выполнен")]
        Finished,
        [Display(Name = "Полностью оплачен")]
        FullPaid,
        [Display(Name = "Скачан пользователем")]
        Downloaded,
        [Display(Name = "Оценен пользователем")]
        Appreciated,
        [Display(Name = "Обнаружена ошибка")]
        HasError,
        
    }

    public enum OrderErrorType
    {
        [Display(Name = "ErrorTypeNone", ResourceType = typeof(OrderResource))]
        None,
        [Display(Name = "ErrorTypeNeedDesc", ResourceType = typeof(OrderResource))]
        NeedDescription,
        [Display(Name = "ErrorTypeNeedFiles", ResourceType = typeof(OrderResource))]
        NeedFiles
    }

    public enum OrderWorkType
    {
        [Display(Name = "Ordinary", ResourceType = typeof(OrderResource))]
        Ordinary,
        [Display(Name = "Essay", ResourceType = typeof(OrderResource))]
        Essay,
        [Display(Name = "CourseWork", ResourceType = typeof(OrderResource))]
        CourseWork,
        [Display(Name = "OnlineHelp", ResourceType = typeof(OrderResource))]
        OnlineHelp
    }

    public enum Subject
    {
        [Display(Name = "Other", ResourceType = typeof(OrderResource))]
        Other,

        
        [Display(Name =  "Language")]
        [PopoverInfo(typeof(OrderResource), "LanguagePopover")]
        ForeignLanguage,

        [Display(Name = "AdvancedMathematics", ResourceType = typeof(OrderResource))]
        AdvancedMathematics,

        [Display(Name = "Chemistry", ResourceType = typeof(OrderResource))]
        Chemistry,

        [Display(Name = "TheoreticalMechanics", ResourceType = typeof(OrderResource))]
        TheoreticalMechanics,

        [Display(Name = "Physics", ResourceType = typeof(OrderResource))]
        Physics,

        [Display(Name = "StrengthOfMaterials", ResourceType = typeof(OrderResource))]
        StrengthOfMaterials,

        [Display(Name = "Informatics", ResourceType = typeof(OrderResource))]
        Informatics,

        [Display(Name = "Programming", ResourceType = typeof(OrderResource))]
        Programming,

        [Display(Name = "Projecting", ResourceType = typeof(OrderResource))]
        Projecting
    }

    
    #endregion


    public class Order
    {
        #region Конструктор
        public Order()
        {
            Messages = new List<Message>();
        }
        #endregion

        #region Свойства
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Статус заказа
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// Тип ошибки указывается только тогда заказ не принят исполнителем или администратором
        /// и требуются типичные исправления от заказчика
        /// </summary>
        public OrderErrorType ErrorType { get; set; }

        public OrderWorkType WorkType { get; set; }

        #region Свойства отношений
        [Required]
        [StringLength(128)]
        [ForeignKey("Customer")]
        public string UserGuid { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser Customer { get; set; }


        [Required]
        [StringLength(128)]
        [ForeignKey("Performer")]
        public string PerformerGuid { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser Performer { get; set; }


        [ForeignKey("CustomerFiles")]
        public virtual string CustomerFilesId { get; set; }
        /// <summary>
        /// Вложение в котором содержатся файлы размещенные заказчиком при добавлении заказа
        /// </summary>
        [JsonIgnore]
        public virtual Attachment CustomerFiles { get; set; }


        [ForeignKey("Solution")]
        public virtual string SolutionId { get; set; }
        /// <summary>
        /// Решение заказа (добавляется исполнителем или администратором)
        /// </summary>
        [JsonIgnore]
        public virtual OrderSolution Solution { get; set; }



        [JsonIgnore]
        public virtual ICollection<Message> Messages { get; set; }
        #endregion

        [Required]
        public Subject Subject { get; set; }

        /// <summary>
        /// Другой предмет (заполняется только тогда когда основного предмета нет в списке)
        /// </summary>
        public string OtherSubject { get; set; }

        /// <summary>
        /// Описание заказа (Заполняется заказчиком)
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        

        /// <summary>
        /// Дата к которой нужно выполнить заказ
        /// </summary>
        [Display(Name = "Finish Date")]
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// Дата публикации заказа
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Оплаченная сумма пользователем
        /// </summary>
        public decimal PaidSum { get; set; }

        /// <summary>
        /// Сумма на которую исполнитель оценил заказ
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Выставленная оценка заказа после завершения
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Комментарий заказчика по заказу
        /// </summary>
        public string Comment { get; set; }

        #endregion

    }

    public static class OrderExtensions
    {
        public static bool IsFree(this Order order)
        {

            if ((order.Status == OrderStatus.New ||
                order.Status == OrderStatus.HasError ||
                order.Status == OrderStatus.Estimated ||
                //полуоплаченный заказ еще не занят
                order.Status == OrderStatus.HalfPaid
                )
                && order.PerformerGuid == order.UserGuid)
            {
                return true;
            }

            return false;
        }

        #region Subject Extensions



        /// <summary>
        /// Возвращает название предмета
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string GetSubjectName(this Order order)
        {
            if (order.Subject == Subject.Other)
            {
                return order.OtherSubject;
            }
            else
            {
                Subject subject = order.Subject;
                return subject.ToDisplayName();
            }

        }

        public static string GetSpecifyDropdownInfo(this Subject val)
        {
            SpecifyDropdownAttribute[] attributes = (SpecifyDropdownAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(SpecifyDropdownAttribute), false);
            return attributes.Length > 0 ? attributes[0].DropdownParams : string.Empty;
        }

        public static string GetLabelInfo(this Subject val)
        {
            SpecifyDropdownAttribute[] attributes = (SpecifyDropdownAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(SpecifyDropdownAttribute), false);
            return attributes.Length > 0 ? attributes[0].Label : string.Empty;
        }

        public static string GetPopoverInfo(this Subject val)
        {
            PopoverInfoAttribute[] attributes = (PopoverInfoAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(PopoverInfoAttribute), false);
            return attributes.Length > 0 ? attributes[0].Info : string.Empty;
        }

       


        #endregion

        

        #region OrderStatus Extensions

        public static string GetColorClass(this OrderStatus val)
        {
            string result = "";
            switch (val)
            {
                case OrderStatus.New:
                    result += "default";
                    break;

                case OrderStatus.Estimated:
                    result += "info";
                    break;

                case OrderStatus.HalfPaid:
                    result += "info";
                    break;

                case OrderStatus.Executing:
                    result += "info";
                    break;

                case OrderStatus.Finished:
                    result += "success";
                    break;

                case OrderStatus.FullPaid:
                    result += "success";
                    break;

                case OrderStatus.Downloaded:
                    result += "success";
                    break;

                case OrderStatus.HasError:
                    result += "danger";
                    break;

                default:
                    result += "default";
                    break;

            }
            return result;
        }

        public static string GetActiveStatus(this OrderStatus val)
        {
            if (val == OrderStatus.New || val == OrderStatus.HasError || val == OrderStatus.Estimated
                || val == OrderStatus.HalfPaid || val == OrderStatus.FullPaid ||
                val == OrderStatus.Executing)
            {
                return "active";
            }

            return "finished";
        }

        
        #endregion

    }

}