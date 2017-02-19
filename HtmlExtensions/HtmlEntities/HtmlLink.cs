namespace HtmlExtensions.HtmlEntities
{
    public class HtmlLink
    {
        #region Constructors

        public HtmlLink(string hrefParam, string textParam)
        {
            href = hrefParam;
            Text = textParam;
        }

        public HtmlLink(string hrefParam, string textParam, HtmlButtonType buttonTypeParam, HtmlButtonSize buttonSizeParam = HtmlButtonSize.Default)
        {
            href = hrefParam;
            Text = textParam;
            Class = $"btn {buttonTypeParam.ToDescriptionString()} {buttonSizeParam.ToDescriptionString()}";
            PreparedHtmlLink = $"<a class='{Class}' href='{href}'>{Text}</a>";
        }
        #endregion

        private string href { get; set; }
        private string Text { get; set; }
        public string Class { get; set; }
        private string PreparedHtmlLink { get; set; }

        public override string ToString()
        {
            if (Class == null)
            {
                return $"<a href='{href}'>{Text}</a>";
            }
            else
            {
                return $"<a class='{Class}' href='{href}'>{Text}</a>";
            }

        }


    }

    
}