using Extensions.Reflection;
using Extensions.String;
using HtmlExtensions.StaticRenderers;
using Runtasker.LocaleBuilders.Enumerations;
using Runtasker.Logic.Attributes;
using Runtasker.Logic.Entities;
using Runtasker.Settings.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Optimization;

namespace Runtasker.HtmlExtensions
{
    public static partial class HtmlExtensions
    {
        #region Стили и скрипты
        
        #region DateTime styles scripts
        public static MvcHtmlString GetDateTimeScriptsAndStyles(this HtmlHelper html)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Scripts.Render("~/DateTimePicker"));
            sb.Append(Styles.Render("~/DateTimePickerCss"));
            
            
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region Date Scripts
        public static MvcHtmlString GetParseDateJSFunction(this HtmlHelper html)
        {
            StringBuilder sb = new StringBuilder();
            Lang lang = Runtasker.LocaleBuilders.Statics.LanguageStatic.Language;
            switch (lang)
            {
                case Lang.Russian:
                    sb.Append("function parseDate(str){")
                    .Append("var dmy = str.split('.');\n")
                    .Append("var date = new Date(20 + dmy[2], dmy[1] - 1, dmy[0])\n")
                    .Append("return date;}\n");
                    break;

                default:
                    sb.Append("function parseDate(str){")
                    .Append("var dmy = str.split('/');\n")
                    .Append("var date = new Date(dmy[2], dmy[0] - 1, dmy[1])\n")
                    .Append("return date;}\n");
                    break;
                    

            }
            

            return MvcHtmlString.Create(sb.ToString());

        }
        #endregion


        #endregion

        #region UI Elements

        #region Vk and Other elements
        public static MvcHtmlString GetVKContactChat(this HtmlHelper html)
        {
            if(Settings.Settings.AppSetting == ApplicationSettingType.Debug)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            string haveQuestions = $"{Resources.Views.Shared.NewLayout.NewLayoutRes.HaveQuestions}?";
            sb.Append("<div id=\"vk_community_messages\" class=\"hidden-xs\"></div>")
            .Append("<script type=\"text/javascript\">")
            .Append("VK.Widgets.CommunityMessages(\"vk_community_messages\", 137750954, {tooltipButtonText: \"" + haveQuestions + "\"});")
            .Append("</script>");

            return MvcHtmlString.Create(sb.ToString());
        }

        
        #endregion

