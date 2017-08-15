using Runtasker.Logic.Entities;

namespace Runtasker.ExtensionsUI.UIExtensions.Orders
{
    //Base class for order html buttons renderers
    //both for customer and performer
    public class OrderHtmlButtonsBase
    {
        #region Constructors
        public OrderHtmlButtonsBase(OrderEntityBase orderEntity, int unreadMesCount)
        {
            OrderEntity = orderEntity;
            UnreadMessagesCount = unreadMesCount;
        }
        #endregion

        #region Поля
        string _btnClass;
        #endregion

        #region Защищенные Свойства
        protected OrderEntityBase OrderEntity { get; set; }

        protected Order Order
        {
            get { return OrderEntity.Order; }
        }

        protected int UnreadMessagesCount { get; set; }

        protected string BtnClass
        {
            get
            {
                if(string.IsNullOrEmpty(_btnClass))
                {
                    _btnClass = GetButtonClass();
                }
                return _btnClass;
            }
        }
        #endregion

        #region Закрытые методы
        private string GetButtonClass()
        {
            return $"btn btn-lg btn-block btn-{OrderEntity.GetColorClass()}";
        }
        #endregion
    }
}
