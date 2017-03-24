
using Runtasker.Logic.Entities;

namespace Runtasker.ExtensionsUI.UIExtensions.Orders
{
    public class OrderEntityBase
    {
        #region Constructors
        public OrderEntityBase(Order order)
        {
            Order = order;
        }
        #endregion

        #region Protected Properties
        public Order Order { get; set; }
        #endregion

        #region Protected Help Building Methods
        protected string GetTypeOfWork()
        {
            return Order.WorkType.ToDescriptionString();
        }

        protected string GetStarted()
        {
            string result = "<div class='bs-calltoaction bs-calltoaction-";

            result += GetColorClass();

            return result + $"' {GetDataStatus()}><div class='row'>";
        }

        
        string GetDataStatus()
        {
           return $"data-status='{Order.Status.GetActiveStatus()}'";
        }
        

        protected string GetClosed()
        {
            return "</div></div>";
        }

        public string GetColorClass()
        {
            string result = "";
            switch (Order.Status)
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
        #endregion



        #region Virtual Building Methods
        public virtual string GetContents()
        {
            return "";
        }

        public virtual string GetButtons()
        {
            return "";
        }


        #endregion
    }
}
