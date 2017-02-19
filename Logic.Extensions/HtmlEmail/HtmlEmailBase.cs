using System.Text;
using System.Web.Hosting;

namespace Logic.Extensions.HtmlEmail
{
    public class HtmlEmailBase
    {
        #region Constants
        public string Host
        {
            get { return @"https://runtasker.ru"; }
        }
        #endregion

        #region Constructors

        public HtmlEmailBase(BigEmailCallToActionBase callToAction)
        {
            Construct(callToAction);
        }

        public HtmlEmailBase(string name, string leadText,
            string callOut, string imageName,
            BigEmailCallToActionBase callToAction)
        {
            Construct(callToAction, name, leadText, callOut, imageName);
        }

        void Construct(BigEmailCallToActionBase callToAction, string name = null,
            string leadText = null,
            string callOut = null, string imageName = null
            )
        {
            Name = name;
            LeadText = leadText;
            CallOutTextWithLink = callOut;


            ImageName = imageName;
            CallToAction = callToAction;
        }
        #endregion

        #region Properties

        string Name { get; set; }

        string LeadText { get; set; }

        string ImageName { get; set; }

        //blue text with prepared html link to click on
        string CallOutTextWithLink { get; set; }

        BigEmailCallToActionBase CallToAction { get; set; }

        #endregion

        #region Protected Main Methods
        protected string GetStyles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<style type='text/css'>");

            string rootDir = HostingEnvironment.MapPath("~/Files");

            string filePath = $"{rootDir}/Site/email.css";
            string fileContents = System.IO.File.ReadAllText(filePath);
            sb.Append("</style>");
            return sb.ToString();
        }

        protected string GetStarted()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' ")
            .Append("'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
            .Append("<html xmlns='http://www.w3.org/1999/xhtml' >");

            return sb.ToString();
        }

        protected string GetHead()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<head>")
            .Append("<meta name='viewport' content='width=device-width' />")
            .Append("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />")
            .Append("<title>Runtasker email</title>")
            //Styles are working
            .Append($"<link rel='stylesheet' type='text/css' href='{Host}/File/GetFileFromSite?filename=email.css' />")

            .Append("</head>");

            //sb.Append(GetStyles());

            return sb.ToString();
        }

        protected string GetBody()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<body bgcolor='#FFFFFF'>")
            .Append(GetBodyHeader())
            .Append(GetBodyBody())
            .Append(GetFooter())
            .Append("</body></html>");

            return sb.ToString();

        }

        #endregion

        #region Help Methods

        #region Body Building Methods
        //FileFromSite
        protected string GetBodyHeader()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table class='head-wrap' bgcolor='#000000'>")
            .Append("<tr><td></td>")
            .Append("<td class='header container'><div class='content'>")
            .Append("<table bgcolor='#000000'><tr>")
            .Append("<td align='right'><h6 class='collapse'>")
            .Append($"<a href='{Host}'>RUNTASKER EMAIL SUPPORT</a></h6></td>")
            .Append("</tr></table></div></td>")
            .Append("<td></td></tr></table>");

            return sb.ToString();
        }

     
        protected string GetBodyBody()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table class='body-wrap'><tr><td></td>")
            .Append("<td class='container' bgcolor='#FFFFFF'><div class='content'>")
            .Append("<table><tr><td>")
            .Append(GetGreeting())
            .Append(GetLeadText())
            .Append(GetImage())
            .Append(GetCallOutPanel())
            .Append($"{CallToAction.ToString()}<br/><br/>")
            .Append(GetSocialAndContact())
            .Append("</td></tr></table></div></td><td></td></tr></table>");





            return sb.ToString();
        }

        protected string GetFooter()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table class='footer-wrap'><tr><td></td>")
            .Append("<td class='container'>")
            .Append("<div class='content'><table><tr><td align='center'>")
            .Append("<p><a href='#'>Terms</a> | ")
            .Append("<a href='#'>Privacy</a> | ")
            .Append("<a href='#'><unsubscribe>Unsubscribe</unsubscribe></a>")
            .Append("</p></td></tr></table></div>")
            .Append("</td><td></td></tr></table>");


            return sb.ToString();
        }
        #endregion

        #region GetBodyBody Help Methods
        protected string GetGreeting()
        {
            if(Name != null)
            {
                return $"<h3>Hello, {Name}!</h3>";
            }
            return null;
        }

        protected string GetLeadText()
        {
            if(LeadText != null)
            {
                return $"<p class='lead'>{LeadText}</p>";
            }
            return null;
        }

        //600 * 300 image
        protected string GetImage()
        {
            if(ImageName != null)
            {
                return $"<p><img src='{Host}/File/GetFileFromSite?filename={ImageName}' /></p>";
            }
            return null;
        }

        //Blue text with a link
        protected string GetCallOutPanel()
        {
            if (CallOutTextWithLink != null)
            {
                return $"<p class='callout'>{CallOutTextWithLink}</p>";
            }
            return null;
        }

        protected string GetSocialAndContact()
        {
            return new HtmlEmailSocialAndContactRenderer().ToString();
        }
        #endregion


        #endregion


        #region Overridden methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetStarted())
                .Append(GetHead())
                .Append(GetBody());

            return sb.ToString();
        }
        #endregion
    }
}
