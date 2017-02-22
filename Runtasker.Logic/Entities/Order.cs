using Runtasker.Logic.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Runtasker.Resources.Entities.Order;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Newtonsoft.Json;
using Extensions.Attributes;

namespace Runtasker.Logic.Entities
{
    #region Enums with extensions

    #region Enumerations
    public enum OrderStatus
    {
        [Description("Новый")]
        New,
        [Description("Оценен")]
        Valued,
        [Description("Оплачен наполовину")]
        HalfPaid,
        [Description("Выполняется")]
        Executing,
        [Description("Выполнен")]
        Finished,
        [Description("Полностью оплачен")]
        Paid,
        [Description("Скачан пользователем")]
        Downloaded,
        [Description("Оценен пользователем")]
        Appreciated,
        [Description("Обнаружена ошибка")]
        HasError
    }

    public enum OrderErrorType
    {
        [MyDescription(typeof(OrderResource), resourceName: "ErrorTypeNone")]
        None,
        [MyDescription(typeof(OrderResource), resourceName: "ErrorTypeNeedDesc")]
        NeedDescription,
        [MyDescription(typeof(OrderResource), resourceName: "ErrorTypeNeedFiles" )]
        NeedFiles
    }

    public enum OrderWorkType
    {
        [MyDescription(typeof(OrderResource), "Ordinary")]
        Ordinary,
        [MyDescription(typeof(OrderResource), "Essay")]
        Essay,
        [MyDescription(typeof(OrderResource), "CourseWork")]
        CourseWork
    }

    public enum Subject
    {
        [MyDescription(typeof(OrderResource), "Other")]
        Other,

        [SpecifyDropdown(typeof(OrderResource), dropdownName: "LangDropdownParams", labelName: "LangLabel")]
        [MyDescription(typeof(OrderResource), "Language")]
        [PopoverInfo(typeof(OrderResource), "LanguagePopover")]
        ForeignLanguage,

        [MyDescription(typeof(OrderResource), "AdvancedMathematics")]
        AdvancedMathematics,

        [MyDescription(typeof(OrderResource), "Chemistry")]
        Chemistry,

        [MyDescription(typeof(OrderResource), "TheoreticalMechanics")]
        TheoreticalMechanics,

        [MyDescription(typeof(OrderResource), "Physics")]
        Physics,

        [MyDescription(typeof(OrderResource), "StrengthOfMaterials")]
        StrengthOfMaterials,

        [MyDescription(typeof(OrderResource), "Informatics")]
        Informatics,

        [MyDescription(typeof(OrderResource), "Programming")]
        Programming,

        [MyDescription(typeof(OrderResource), "Projecting")]
        Projecting
    }

    #endregion

    public static class OrderExtensions
    {
        public static bool IsFree(this Order order)
        {
            if( (order.Status == OrderStatus.New ||
                order.Status == OrderStatus.HasError ||
                order.Status == OrderStatus.Valued ||
                //полуоплаченный заказ еще не занят
                order.Status == OrderStatus.HalfPaid
                )
                && order.PerformerGuid == order.UserGuid )
            {
                return true;
            }

            return false;
        }

        #region Subject Extensions
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

        public static string ToDescriptionString(this Subject val)
        {
            MyDescriptionAttribute[] attributes = (MyDescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(MyDescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
        #endregion

        public static string ToDescriptionString(this OrderErrorType val)
        {
            MyDescriptionAttribute[] attributes = (MyDescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(MyDescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        #region OrderStatus Extensions
        public static string ToDescriptionString(this OrderStatus val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string GetColorClass(this OrderStatus val)
        {
            string result = "";
            switch (val)
            {
                case OrderStatus.New:
                    result += "default";
                    break;

                case OrderStatus.Valued:
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

                case OrderStatus.Paid:
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
            return ((int)val < (int)OrderStatus.Downloaded || val == OrderStatus.HasError) ? "active" : "finished";
        }
        #endregion

        #region WorkType Extensions
        public static string ToDescriptionString(this OrderWorkType val)
        {
            MyDescriptionAttribute[] attributes = (MyDescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(MyDescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
        #endregion

    }
    #endregion

    public class Order
    {
        #region Constructors
        public Order()
        {
            Messages = new List<Message>();
        }
        #endregion

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public OrderStatus Status { get; set; }

        public OrderErrorType ErrorType { get; set; }

        public OrderWorkType WorkType { get; set; }

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

        [Required]
        public Subject Subject { get; set; }

        public string OtherSubject { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public string Attachments { get; set; }

        [Display(Name = "Finish Date")]
        public DateTime FinishDate { get; set; }

        public DateTime PublishDate { get; set; }

        public decimal PaidSum { get; set; }

        public decimal Sum { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
        [JsonIgnore]
        public virtual ICollection<Message> Messages { get; set; }

        

    }

}