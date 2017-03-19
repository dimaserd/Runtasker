using Runtaker.LocaleBuiders.Enumerations;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Views.Home.KnowPrice;
using Runtasker.Resources.Views.Orders.Create;

namespace Runtaker.LocaleBuiders.Views.Order
{
    public class OrderViewModelBuilder : UICultureSwitcher
    {
        public LocaleViewModel CreateOrderView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", Create.Title);
            result.Add("HomeNav", Create.HomeNav);
            result.Add("MyOrdersNav", Create.MyOrdersNav);
            result.Add("CreateNav", Create.CreateNav);
            result.Add("Header", Create.Header);
            result.Add("MiniHeader", Create.MiniHeader);
            result.Add("DescriptionPlaceholder", Create.DescriptionPlaceholder);
            result.Add("CreateOrder", Create.CreateOrder);
            result.Add("FileUploading", Create.FileUploading);
            result.Add("BackToList", Create.BackToList);
            //Стащено с другого файла ресурсов
            result.Add("NameError", KnowPriceRes.NameError);
            result.Add("EmailError", KnowPriceRes.EmailError);
            result.Add("DescriptionError", KnowPriceRes.DescriptionError);
            result.Add("OtherSubjectError", KnowPriceRes.OtherSubjectError);

            switch (UILang)
            {
                case Lang.Russian:
                    
                    
                    break;

                default:
                    break;
            }

            return result;
        }
    }
}
