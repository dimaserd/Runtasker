using HtmlExtensions.HtmlEntities;
using System.Text;

namespace Logic.Extensions.HtmlEmail
{
    public class BigEmailCallToActionBase
    {
        #region Constructors
        public BigEmailCallToActionBase(string header, string bigText, HtmlLink link, string littleHeader = null)
        {
            Construct(header, bigText, link, littleHeader);
        }

        void Construct(string header, string bigText, HtmlLink link, string littleHeader = null)
        {
            Header = header;
            LittleHeader = littleHeader;
            BigText = bigText;
            Link = link;
            if(Link != null)
            {
                Link.Class = "btn";
            }
            
        }
        #endregion

        #region Properties
        public string Header { get; set; }

        public string LittleHeader { get; set; }

        public string BigText { get; set; }

        public HtmlLink Link { get; set; }
        #endregion

        #region Overriden Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GetHeader())
            .Append($"<p>{BigText}</p>");

            if(Link != null)
            {
                sb.Append(Link.ToString());
            }
            

            return sb.ToString();
        }
        #endregion

        #region Help Methods
        protected string GetHeader()
        {
            return $"<h3>{Header}" +
                ( (LittleHeader != null)? $"<small>{LittleHeader}</small>" : null )
                + "</h3>";
        }
        #endregion
    }
}
