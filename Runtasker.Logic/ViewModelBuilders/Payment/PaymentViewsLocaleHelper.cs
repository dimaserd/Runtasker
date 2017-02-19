using Runtasker.LocaleBuilders.Models;
using Runtasker.LocaleBuilders.Views.Payment;

namespace Runtasker.Logic.ViewModelBuilders.Payment
{
    public class PaymentViewsLocaleHelper : ViewModelLocaleHelperBase
    {
        #region Constructors
        public PaymentViewsLocaleHelper() : base()
        {
            Construct();
        }

        void Construct()
        {
            ModelBuilder = new PaymentViewModelBuilder();
        }
        #endregion

        #region Properties
        PaymentViewModelBuilder ModelBuilder { get; set; }
        #endregion

        #region Public Methods
        public LocaleViewModel Robokassa(decimal amount, decimal withdrawAmount)
        {
            return ModelBuilder.Robokassa(amount, withdrawAmount, HtmlSigns.Rouble);
        }

        public LocaleViewModel Yandex()
        {
            return ModelBuilder.Yandex();
        }

        public LocaleViewModel Index()
        {
            return ModelBuilder.Index();
        }
        #endregion
    }
}
