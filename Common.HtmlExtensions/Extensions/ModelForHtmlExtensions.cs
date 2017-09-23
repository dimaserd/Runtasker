using Common.JavascriptValidation.Statics;
using Extensions.Attributes;
using Extensions.Reflection;
using Extensions.String;
using oksoft.Common.HtmlExtensions.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using UI.Settings;
using UI.Settings.Enumerations;

namespace oksoft.Common.HtmlExtensions.Extensions
{
    public static partial class HtmlExtensions
    {
        
        #region Текстбоксы


        public static MvcHtmlString LabelAndTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html,
           Expression<Func<TModel, TValue>> expression)
        {

            MemberExpression member = expression.Body as MemberExpression;

            string propName = GetNameFromMemberExpression(member);

            string displayName = GetDisplayName(member);

            string placeholderValue = GetPlaceholderValue(member);

            return LabelAndTextBox(html: html, propertyName: propName, labelText: displayName, placeholderValue: placeholderValue, formType: AtroposSettings.FormType);
        }

        public static MvcHtmlString LabelAndTextBoxWithTooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
           Expression<Func<TModel, TValue>> expression)
        {

            MemberExpression member = expression.Body as MemberExpression;

            string propName = GetNameFromMemberExpression(member);

            string displayName = GetDisplayName(member);

            string placeholderValue = GetPlaceholderValue(member);

            string tooltipValue = GetTooltipValue(member);

            return LabelAndTextBoxWithTooltip(html: html, propertyName: propName, labelText: displayName, tooltipText: tooltipValue, 
                placementType: Enumerations.TooltipPlacementType.Top, placeholderValue: placeholderValue, formType: AtroposSettings.FormType);
        }

        #region Пароли
        public static MvcHtmlString LabelAndPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html,
           Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {

            MemberExpression member = expression.Body as MemberExpression;
            
            string propName = GetNameFromMemberExpression(member);

            string displayName = GetDisplayName(member);

            return LabelAndPassword(html: html, propertyName: propName, labelText: displayName, formType: AtroposSettings.FormType);
        }

        public static MvcHtmlString LabelAndPasswordWithTooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
           Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {

            MemberExpression member = expression.Body as MemberExpression;

            string propName = GetNameFromMemberExpression(member);

            string displayName = GetDisplayName(member);

            string tooltipText = GetTooltipValue(member);

            return html.LabelAndPasswordWithTooltip(propertyName: propName, labelText: displayName, tooltipText: tooltipText, placementType: TooltipPlacementType.Top,  formType: AtroposSettings.FormType);
        }

        #endregion

        #region Файлы
        public static MvcHtmlString LabelAndFileInputFor<TModel, TValue>(this HtmlHelper<TModel> html,
           Expression<Func<TModel, TValue>> expression, bool multiple = false)
        {
            MemberExpression member = expression.Body as MemberExpression;

            string propName = GetNameFromMemberExpression(member);

            string displayName = GetDisplayName(member);

            return html.LabelAndFileInput(propertyName: propName, labelText: displayName, multiple: multiple, formType: AtroposSettings.FormType);
        }


        public static MvcHtmlString LabelAndFileInputWithTooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
           Expression<Func<TModel, TValue>> expression, bool multiple = false)
        {
            //this HtmlHelper<TModel> html, string propertyName, string labelText, string tooltipText, TooltipPlacementType placementType, bool multiple = false, UIFormType formType = UIFormType.Ordinary)
            MemberExpression member = expression.Body as MemberExpression;

            string propName = GetNameFromMemberExpression(member);

            string displayName = GetDisplayName(member);

            string placeholderValue = GetPlaceholderValue(member);

            string tooltipValue = GetTooltipValue(member);

            return html.LabelAndFileInputWithTooltip(propertyName: propName, labelText: displayName, tooltipText: tooltipValue,
                multiple: multiple, placementType: TooltipPlacementType.Top, formType: AtroposSettings.FormType);
        }

        #endregion

        #endregion

        #region Выпадающие списки
        public static MvcHtmlString LabelAndDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
        {
            MemberExpression member = expression.Body as MemberExpression;

            string propName = GetNameFromMemberExpression(member);

            string displayName = GetDisplayName(member);

            return LabelAndDropDownList(html: html, propertyName: propName, labelText: displayName, selectItems: selectList, formType: AtroposSettings.FormType);
        }

        public static MvcHtmlString LabelAndDropDownListWithTooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
        {
            MemberExpression member = expression.Body as MemberExpression;

            string propName = GetNameFromMemberExpression(member);

            string displayName = GetDisplayName(member);

            string tooltipValue = GetTooltipValue(member);


            return html.LabelAndDropDownListWithTooltip(propertyName: propName, labelText: displayName, 
                tooltipText: tooltipValue, placementType: TooltipPlacementType.Top, selectItems: selectList, formType: AtroposSettings.FormType);
        }

        #endregion

        #region ТекстАрии

        public static MvcHtmlString LabelAndTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            MemberExpression member = expression.Body as MemberExpression;

            string propName = GetNameFromMemberExpression(member);

            string displayName = GetDisplayName(member);

            string placeholderValue = GetPlaceholderValue(member);

            //string propertyName, string labelText, string placeholderValue = "", UIFormType formType = UIFormType.Ordinary)
            return html.LabelAndTextArea(propertyName: propName, labelText: displayName, placeholderValue: placeholderValue, formType: AtroposSettings.FormType);
        }

        public static MvcHtmlString LabelAndTextAreaWithTooltipFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            MemberExpression member = expression.Body as MemberExpression;

            string propName = GetNameFromMemberExpression(member);

            string displayName = GetDisplayName(member);

            string placeholderValue = GetPlaceholderValue(member);

            string tooltipValue = GetTooltipValue(member);

            return html.LabelAndTextAreaWithTooltip(propertyName: propName, labelText: displayName,tooltipText: tooltipValue, placementType: TooltipPlacementType.Top, placeholderValue: placeholderValue, formType: AtroposSettings.FormType);
        }
        #endregion


        #region Вспомогательные методы

        private static string GetPlaceholderValue(MemberExpression memberExpression)
        {
            PlaceholderAttribute attr = memberExpression.Member.GetAttribute<PlaceholderAttribute>(isRequired: false);

            return (attr != null) ? attr.Text : "";
        }

        private static string GetTooltipValue(MemberExpression memberExpression)
        {
            TooltipAttribute attr = memberExpression.Member.GetAttribute<TooltipAttribute>(isRequired: true);

            return attr.TooltipText;
        }

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

        private static string GetDisplayName(MemberExpression memberExpression)
        {
            return memberExpression.Member.GetAttribute<DisplayAttribute>().GetName();
        }

       

        private static string GetDisplayName(PropertyInfo prop)
        {
            return prop.GetAttribute<DisplayAttribute>().GetName();
        }

        private static string GetNameFromMemberExpression(MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }


        public static string GetPlaceholderValue<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            MemberExpression body = expression.Body as MemberExpression;

            PlaceholderAttribute attr = ReflectionExtensionMethods.GetAttribute<PlaceholderAttribute>(body.Member, false);

            return (attr != null) ? attr.Text : string.Empty;
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