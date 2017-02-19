using Runtasker.Resources.HtmlEmail.SocialAndContact;
using System.Text;

namespace Logic.Extensions.HtmlEmail
{
    //Not ready not implemented
    public class HtmlEmailSocialAndContactRenderer
    {
        #region Constants
        public string Host
        {
            get { return @"https://runtasker.ru"; }
        }

        public string ContactEmail
        {
            get { return "contact@runtasker.ru"; }
        }

        public string ContactPhone
        {
            get { return "8-916-604-49-60"; }
        }

        #region SocialNetworks Links
        public string Facebook
        {
            get { return "#"; }
        }

        public string Twitter
        {
            get { return "#"; }
        }

        public string GooglePlus
        {
            get { return "#"; }
        }
        #endregion
        #endregion

        //socials are swithced off
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table class='social' width='100%' ><tr><td>")
            //.Append("<table align='left' class='column'><tr><td>")
            //.Append(RenderSocials())            
            //.Append("</td></tr></table>")

            .Append("<table align='left' class='column'><tr><td>")
            .Append(RenderContactInfo())
            .Append("</td></tr></table>")
            .Append("<span class='clear'></span></td></tr></table>");

            return sb.ToString();
        }

        #region Protected Help Methods

        protected string RenderSocials()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<h5>Connect with Us:</h5><p>")
            .Append($"<a href='{Facebook}' class='soc-btn fb'>Facebook</a>")
            .Append($"<a href='{Twitter}' class='soc-btn tw'>Twitter</a>")
            .Append($"<a href='{GooglePlus}' class='soc-btn gp'>Google+</a>")
            .Append("</p>");

            return sb.ToString();
        }

        protected string RenderContactInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<h5>{SocContactRes.ContactInfo}:</h5><p>")
            //.Append($"Phone: <strong>{ContactPhone}</strong><br/>")

            .Append("Email: <strong>")
            .Append($"<a href='emailto: {ContactEmail}'>")
            .Append($"{ContactEmail}</a></strong></p>");

            return sb.ToString();
        }
        #endregion
    }
}
