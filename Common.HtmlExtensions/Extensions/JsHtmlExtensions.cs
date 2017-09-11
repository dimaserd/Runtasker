using Common.JavascriptValidation.Entities;
using Common.JavascriptValidation.Enumerations;
using oksoft.Common.HtmlExtensions.Enumerations;
using System.Collections.Generic;
using System.Web.Mvc;
using UI.Settings;

namespace oksoft.Common.HtmlExtensions.Extensions
{
    public static partial class HtmlExtensions
    {
        public static MvcHtmlString LabelAndEditorForJsProp(this HtmlHelper html, JsPropertyWithLabel jsProp)
        {
            switch (jsProp.HtmlDataType)
            {
                case JsHtmlDataType.Text:
                    if (jsProp.InputType == JsInputDataType.Password)
                    {
                        return LabelAndPassword(html, jsProp);
                    }

                    return LabelAndTextBox(html, jsProp);

            }

            return null;
        }

        #region TextBox
        public static MvcHtmlString LabelAndTextBox(this HtmlHelper html, JsPropertyWithLabel jsProp)
        {
            string type = (jsProp.HtmlDataType == JsHtmlDataType.Email) ? "email" : "text";

            return LabelAndTextBox(html, propertyName: jsProp.PropertyName, labelText: jsProp.LabelText, placeholderValue: jsProp.PlaceholderValue, formType: AtroposSettings.FormType, type: type);
        }

        public static MvcHtmlString LabelAndTextBoxWithTooltip(this HtmlHelper html, JsPropertyWithLabel jsProp, string tooltipText, TooltipPlacementType placementType)
        {
            return LabelAndTextBoxWithTooltip(html: html, propertyName: jsProp.PropertyName, labelText: jsProp.LabelText,
                tooltipText: tooltipText, placementType: placementType,
                placeholderValue: jsProp.PlaceholderValue, formType: AtroposSettings.FormType);
        }
        #endregion

        #region Password
        public static MvcHtmlString LabelAndPassword(this HtmlHelper html, JsPropertyWithLabel jsProp)
        {
            return LabelAndPassword(html, propertyName: jsProp.PropertyName, labelText: jsProp.LabelText, formType: AtroposSettings.FormType);
        }

        public static MvcHtmlString LabelAndPasswordWithTooltip(this HtmlHelper html, JsPropertyWithLabel jsProp, string tooltipText, TooltipPlacementType placementType)
        {
            return LabelAndPasswordWithTooltip(html: html, propertyName: jsProp.PropertyName, labelText: jsProp.LabelText,
                tooltipText: tooltipText, placementType: placementType, 
                placeholderValue: jsProp.PlaceholderValue, formType: AtroposSettings.FormType);
        }
        #endregion

        public static MvcHtmlString LabelAndTextArea(this HtmlHelper html, JsPropertyWithLabel jsProp)
        {
            return LabelAndTextArea(html, propertyName: jsProp.PropertyName, labelText: jsProp.LabelText, placeholderValue: jsProp.PlaceholderValue, formType: AtroposSettings.FormType);
        }

        public static MvcHtmlString LabelAndDateTimePicker(this HtmlHelper html, JsPropertyWithLabel jsProp)
        {
            return LabelAndDateTimePicker(html, propertyName: jsProp.PropertyName, labelText: jsProp.LabelText, formType: AtroposSettings.FormType);
        }

        public static MvcHtmlString LabelAndFileInput(this HtmlHelper html, JsPropertyWithLabel jsProp, bool multiple = false)
        {
            return LabelAndFileInput(html, propertyName: jsProp.PropertyName, labelText: jsProp.LabelText, multiple: multiple, formType: AtroposSettings.FormType);
        }
        
        public static MvcHtmlString LabelAndDropDownList(this HtmlHelper html, JsPropertyWithLabel jsProp, IEnumerable<SelectListItem> selectItems)
        {
            return LabelAndDropDownList(html, propertyName: jsProp.PropertyName, labelText: jsProp.LabelText, selectItems: selectItems, formType: AtroposSettings.FormType);
        }

        public static MvcHtmlString LabelAndRadio(this HtmlHelper html, JsPropertyWithLabel jsProp, IEnumerable<SelectListItem> selectItems)
        {
            return LabelAndRadio(html, propertyName: jsProp.PropertyName, labelText: jsProp.LabelText, selectItems: selectItems);
        }
    }
}