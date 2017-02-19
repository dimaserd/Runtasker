using System.ComponentModel;

namespace HtmlExtensions.HtmlEntities
{
    //<button type="button" class="btn btn-danger">Выполнить это действие</button>
    public class HtmlButton
    {
        public HtmlButton(HtmlButtonType buttonTypeParam, HtmlButtonSize buttonSizeParam = HtmlButtonSize.Default, HtmlLink linkParam = null, string textParam = null)
        {
            buttonSize = buttonSizeParam;
            buttonType = buttonTypeParam;
            Text = textParam;
            Link = linkParam;
        }

        public HtmlLink Link { get; set; }

        public HtmlButtonSize buttonSize { set; get; }

        public HtmlButtonType buttonType { get; set; }

        public string Text { get; set; }

        public override string ToString()
        {
            if(Link != null)
            {
                return $"<button type='button' class='btn {buttonType.ToDescriptionString()} {buttonSize.ToDescriptionString()}'>{Link.ToString()}</button>";
            }
            else
            {
                return $"<button type='button' class='btn {buttonType.ToDescriptionString()} {buttonSize.ToDescriptionString()}'>{Text}</button>";
            }

        }
    }

    #region Enums with extensions
    public enum HtmlButtonType
    {
        [Description("btn-default")]
        Default,
        [Description("btn-primary")]
        Primary,
        [Description("btn-success")]
        Success,
        [Description("btn-info")]
        Info,
        [Description("btn-warning")]
        Warning,
        [Description("btn-danger")]
        Danger,
        [Description("btn-link")]
        Link
    }

    public enum HtmlButtonSize
    {
        [Description("btn-xs")]
        Tiny,
        [Description("btn-sm")]
        Little,
        [Description("")]
        Default,
        [Description("btn-lg")]
        Large,
        [Description("btn-block")]
        XLarge
    }

    public static class HtmlButtonExtensions
    {
        public static string ToDescriptionString(this HtmlButtonType val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString(this HtmlButtonSize val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
    #endregion
}