using System.Web.Mvc;

namespace oksoft.Common.HtmlExtensions.Entities
{
    public class FontAwesomeModel
    {
        #region Конструкторы
        public FontAwesomeModel(string afterFaClass)
        {
            AfterFaClass = afterFaClass;
        }

        public FontAwesomeModel(string afterFaClass, string id)
        {
            AfterFaClass = afterFaClass;
            _attributesString = $" id={id}";
        }
        #endregion


        #region Поля
        

        string _attributesString = string.Empty;
        #endregion

        #region Свойства
        public string AfterFaClass { get; set; }

        /// <summary>
        /// Возвращает название класса с префиксом fa
        /// </summary>
        public string WholeFaClass
        {
            get
            {
                return $"fa-{AfterFaClass}";
            }
        }
        #endregion

        #region Методы 
        public override string ToString()
        {
            return $"<i{_attributesString} class=\"fa fa-{AfterFaClass}\" aria-hidden=\"true\"></i>";
        }

        public MvcHtmlString ToHtml()
        {
            return MvcHtmlString.Create(this.ToString());
        }

        #endregion
    }

    

    
}
