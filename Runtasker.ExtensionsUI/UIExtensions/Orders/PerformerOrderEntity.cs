using Runtasker.Logic.Entities;
using System.Text;

namespace Runtasker.ExtensionsUI.UIExtensions.Orders
{
    //TO DO make the base class
    public class PerformerOrderHtmlEntity : OrderEntityBase
    {
        #region Constructors
        public PerformerOrderHtmlEntity(Order order) : base(order)
        {
        }
        #endregion

        #region Building Methods
        //TODO ClaimForDecription Method
        string GetDescription()
        {
            

            string uniqueId = $"collapse{Order.Id}";

            StringBuilder sb = new StringBuilder();
            sb.Append("<h4 class='panel-title'>")
            .Append($"<a data-toggle='collapse' data-parent='#accordion' href='#{uniqueId}'>")
            //.Append("<span class='glyphicon glyphicon-chevron-down'> </span>Description</a></h4>")
            .Append("Description</a></h4>")
            .Append($"<div id='{uniqueId}' class='panel-collapse collapse'>")
            .Append("<div class='panel-body'>")
            .Append($"{Order.Description}</div></div>");
        
             return sb.ToString();
        }
        
        #endregion

        #region Private Help Methods
        string GetOrderStatusDescription()
        {
            string result = $"({Order.Status.ToDescriptionString()})";
            if (Order.Status == OrderStatus.HasError)
            {
                result = result + $"<p>[{Order.ErrorType.ToDescriptionString()}]</p>";
            }
            return result;    
        }

        string GetSubject()
        {
            if (Order.Subject != Subject.Other)
            {
                return Order.Subject.ToDescriptionString();
            }

            return Order.OtherSubject;
        }

        

        string GetReview()
        {
            if(Order.Status == OrderStatus.Appreciated)
            {
                string uniqueId = $"collapseReview{Order.Id}";

                StringBuilder sb = new StringBuilder();
                sb.Append("<h4 class='panel-title'>")
                .Append($"<a data-toggle='collapse' data-parent='#accordion' href='#{uniqueId}'>")
                //.Append("<span class='glyphicon glyphicon-chevron-down'> </span>Description</a></h4>")
                .Append("Оценка пользователя</a></h4>")
                .Append($"<div id='{uniqueId}' class='panel-collapse collapse'>")
                .Append("<div class='panel-body'>")
                .Append($"<h5>оценка: {Order.Rating} из 5</h5>")
                .Append($"комментарий: {Order.Comment}</div></div>");

                return sb.ToString();
            }

            return null;
        }
        #endregion

        #region Overridden Methods

        public override string GetContents()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class='col-md-9 cta-contents'><h1 class='cta-title'>")
            .Append($"Order №{Order.Id} {GetOrderStatusDescription()}")
            .Append("</h1><div class='cta-desc'>")
            .Append($"{GetDescription()}");
            if (Order.Sum != 0)
            {
                sb.Append($"<h4><span>Sum: </span> {Order.Sum}&#8381;</h4>");
            }
            sb.Append($"<h4>Предмет: {GetSubject()}</h4>")
            .Append($"<h4>Тип работы: {GetTypeOfWork()}</h4>")
            .Append(GetReview())
            .Append("</div></div>");

            return sb.ToString();
        }

        public override string GetButtons()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='col-md-3 cta-button'>")
            .Append(new PerformerActionButtons(this).ToString())
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
