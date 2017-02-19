
using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.Text;

namespace Runtasker.ExtensionsUI.UIExtensions.Orders
{
    //Not finished and nowhere implemented
    public class SortOrderButtons
    {
        #region Constructors
        public SortOrderButtons(IEnumerable<Order> orders)
        {
            Orders = orders;
        }

        void Construct()
        {
            foreach(Order order in Orders)
            {

            }
        }
        #endregion

        #region Properties
        IEnumerable<Order> Orders { get; set; }
        #endregion

        #region Help Methods
        string GetButton(Order order)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<button type='button' class='btn btn-{order.Status.GetColorClass()} btn-filter' ")
            .Append($"data-target='{order.Status}'>")
            .Append($"{order.Status}")
            .Append("</button>");

            return sb.ToString();
        }
        #endregion

        #region Overriden Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class='row'><div class='pull-right'><div class='btn-group'>");

            return sb.ToString();
        }
        #endregion
    }
}
