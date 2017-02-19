using Extensions.String;

namespace Runtasker.ExtensionsUI.UIExtensions.Orders
{
    public class HtmlActionButtonLink
    {
        #region Constructors


        public HtmlActionButtonLink(string buttonLink, string buttonText, string buttonClass)
        {
            Construct(buttonLink, buttonText, buttonClass);
        }

        public HtmlActionButtonLink(string buttonLink, string buttonText, object htmlAttributes)
        {
            Construct(buttonLink, buttonText, null, false, htmlAttributes);
        }

        public HtmlActionButtonLink(string buttonLink, string buttonText, string buttonClass, object htmlAttributes)
        {
            Construct(buttonLink, buttonText, buttonClass, false, htmlAttributes);
        }

        public HtmlActionButtonLink(string buttonLink, string buttonText, string buttonClass, bool disabled)
        {
            Construct(buttonLink, buttonText, buttonClass, disabled);
        }

        public HtmlActionButtonLink(string buttonLink, string buttonText, string buttonClass, bool disabled, object htmlAttributes)
        {
            
        }

        void Construct(string buttonLink = null, string buttonText = null,
            string buttonClass = null, bool disabled = false,
            object htmlAttributes = null)
        {
            ButtonLink = buttonLink;
            ButtonText = buttonText;
            HtmlClass = buttonClass;
            Disabled = disabled;
            HtmlAttributes = htmlAttributes;
        }
        #endregion

        #region Properties

        string ButtonLink { get; set; }
        string ButtonText { get; set; }
        string HtmlClass { get; set; }
        bool Disabled { get; set; }

        object HtmlAttributes { get; set; }

        #endregion

        #region Help Methods
        string RenderDisabled()
        {
            if (Disabled)
            {
                return "disabled";
            }
            else
            {
                return null;
            }
        }

        string RenderAttributes()
        {
            if(HtmlClass != null)
            {
                return HtmlAttributes.RenderAttributesKeyValuePairExcept("class");
            }
            return HtmlAttributes.RenderAttributesKeyValuePair();
        }
        #endregion

        #region Overridden Methods
        public override string ToString()
        {
            string result = $"<a {RenderDisabled()} href='{ButtonLink}'";
            
            if(HtmlClass != null)
            {
                result += $" class='{HtmlClass}' ";
            }
            return result + $"{RenderAttributes()}>{ButtonText}</a>"; ;
        }
        #endregion
    }
}
