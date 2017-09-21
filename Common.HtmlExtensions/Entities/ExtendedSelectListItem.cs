using Extensions.Dictionary;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace oksoft.Common.HtmlExtensions.Entities
{
    public class ExtendedSelectListItem : SelectListItem
    {
        #region Конструкторы
        public ExtendedSelectListItem()
        {
           
        }

        public ExtendedSelectListItem(Dictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Keys.ToList().ForEach(x => HtmlAttributes.Add(x, htmlAttributes[x]));
        }
        #endregion

        #region Поля
        Dictionary<string, object> _htmlAttributes;
        #endregion

        #region Свойства
        public Dictionary<string, object> HtmlAttributes
        {
            get
            {
                if(_htmlAttributes == null)
                {
                    _htmlAttributes = new Dictionary<string, object>();
                }
                return _htmlAttributes;
            }
        }
        #endregion

        public override string ToString()
        {
            return $"<option value={this.Value} {HtmlAttributes.GetPropertiesString()}>{this.Text}</option>";
        }
    }
}
