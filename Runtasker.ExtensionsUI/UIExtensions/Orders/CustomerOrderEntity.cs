using Runtasker.Logic.Entities;
using System.Text;
using Runtasker.Resources.UIExtensions.Orders;
using Extensions.Decimal;

namespace Runtasker.ExtensionsUI.UIExtensions.Orders
{
    //bs-calltoaction-primary - blue (executing)
    //bs-calltoaction-info  - light blue (valued waits for payment)
    //bs-calltoaction-default - white (proccessed)
    //bs-calltoaction-success -green (done and wait for last payment) 
    //bs-calltoaction-warning  -yellow
    //bs-calltoaction-danger  - red (have some mistake)
    public class CustomerOrderHtmlEntity : OrderEntityBase
    {
        #region Constructors
        public CustomerOrderHtmlEntity(Order order) : base(order)
        {
        }
        #endregion


        #region Button Renderer
        //For now there is only one action button for customer
        //We get it throw this class
        string GetButton()
        {
            return new CustomerOrderActionButtons(this).ToString();
        }
        #endregion

        #region Private Building Methods

        string GetDescription()
        {
            string uniqueId = $"collapse{Order.Id}";
            StringBuilder sb = new StringBuilder();
            sb.Append("<h4 class='panel-title'>")
            .Append($"<a data-toggle='collapse' data-parent='#accordion' href='#{uniqueId}'>")
            //.Append("<span class='glyphicon glyphicon-chevron-down'> </span>Description</a></h4>")
            .Append($"{OrderEntityRes.Description}</a></h4>")
            //was collapse in (in for expand)
            .Append($"<div id='{uniqueId}' class='panel-collapse collapse'>")
            .Append("<div class='panel-body'>")
            .Append($"{Order.Description}</div></div>");

            return sb.ToString();
        }

        string GetSubject()
        {
            if(Order.Subject != Subject.Other)
            {
                return Order.Subject.ToDescriptionString();
            }

            return Order.OtherSubject;
        }
        
        
        #endregion

        #region Overriden Methods
        public override string GetContents()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<div class='col-md-9 cta-contents'><h1 class='cta-title'>")
            .Append($"{OrderEntityRes.Order} №{Order.Id}")
            .Append("</h1><div class='cta-desc'>")
            .Append($"{GetDescription()}");
            if (Order.Sum != 0)
            {
                sb.Append($"<h4><span>{OrderEntityRes.Sum}: </span> {Order.Sum.ToMoney()}&#8381;</h4>");
            }
            sb.Append($"<h4>{OrderEntityRes.Subject}: {GetSubject()}</h4>")
            .Append($"<h4>{OrderEntityRes.WorkType}: {GetTypeOfWork()}</h4>")
            .Append("</div></div>");

            return sb.ToString();
        }

        public override string GetButtons()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='col-md-3 cta-button'>")
            //.Append($"<a href='#' class='btn btn-lg btn-block btn-{GetColorClass()}'>Go for It!</a>")
            .Append(GetButton())
            .Append("</div>");

            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetStarted())
            .Append(GetContents())
            .Append(GetButtons())
            .Append(GetClosed());

            return sb.ToString();
        }
        #endregion
    }
}
