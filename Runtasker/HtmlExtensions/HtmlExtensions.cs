using Common.JavascriptValidation.Statics;
using Extensions.Attributes;
using Extensions.Reflection;
using Extensions.String;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Runtasker.HtmlExtensions
{
    public static partial class HtmlExtensions
    {

        #region Лейбл с чем-то там
        
        public static MvcHtmlString LabelAndPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html,
           Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {

            var member = expression.Body as MemberExpression;
            var prop = member.Member as PropertyInfo;


            StringBuilder sb = new StringBuilder();
            sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(GetNameFromProperty(html, expression))}\" class='form-group {htmlAttributes.GetPropertyValue("class")}' ");
            if (htmlAttributes != null)
            {
                sb.Append(htmlAttributes.RenderAttributesKeyValuePairExcept("class"));
            }
            sb.Append(" >");

            sb.Append(html.LabelFor(expression).ToString())
            .Append(html.EditorFor(expression, new { id = expression.Type.Name.ToId(), htmlAttributes = new { @class = "form-control", type = "password" } }))
            .Append(html.ValidationMessageFor(expression, "", new { @class = "text-danger" }))

            .Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        #endregion
        
        public static MvcHtmlString LabelAndTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null, bool showPopover = false)
        {

            var member = expression.Body as MemberExpression;
            var prop = member.Member as PropertyInfo;


            StringBuilder sb = new StringBuilder();
            sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(GetNameFromProperty(html, expression))}\" class='form-group {htmlAttributes.GetPropertyValue("class")}' ");
            if (htmlAttributes != null)
            {
                sb.Append(htmlAttributes.RenderAttributesKeyValuePairExcept("class"));
            }
            sb.Append(" >");

            
            sb.Append(html.LabelFor(expression).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2" }))

            .Append("<div class='col-md-10'>");

            sb.Append("<div class=\"input-group\">");

            sb.Append(html.TextBoxFor(expression, htmlAttributes: new
            {
                id = PropertyNameHelper.GetIdForInput(GetNameFromProperty(html, expression)),
                @class = "form-control",
                placeholder = GetPlaceholderValue(html, expression)
            }));

            if (showPopover)
            {
                sb.Append(html.PopoverInfoFor(expression).ToString().WrapToHtmlTag("div", new { @class = "input-group-btn" }));
            }

            sb.Append(GetStrongErrorText(html, expression));
           

            sb.Append("</div>")
            .Append("</div>")
            .Append("</div>");


            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString LabelAndDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes = null, bool showPopover = false)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(GetNameFromProperty(html, expression))}\" class=\"form-group\">");

            sb.Append(html.LabelFor(expression).ToString().WrapToHtmlTag("div", new { @class = "col-md-2 col-sm-2" }));

            sb.Append("<div class=\"col-md-10 col-sm-10\">");

            sb.Append("<div class=\"input-group\">");

            //соглашение по именованию для полей для создания джаваскрипт функций
            sb.Append(html.DropDownListFor(expression, selectList, 
                new
                {
                    id = PropertyNameHelper.GetIdForInput(GetNameFromProperty(html, expression)),
                    @class = "form-control"
                }));

            if(showPopover)
            {
                sb.Append(html.PopoverInfoFor(expression).ToString().WrapToHtmlTag("div", new { @class = "input-group-btn"}));
            }

            sb.Append(html.GetStrongErrorText(expression));

            sb.Append("</div>");

            sb.Append("</div>");


            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }


        public static MvcHtmlString LabelAndTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(GetNameFromProperty(html, expression))}\" class=\"form-group\">");

            sb.Append(html.LabelFor(expression).ToString().WrapToHtmlTag("div", new { @class = "col-md-2 col-sm-2" }));

            sb.Append("<div class=\"col-md-10 col-sm-10\">");

            
            //соглашение по именованию для полей для создания джаваскрипт функций
            sb.Append(html.TextAreaFor(expression,
                new
                {
                    id = PropertyNameHelper.GetIdForInput(GetNameFromProperty(html, expression)),
                    @class = "form-control",
                    placeholder = GetPlaceholderValue(html, expression),
                }));

            

            sb.Append(html.GetStrongErrorText(expression));

            
            sb.Append("</div>");


            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString LabelAndDateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null, bool showPopover = false)
        {
            var member = expression.Body as MemberExpression;
            var prop = member.Member as PropertyInfo;


            StringBuilder sb = new StringBuilder();
            sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(GetNameFromProperty(html, expression))}\" class='form-group {htmlAttributes.GetPropertyValue("class")}' ");
            if (htmlAttributes != null)
            {
                sb.Append(htmlAttributes.RenderAttributesKeyValuePairExcept("class"));
            }
            sb.Append(" >");


            sb.Append(html.LabelFor(expression).ToString().WrapToHtmlTag("div", new { @class = "control-label col-md-2" }))

            .Append("<div class='col-md-10'>");

            sb.Append("<div class=\"input-group\">");

            sb.Append(html.TextBoxFor(expression, htmlAttributes: new
            {
                id = PropertyNameHelper.GetIdForInput(GetNameFromProperty(html, expression)),
                @class = "form-control",
                @Value = ""
            }));

            if (showPopover)
            {
                sb.Append(html.PopoverInfoFor(expression).ToString().WrapToHtmlTag("div", new { @class = "input-group-btn" }));
            }

            sb.Append(GetStrongErrorText(html, expression));


            sb.Append("</div>")
            .Append("</div>")
            .Append("</div>");


            return MvcHtmlString.Create(sb.ToString());
        }

        #region  Новые разработки

        #region Основные методы (в жирном)


        public static MvcHtmlString LabelWithTextBoxAndStrongErrorTextFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(GetNameFromProperty(html, expression))}\" class=\"form-group\">");

            sb.Append(GetStrongLabelFor(html, expression, new { @class = "col-md-2 col-sm-2" }));

            //соглашение по именованию для полей для создания джаваскрипт функций
            sb.Append(html.GetTextBoxWithStrongErrorTextFor(expression, new { @class = "col-md-10 col-sm-10",
                id = PropertyNameHelper.GetIdForInput(GetNameFromProperty(html, expression)) }));

            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }


        public static MvcHtmlString LabelWithTextAreaAndStrongErrorTextFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(GetNameFromProperty(html, expression))}\" class=\"form-group\">");

            sb.Append(GetStrongLabelFor(html, expression, new { @class = "col-md-2 col-sm-2" }));

            //соглашение по именованию для полей для создания джаваскрипт функций
            sb.Append(html.GetTextAreaWithStrongErrorTextFor(expression, new { @class = "col-md-10 col-sm-10", id = PropertyNameHelper.GetIdForInput(GetNameFromProperty(html, expression)) }));

            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString LabelWithCheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<div id=\"{PropertyNameHelper.GetIdForForm(GetNameFromProperty(html, expression))}\" class=\"form-group\">");

            //.Append("<div class=\"col-sm-2 col-md-2\">")
            //.Append("</div>");


            sb.Append("<div class=\"col-sm-12 col-md-12\">");

            sb.Append($"<input type=\"checkbox\"  id=\"{PropertyNameHelper.GetIdForInput(GetNameFromProperty(html, expression))}\" class=\"form-control\" value=\"false\" />");

            sb.Append($" { GetDisplayValue(html, expression).WrapToHtmlTag("strong")}");

            sb.Append("</div>");

            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion


        #region Перевод ошибок в html
       

        
        #endregion

        /// <summary>
        /// Возвращает строку html строку 
        /// (в html Аттрибутах нужно внести бутстрап класс разделения типо col-md-2)
        /// </summary>
        /// <param name="html"></param>
        /// <param name="label"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString GetStrongLabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,  object htmlAttributes = null)
        {
            string text = GetDisplayValue(html, expression);

            StringBuilder sb = new StringBuilder();

            sb.Append($"<label class=\"control-label {htmlAttributes.GetPropertyValue("class")}\" ");

            sb.Append($"{htmlAttributes.RenderAttributesKeyValuePairExcept("class")} >")

            .Append(text.WrapToHtmlTag("strong"));

            sb.Append("</label>");

            return MvcHtmlString.Create(sb.ToString());
        }


        #region Элементы управления

        /// <summary>
        /// Возвращает Редактор для свойства модели и текстовое поле с ошибкой
        /// (в html Аттрибутах нужно внести бутстрап класс разделения типо col-md-2)
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString GetTextBoxWithStrongErrorTextFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<div class=\"{htmlAttributes.GetPropertyValue("class")}\">");

            

            sb.Append(html.TextBoxFor(expression, new
            {
                id = PropertyNameHelper.GetIdForInput(GetNameFromProperty(html, expression)),
                placeholder = GetPlaceholderValue(html, expression),
                @class = "form-control",
                @Value = ""
            }))
            .Append(html.GetStrongErrorText(expression));

            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Возвращает Редактор для свойства модели и текстовое поле с ошибкой
        /// (в html Аттрибутах нужно внести бутстрап класс разделения типо col-md-2)
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString GetTextAreaWithStrongErrorTextFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<div class=\"{htmlAttributes.GetPropertyValue("class")}\">");



            sb.Append(html.TextAreaFor(expression, new
            {
                id = PropertyNameHelper.GetIdForInput(GetNameFromProperty(html, expression)),
                placeholder = GetPlaceholderValue(html, expression),
                @class = "form-control",
                @Value = ""
            }))
            .Append(html.GetStrongErrorText(expression));

            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion
        /// <summary>
        /// Возвращает ошибочную строку для свойства
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString GetStrongErrorText<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" ".WrapToHtmlTag("span", attributes: new { @class = "text-danger hidden", id = PropertyNameHelper.GetIdForTextError(GetNameFromProperty(html, expression)) }));
                

            return MvcHtmlString.Create(sb.ToString().WrapToHtmlTag("strong"));
        }

        #endregion


        #region Вспомогательные методы
        /// <summary>
        /// Возвращает название свойства
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetNameFromProperty<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
                return null;

            return memberExpression.Member.Name;
        }


        public static string GetPlaceholderValue<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            MemberExpression body = expression.Body as MemberExpression;

            PlaceholderAttribute attr = ReflectionExtensionMethods.GetAttribute<PlaceholderAttribute>(body.Member, false);

            return (attr != null)? attr.Text : string.Empty;
        }



        public static string GetDisplayValue<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            MemberExpression body = expression.Body as MemberExpression;

            DisplayAttribute attr = ReflectionExtensionMethods.GetAttribute<DisplayAttribute>(body.Member);

            return attr.GetName();
        }
        #endregion
    }
}