        #region CustomFile Input
        public static MvcHtmlString RenderCustomFileInput(this HtmlHelper html)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Scripts.Render("~/CustomFileInputJS"));
            sb.Append(Styles.Render("~/CustomFileInputCSS"));

            //body of script

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region ValidationSummary
        public static MvcHtmlString ValidationErrorsToAlerts(this HtmlHelper html)
        {
            
            List<ModelError> ModelErrors = new List<ModelError>(); 
            foreach (ModelState modelState in html.ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    ModelErrors.Add(error);
                }
            }

            
            StringBuilder sb = new StringBuilder();
            foreach(ModelError error in ModelErrors)
            {
                sb.Append($"<div class='alert alert-danger'><i class='fa fa-frown-o'></i>")
                .Append($"<strong>{error.ErrorMessage}</strong></div>");
            }
            

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString FirstValidationErrorToAlert(this HtmlHelper html)
        {
            List<ModelError> ModelErrors = new List<ModelError>();
            foreach (ModelState modelState in html.ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    ModelErrors.Add(error);
                }
            }

            if(ModelErrors.Count == 0)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append($"<div class='alert alert-danger'><i class='fa fa-frown-o'></i>")
            .Append($"<strong>{ModelErrors[0].ErrorMessage}</strong></div>");

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region With Label
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (string.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            TagBuilder tag = new TagBuilder("label");
            tag.MergeAttributes(htmlAttributes);
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            TagBuilder span = new TagBuilder("span");
            span.SetInnerText(labelText);

            // assign <span> to <label> inner html
            tag.InnerHtml = span.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString LabelAndEditorFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            
            var member = expression.Body as MemberExpression;
            var prop = member.Member as PropertyInfo;

            string popoverInfo = GetPopoverInfoFromProperty(prop);

            
            StringBuilder sb = new StringBuilder();
            sb.Append($"<div class='form-group {htmlAttributes.GetPropertyValue("class")}' ");
            if(htmlAttributes != null)
            {
                sb.Append(htmlAttributes.RenderAttributesKeyValuePairExcept("class"));
            }
            sb.Append(" >");

            sb.Append(html.LabelFor(expression, htmlAttributes: new { @class = "control-label col-md-2" }).ToString())
            .Append("<div class='col-md-5'>");
            if(popoverInfo != null)
            {
                sb.Append("<div class='input-group'>")

                .Append(html.EditorFor(expression, new { id = expression.Type.Name.ToId(), htmlAttributes = new { @class = "form-control" } }))
                .Append("<div class='input-group-btn'>")
                .Append("<button type='button' class='btn btn-default popoverBtn' ")
                .Append("data-container='body' data-toggle='popover' ")
                .Append("data-placement='bottom' ")
                .Append($"data-content=\"{popoverInfo}\">")
                .Append("<span class='glyphicon glyphicon-info-sign'></span>")
                .Append("</button></div>")
                .Append("</div>")
                .Append(html.ValidationMessageFor(expression, "", new { @class = "text-danger" }));//here
            }
            else
            {
                sb.Append(html.EditorFor(expression, new { id = expression.Type.Name.ToId(), htmlAttributes = new { @class = "form-control" } }))
                .Append(html.ValidationMessageFor(expression, "", new { @class = "text-danger" }));
            }
            
            sb.Append("</div>")
            .Append("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }


        public static MvcHtmlString FormWithLabelAndEditorFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {

            var member = expression.Body as MemberExpression;
            var prop = member.Member as PropertyInfo;

            string popoverInfo = GetPopoverInfoFromProperty(prop);

            string formId = $"{prop.Name}Form";
            string errorId = $"{prop.Name}Error";
            string editorId = $"{prop.Name}Field";

            StringBuilder sb = new StringBuilder();
            sb.Append($"<div id='{formId}' class='form-group {htmlAttributes.GetPropertyValue("class")}' "); //0
            if (htmlAttributes != null)
            {
                sb.Append(htmlAttributes.RenderAttributesKeyValuePairExcept("class", "id"));
            }
            sb.Append(" >");

            sb.Append(html.LabelFor(expression, htmlAttributes: new { @class = "control-label col-md-2" }).ToString())
            .Append("<div class='col-md-5'>"); //1
            if (popoverInfo != null)
            {
                sb.Append("<div class='input-group'>") //2

                .Append(html.EditorFor(expression, new { id = expression.Type.Name.ToId(), htmlAttributes = new { @class = "form-control" } }))
                .Append("<div class='input-group-btn'>") //3
                .Append("<button type='button' class='btn btn-default popoverBtn' ")
                .Append("data-container='body' data-toggle='popover' ")
                .Append("data-placement='bottom' ")
                .Append($"data-content=\"{popoverInfo}\">")
                .Append("<span class='glyphicon glyphicon-info-sign'></span>")
                .Append("</button></div>") //3
                .Append("</div>") //2
                .Append(html.ValidationMessageFor(expression, "", new { @class = "text-danger" }));//here
            }
            else
            {
                sb.Append(html.EditorFor(expression, new { id = editorId, htmlAttributes = new { @class = "form-control" } }))
                .Append(html.ValidationMessageFor(expression, "", new { @class = "text-danger" }));
            }

            sb.Append($"<p id='{errorId}' class='text-danger'></p>")

            .Append("</div>") //1
            .Append("</div>"); //0
            return MvcHtmlString.Create(sb.ToString());
        }



        #endregion

        #region Popover methods

        public static MvcHtmlString PopoverInfoFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            StringBuilder sb = new StringBuilder();

            var member = expression.Body as MemberExpression;
            var prop = member.Member as PropertyInfo;

            string popoverInfo = GetPopoverInfoFromProperty(prop);

            if(popoverInfo != null)
            {
                
                sb.Append($"<button type='button' class='btn btn-default popoverBtn {htmlAttributes.GetPropertyValue("class")}' ")
                .Append("data-container='body' data-toggle='popover' ")
                .Append("data-placement='bottom' ")
                .Append($"data-content=\"{popoverInfo}\" {htmlAttributes.RenderAttributesKeyValuePairExcept("class")} >")
                .Append($"{FASigns.InfoCircle}")
                .Append("</button>");
            }
            

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString PopoverInfoForSubject(this HtmlHelper html, Subject subjectEnum)
        {
            StringBuilder sb = new StringBuilder();

            string popoverInfo = subjectEnum.GetPopoverInfo();

            if (popoverInfo != null)
            {

                sb.Append($"<button type='button' class='btn btn-default popoverBtn' ")
                .Append("data-container='body' data-toggle='popover' ")
                .Append("data-placement='bottom' ")
                .Append($"data-content=\"{popoverInfo}\" >")
                .Append("<span class=\"glyphicon glyphicon-info-sign\" ></span>")
                .Append("</button>");
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        #endregion

        public static MvcHtmlString ErrorFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string errorText = null)
        {
            

            var member = expression.Body as MemberExpression;
            var prop = member.Member as PropertyInfo;

            if(errorText == null)
            {
                errorText = GetErrorTextFromProperty(prop);
            }
            

            return MvcHtmlString.Create($"<p id=\"{prop.Name}Error\" class=\"text-danger\">{errorText}</p>");

        }


        #endregion

        #region Получение значений из атрибута для свойства объекта
        public static string GetPopoverInfoFromProperty(PropertyInfo prop)
        {
            
            object[] attrs = prop.GetCustomAttributes(false);

            foreach (object attr in attrs)
            {
                PopoverInfoAttribute authAttr = attr as PopoverInfoAttribute;
                if (authAttr != null)
                {
                    return authAttr.Info;
                }
            }


            return null;
        }

        public static string GetErrorTextFromProperty(PropertyInfo prop)
        {

            object[] attrs = prop.GetCustomAttributes(true);

            foreach (object attr in attrs)
            {
                ErrorTextAttribute authAttr = attr as ErrorTextAttribute;
                if (authAttr != null)
                {
                    string propName = prop.Name;
                    string info = authAttr.Text;

                    return info;
                }
            }


            return null;
        }
        #endregion
    }
}