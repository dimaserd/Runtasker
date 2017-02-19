using Runtasker.Logic.Entities;

namespace Runtasker.ExtensionsUI.UIExtensions.Orders
{
    //Base class for order html buttons renderers
    //both for customer and performer
    public class OrderHtmlButtonsBase
    {
        #region Constructors
        public OrderHtmlButtonsBase(OrderEntityBase orderEntity)
        {
            OrderEntity = orderEntity;
        }
        #endregion

        #region Properties
        protected OrderEntityBase OrderEntity { get; set; }
        protected Order Order
        {
            get { return OrderEntity.Order; }
        }


        #endregion

        #region Protected Methods
        protected string GetButtonClass()
        {
            return $"btn btn-lg btn-block btn-{OrderEntity.GetColorClass()}";
        }
        #endregion
    }
}
