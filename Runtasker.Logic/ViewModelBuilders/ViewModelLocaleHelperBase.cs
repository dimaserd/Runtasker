
using HtmlExtensions.Renderers;

namespace Runtasker.Logic.ViewModelBuilders
{
    public class ViewModelLocaleHelperBase
    {
        #region Constructors
        public ViewModelLocaleHelperBase()
        {
            Construct();
        }

        void Construct()
        {
            FASigns = new FontAwesomeRenderer();
            GISigns = new GlyphiconRenderer();
            HtmlSigns = new HtmlSignsRenderer();
        }
        #endregion

        #region Protected Properties
        protected FontAwesomeRenderer FASigns { get; set; }

        protected GlyphiconRenderer GISigns { get; set; }

        protected HtmlSignsRenderer HtmlSigns { get; set; }
        #endregion
    }
}
