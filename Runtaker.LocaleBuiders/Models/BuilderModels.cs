
namespace Runtasker.LocaleBuilders.Models
{
    public class ForNotification
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public string ActionBtnText { get; set; }
    }

    public class ForEmailCallToAction
    {
        //Not decided yet where it should be
        //public string Subject { get; set; }

        public string Header { get; set; }

        public string BigText { get; set; }

        public string LittleHeader { get; set; }

        public string ActionBtnText { get; set; }
    }
}
