using Extensions.Enumerations;
using HtmlExtensions.HtmlEntities;
using Runtasker.Logic.Entities;
using System.ComponentModel;
using System.Text;


namespace Runtasker.ExtensionsUI.UIExtensions.Alerts
{
    public static class AlertBuilder
    {

        public static string BuildAlertFromNotification(Notification N)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<div class='alert {N.Type.ToDisplayName()} alert-dismissable'>")
                .Append("<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>");
            if (!string.IsNullOrEmpty(N.Title))
            {
                sb.Append($"<h4>{N.Title}</h4> ");
            }
            sb.Append($"<p>{N.Text}</p>");
            
            if(!string.IsNullOrEmpty(N.Link))
            {
                sb.Append($"<p>{N.Link}</p>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        
    }

    #region Enums with extensions
    public enum AlertColorType
    {
        [Description("alert-success")]
        Success,
        [Description("alert-info")]
        Info,
        [Description("alert-warning")]
        Warning,
        [Description("alert-danger")]
        Danger
    }

    public enum AlertSizeType
    {
        JustText, WithTitleAndButtons
    }

    public static class EnumExtensions
    {
        public static string ToDescriptionString(this AlertColorType val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
    #endregion
}