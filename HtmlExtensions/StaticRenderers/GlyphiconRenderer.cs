
using System.Web;

namespace HtmlExtensions.StaticRenderers
{
    public class GlyphiconEntity
    {
        public GlyphiconEntity(string afterGlyphiconClass)
        {
            AfterGlyphiconClass = afterGlyphiconClass;
        }

        public string Id { get; set; }

        public string AfterGlyphiconClass { get; set; }

        public override string ToString()
        {
            return $"<span id=\"{Id}\" class=\"glyphicon {AfterGlyphiconClass}\"></span>";
        }

        public void SetId(string id)
        {
            Id = id;
        }

        public HtmlString ToHtml()
        {
            return new HtmlString(this.ToString());
        }
    }

    public static class GISigns
    {
        public static GlyphiconEntity Trash
        {
            get { return new GlyphiconEntity("glyphicon-trash"); }
        }

        public static GlyphiconEntity MinusSign
        {
            get { return new GlyphiconEntity("glyphicon-minus"); }
        }

        public static GlyphiconEntity ListAlt
        {
            get { return new GlyphiconEntity("glyphicon-list-alt"); }
        }

        public static GlyphiconEntity Pencil
        {
            get { return new GlyphiconEntity("glyphicon-pencil"); }
        }

        public static GlyphiconEntity Send
        {
            get { return new GlyphiconEntity("glyphicon-send"); }
        }

        public static GlyphiconEntity Gift
        {
            get { return new GlyphiconEntity("glyphicon-gift"); }
        }

        public static GlyphiconEntity PiggyBank
        {
            get { return new GlyphiconEntity("glyphicon-piggy-bank"); }
        }

        public static GlyphiconEntity Login
        {
            get { return new GlyphiconEntity("glyphicon-log-in"); }
        }

        public static GlyphiconEntity Envelope
        {
            get { return new GlyphiconEntity("glyphicon-envelope"); }
        }


        public static GlyphiconEntity User
        {
            get { return new GlyphiconEntity("glyphicon-user"); }
        }

        public static GlyphiconEntity Star
        {
            get { return new GlyphiconEntity("glyphicon-star"); }
        }

        public static GlyphiconEntity Save
        {
            get { return new GlyphiconEntity("glyphicon-save"); }
        }

        public static GlyphiconEntity Briefcase
        {
            get { return new GlyphiconEntity("glyphicon-briefcase"); }
        }

        public static GlyphiconEntity PlusSign
        {
            get { return new GlyphiconEntity("glyphicon-plus"); }
        }

        public static string Count(int count, string id = null)
        {
            return $"<span id=\"{id}\" class='badge'>{count}</span>";
        }

        public static GlyphiconEntity Configuration
        {
            get { return new GlyphiconEntity("glyphicon-cog"); }
        }

        public static GlyphiconEntity Ok
        {
            get { return new GlyphiconEntity("glyphicon-ok"); }
        }

        public static GlyphiconEntity Refresh
        {
            get { return new GlyphiconEntity("glyphicon-refresh"); }
        }


    }
}
