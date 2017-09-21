using Common.JavascriptValidation.Statics;
using Extensions.String;
using oksoft.Common.HtmlExtensions.Entities;
using oksoft.Common.HtmlExtensions.Enumerations;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using UI.Settings.Enumerations;

namespace oksoft.Common.HtmlExtensions.Extensions
{
    public static partial class HtmlExtensions
    {
        #region TextBox
        public static MvcHtmlString LabelAndTextBox(this HtmlHelper html, string propertyName, string value, string labelText, string placeholderValue = "", UIFormType formType = UIFormType.Ordinary, string type = "text")
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(placeholderValue))
            {
                placeholderValue = labelText;
            }

            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.TextBox(name: propertyName, value: value, htmlAttributes: new { id = PropertyNameHelper.GetIdForInput(propertyName), placeholder = placeholderValue, @class = "form-control", type = type })
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if (formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.TextBox(name: propertyName, value: value, htmlAttributes: new { id = PropertyNameHelper.GetIdForInput(propertyName), placeholder = placeholderValue, @class = "form-control", type = type }))

                .Append(html.Label(expression: propertyName, labelText: labelText, htmlAttributes: new { onclick = GetLabelJsFocucFunc(propertyName) }))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }


            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString LabelAndTextBox(this HtmlHelper html, string propertyName, string labelText, string placeholderValue = "", UIFormType formType = UIFormType.Ordinary, string type = "text")
        {
            StringBuilder sb = new StringBuilder();

            if(string.IsNullOrWhiteSpace(placeholderValue))
            {
                placeholderValue = labelText;
            }
            
            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.TextBox(name: propertyName, value: "", htmlAttributes: new { id = PropertyNameHelper.GetIdForInput(propertyName), placeholder = placeholderValue, @class = "form-control", type = type })
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if(formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.TextBox(name: propertyName, value: "", htmlAttributes: new { id = PropertyNameHelper.GetIdForInput(propertyName), placeholder = placeholderValue, @class = "form-control", type = type }))

                .Append(html.Label(expression: propertyName, labelText: labelText, htmlAttributes: new { onclick = GetLabelJsFocucFunc(propertyName) }))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }

            
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString LabelAndTextBoxWithTooltip(this HtmlHelper html, string propertyName, string labelText, string tooltipText, TooltipPlacementType placementType, string placeholderValue = "", UIFormType formType = UIFormType.Ordinary, string type = "text")
        {
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "data-toggle", "tooltip" },
                { "data-placement", placementType.ToString().ToLower()},
                { "title", tooltipText },
                { "id", PropertyNameHelper.GetIdForInput(propertyName) },
                { "placeholder", placeholderValue },
                { "class", "form-control" },
                { "type", type }
            };

            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(placeholderValue))
            {
                placeholderValue = labelText;
            }

            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.TextBox(name: propertyName, value: "", htmlAttributes: 
                    dict)
                    .ToString()
                    + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if (formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.TextBox(name: propertyName, value: "", htmlAttributes: dict))

                .Append(html.Label(expression: propertyName, labelText: labelText, htmlAttributes: new { onclick = GetLabelJsFocucFunc(propertyName) }))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }


            return MvcHtmlString.Create(sb.ToString());
        }

        #endregion

        #region TextArea
        public static MvcHtmlString LabelAndTextArea(this HtmlHelper html, string propertyName, string labelText, string placeholderValue = "", UIFormType formType = UIFormType.Ordinary)
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(placeholderValue))
            {
                placeholderValue = labelText;
            }

            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.TextArea(name: propertyName, value: "", htmlAttributes: new { id = PropertyNameHelper.GetIdForInput(propertyName), placeholder = placeholderValue, @class = "form-control" })
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if(formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.TextArea(name: propertyName, value: "", htmlAttributes: new { id = PropertyNameHelper.GetIdForInput(propertyName), placeholder = placeholderValue, cols = 3, @class = "form-control" }))
                
                .Append(html.Label(expression: propertyName, labelText: labelText, htmlAttributes: new { onclick = GetLabelJsFocucFunc(propertyName) }))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }
            

            return MvcHtmlString.Create(sb.ToString());
        }


        public static MvcHtmlString LabelAndTextAreaWithTooltip(this HtmlHelper html, string propertyName, string labelText, string tooltipText, TooltipPlacementType placementType, string placeholderValue = "", UIFormType formType = UIFormType.Ordinary)
        {
            StringBuilder sb = new StringBuilder();

            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "data-toggle", "tooltip" },
                { "data-placement", placementType.ToString().ToLower()},
                { "title", tooltipText },
                { "id", PropertyNameHelper.GetIdForInput(propertyName) },
                { "class", "form-control"},
                { "cols", 3 }
            };


            if (string.IsNullOrWhiteSpace(placeholderValue))
            {
                placeholderValue = labelText;
            }

            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.TextArea(name: propertyName, value: "", htmlAttributes: dict)
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if (formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.TextArea(name: propertyName, value: "", htmlAttributes: dict))

                .Append(html.Label(expression: propertyName, labelText: labelText, htmlAttributes: new { onclick = GetLabelJsFocucFunc(propertyName) }))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }


            return MvcHtmlString.Create(sb.ToString());
        }

        #endregion

        #region Password

        public static MvcHtmlString LabelAndPassword(this HtmlHelper html, string propertyName, string labelText, string placeholderValue = "••••••••", UIFormType formType = UIFormType.Ordinary)
        {
            return LabelAndTextBox(html, propertyName, labelText, placeholderValue, formType, type: "password");        
        }

        public static MvcHtmlString LabelAndPasswordWithTooltip(this HtmlHelper html, string propertyName, string labelText, string tooltipText, TooltipPlacementType placementType, string placeholderValue = "••••••••", UIFormType formType = UIFormType.Ordinary)
        {
            return LabelAndTextBoxWithTooltip(html, propertyName, labelText, tooltipText, placementType, placeholderValue, formType, type: "password");
        }

        #endregion

        #region DropDownList


        #region Обычный список
        public static MvcHtmlString LabelAndDropDownList(this HtmlHelper html, string propertyName, string labelText, IEnumerable<SelectListItem> selectItems, UIFormType formType = UIFormType.Ordinary)
        {
            StringBuilder sb = new StringBuilder();

            if(formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.DropDownList(name: propertyName, selectList: selectItems, htmlAttributes: new { id = PropertyNameHelper.GetIdForInput(propertyName), @class = "form-control" })
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if(formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.DropDownList(name: propertyName, selectList: selectItems, htmlAttributes: new { id = PropertyNameHelper.GetIdForInput(propertyName), @class = "form-control" }))

                .Append(html.Label(expression: propertyName, labelText: labelText))

                .Append(GetStrongErrorText(propertyName))
                
                .Append("</div>");
            }
            

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString LabelAndDropDownList(this HtmlHelper html, string propertyName, string labelText, IEnumerable<SelectListItem> selectItems, object htmlAttributes, UIFormType formType = UIFormType.Ordinary)
        {
            StringBuilder sb = new StringBuilder();

            Dictionary<string, object> dict = htmlAttributes.GetPropertiesDictionary();

            dict.Add("id", PropertyNameHelper.GetIdForInput(propertyName));
            dict.Add("class", "form-control");


            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.DropDownList(name: propertyName, selectList: selectItems, htmlAttributes: dict)
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if (formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.DropDownList(name: propertyName, selectList: selectItems, htmlAttributes: dict))

                .Append(html.Label(expression: propertyName, labelText: labelText))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }


            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString LabelAndDropDownList(this HtmlHelper html, string propertyName, string labelText, IEnumerable<SelectListItem> selectItems, Dictionary<string, object> htmlAttributes, UIFormType formType = UIFormType.Ordinary)
        {
            StringBuilder sb = new StringBuilder();

            Dictionary<string, object> dict = htmlAttributes;

            dict.Add("id", PropertyNameHelper.GetIdForInput(propertyName));
            dict.Add("class", "form-control");


            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.DropDownList(name: propertyName, selectList: selectItems, htmlAttributes: dict)
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if (formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.DropDownList(name: propertyName, selectList: selectItems, htmlAttributes: dict))

                .Append(html.Label(expression: propertyName, labelText: labelText))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }


            return MvcHtmlString.Create(sb.ToString());
        }

        #region С подсказочкой
        public static MvcHtmlString LabelAndDropDownListWithTooltip(this HtmlHelper html, string propertyName, string labelText, string tooltipText, TooltipPlacementType placementType, IEnumerable<SelectListItem> selectItems, UIFormType formType = UIFormType.Ordinary)
        {
            StringBuilder sb = new StringBuilder();

            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "data-toggle", "tooltip" },
                { "data-placement", placementType.ToString().ToLower()},
                { "title", tooltipText },
                { "id", PropertyNameHelper.GetIdForInput(propertyName) },
                { "class", "form-control"}
            };

            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.DropDownList(name: propertyName, selectList: selectItems, 
                    htmlAttributes: dict)
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if (formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.DropDownList(name: propertyName, selectList: selectItems, htmlAttributes: dict)
                .ToString())

                .Append(html.Label(expression: propertyName, labelText: labelText))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }


            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString LabelAndDropDownListWithTooltip(this HtmlHelper html, string propertyName, string labelText, string tooltipText, TooltipPlacementType placementType, IEnumerable<SelectListItem> selectItems, object htmlAttributes, UIFormType formType = UIFormType.Ordinary)
        {
            StringBuilder sb = new StringBuilder();

            Dictionary<string, object> dict = htmlAttributes.GetPropertiesDictionary();

            dict.Add("data-toggle", "tooltip");
            dict.Add("data-placement", placementType.ToString().ToLower());
            dict.Add("title", tooltipText);
            dict.Add("id", PropertyNameHelper.GetIdForInput(propertyName));
            dict.Add("class", "form-control");
            

            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.DropDownList(name: propertyName, selectList: selectItems,
                    htmlAttributes: dict)
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if (formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.DropDownList(name: propertyName, selectList: selectItems, htmlAttributes: dict)
                .ToString())

                .Append(html.Label(expression: propertyName, labelText: labelText))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }


            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString LabelAndDropDownListWithTooltip(this HtmlHelper html, string propertyName, string labelText, string tooltipText, TooltipPlacementType placementType, IEnumerable<SelectListItem> selectItems, Dictionary<string, object> htmlAttributes, UIFormType formType = UIFormType.Ordinary)
        {
            StringBuilder sb = new StringBuilder();

            Dictionary<string, object> dict = htmlAttributes;

            dict.Add("data-toggle", "tooltip");
            dict.Add("data-placement", placementType.ToString().ToLower());
            dict.Add("title", tooltipText);
            dict.Add("id", PropertyNameHelper.GetIdForInput(propertyName));
            dict.Add("class", "form-control");


            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.DropDownList(name: propertyName, selectList: selectItems,
                    htmlAttributes: dict)
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if (formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.DropDownList(name: propertyName, selectList: selectItems, htmlAttributes: dict)
                .ToString())

                .Append(html.Label(expression: propertyName, labelText: labelText))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }


            return MvcHtmlString.Create(sb.ToString());
        }
        
        #endregion

        #endregion

        #region Расширенный список
        public static MvcHtmlString LabelAndExtendedDropDownList(this HtmlHelper html, string propertyName, string labelText, IEnumerable<ExtendedSelectListItem> selectItems, UIFormType formType = UIFormType.Ordinary)
        {
            StringBuilder sb = new StringBuilder();


            Dictionary<string, object> dict = new Dictionary<string, object>()
            {
                {"id", PropertyNameHelper.GetIdForInput(propertyName)},
                {"class", "form-control"}
            };

            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.ExtendedDropdownList(propName: propertyName, selectList: selectItems, htmlAttributes: dict)
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if (formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.ExtendedDropdownList(propName: propertyName, selectList: selectItems, htmlAttributes: dict))

                .Append(html.Label(expression: propertyName, labelText: labelText))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }


            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString LabelAndExtendedDropDownList(this HtmlHelper html, string propertyName, string labelText, IEnumerable<ExtendedSelectListItem> selectItems, object htmlAttributes = null, UIFormType formType = UIFormType.Ordinary)
        {
            StringBuilder sb = new StringBuilder();

            Dictionary<string, object> dict = htmlAttributes.GetPropertiesDictionary();

            dict.Add("id", PropertyNameHelper.GetIdForInput(propertyName));
            dict.Add("class", "form-control");
            

            

            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.ExtendedDropdownList(propName: propertyName, selectList: selectItems, htmlAttributes: dict)
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if (formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.ExtendedDropdownList(propName: propertyName, selectList: selectItems, htmlAttributes: dict))

                .Append(html.Label(expression: propertyName, labelText: labelText))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }


            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #endregion

        #region DateTimePicker
        /// <summary>
        /// Пока что он такой же как и тесктбокс 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="propertyName"></param>
        /// <param name="labelText"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelAndDateTimePicker(this HtmlHelper html, string propertyName, string labelText, UIFormType formType = UIFormType.Ordinary)
        {

            StringBuilder sb = new StringBuilder();

            if(formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\" >");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append((html.TextBox(name: propertyName, value: "", htmlAttributes: new { id = PropertyNameHelper.GetIdForInput(propertyName), @class = "form-control" })
                    .ToString() + GetStrongErrorText(propertyName))
                    .WrapToHtmlTag("div", new { @class = "col-md-10 col-sm-10" }));

                sb.Append("</div>");
            }
            else if(formType == UIFormType.FloatLabel)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">")

                .Append(html.TextBox(name: propertyName, value: "", htmlAttributes: new { id = PropertyNameHelper.GetIdForInput(propertyName), @class = "form-control" }))

                .Append(html.Label(expression: propertyName, labelText: labelText, htmlAttributes: new { onclick = GetLabelJsFocucFunc(propertyName) }))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region FileInput
        
        public static MvcHtmlString LabelAndFileInput(this HtmlHelper html, string propertyName, string labelText, bool multiple = false, UIFormType formType = UIFormType.Ordinary)
        {

            StringBuilder sb = new StringBuilder();

            Dictionary<string, object> dict = new Dictionary<string, object>()
            {
                { "type", "file" },
                { "id", PropertyNameHelper.GetIdForInput(propertyName) }
            };

            if(multiple)
            {
                dict.Add("multiple", "");
            }

            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\" >");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append("<div class=\"col-md-10 col-sm-10\">");


                sb.Append(html.TextBox(name: propertyName, value: "", htmlAttributes: dict)
                    .ToString().WrapToHtmlTag("p").WrapToHtmlTag("p"));
                


                sb.Append(GetStrongErrorText(propertyName).ToString().WrapToHtmlTag("p"));

                sb.Append("</div>");

                sb.Append("</div>");
            }
            else if (formType == UIFormType.FloatLabel)
            {
                dict.Add("class", "form-control");

                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">");

                
                sb.Append(html.TextBox(name: propertyName, value: "", htmlAttributes: dict));
                

                sb.Append(html.Label(expression: propertyName, labelText: labelText))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }




            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString LabelAndFileInputWithTooltip(this HtmlHelper html, string propertyName, string labelText,  string tooltipText, TooltipPlacementType placementType, bool multiple = false, UIFormType formType = UIFormType.Ordinary)
        {

            StringBuilder sb = new StringBuilder();

            Dictionary<string, object> dict = new Dictionary<string, object>()
            {
                { "data-toggle", "tooltip" },
                { "data-placement", placementType.ToString().ToLower()},
                { "title", tooltipText },
                { "type", "file" },
                { "id", PropertyNameHelper.GetIdForInput(propertyName) }
            };

            if (multiple)
            {
                dict.Add("multiple", "");
            }

            if (formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\" >");

                sb.Append(html.Label(expression: propertyName, labelText: labelText).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2 col-sm-2" }));

                sb.Append("<div class=\"col-md-10 col-sm-10\">");


                sb.Append(html.TextBox(name: propertyName, value: "", htmlAttributes: dict)
                    .ToString().WrapToHtmlTag("p").WrapToHtmlTag("p"));



                sb.Append(GetStrongErrorText(propertyName).ToString().WrapToHtmlTag("p"));

                sb.Append("</div>");

                sb.Append("</div>");
            }
            else if (formType == UIFormType.FloatLabel)
            {
                dict.Add("class", "form-control");

                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group has-float-label\">");


                sb.Append(html.TextBox(name: propertyName, value: "", htmlAttributes: dict));


                sb.Append(html.Label(expression: propertyName, labelText: labelText))

                .Append(GetStrongErrorText(propertyName))

                .Append("</div>");
            }




            return MvcHtmlString.Create(sb.ToString());
        }

        #endregion


        public static MvcHtmlString LabelAndRadio(this HtmlHelper html, string propertyName, string labelText, IEnumerable<SelectListItem> selectItems, UIFormType formType = UIFormType.Ordinary)
        {

            StringBuilder sb = new StringBuilder();

            if(formType == UIFormType.Ordinary)
            {
                sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(propertyName)}\" class=\"form-group\">");

                sb.Append($"{labelText}".WrapToHtmlTag("strong").WrapToHtmlTag("div", new { @class = "col-md-2 col-sm-2 control-label" }));

                sb.Append("<div class=\"col-md-10 col-sm-10\">");

                foreach (SelectListItem item in selectItems)
                {
                    sb.Append($"<input class=\"form-control\" type=\"radio\" value=\"{item.Value}\" name=\"{propertyName}\">{item.Text.WrapToHtmlTag("strong")}"
                        .WrapToHtmlTag("label").WrapToHtmlTag("div", new { @class = "radio" }));
                }
                sb.Append(GetStrongErrorText(propertyName));

                sb.Append("</div>");
                sb.Append("</div>");
            }
            else
            {
                LabelAndRadio(html, propertyName, labelText, selectItems, UIFormType.Ordinary);
            }


            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Возвращает ошибочную строку для свойства
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString GetStrongErrorText(string propertyName, UIFormType formType = UIFormType.Ordinary)
        {
            StringBuilder sb = new StringBuilder();


            switch (formType)
            {
                case UIFormType.FloatLabel:
                    sb.Append(" ".WrapToHtmlTag("span", attributes: new { @class = "text-danger hidden", id = PropertyNameHelper.GetIdForTextError(propertyName) }));
                    break;
                    
                default:
                    sb.Append(" ".WrapToHtmlTag("span", attributes: new { @class = "text-danger hidden", id = PropertyNameHelper.GetIdForTextError(propertyName) }));
                    break;
            }


            return MvcHtmlString.Create(sb.ToString().WrapToHtmlTag("strong"));
        }

        public static string GetLabelJsFocucFunc(string propertyName)
        {
            return $"javascript: document.getElementById('{PropertyNameHelper.GetIdForInput(propertyName)}').focus()";
        }
    }
}


