using System.Web.Optimization;

namespace Runtasker
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region Main Bundles
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/Sit.css"));
            #endregion

            #region NewDesign

            #region Scripts
            bundles.Add(new ScriptBundle("~/NewDesign/Jquery").Include("~/Scripts/NewDesign/jquery.appear.js",
                "~/Scripts/NewDesign/jquery.cookie.js", "~/Scripts/NewDesign/jquery.easing.1.3.js",
                "~/Scripts/NewDesign/jquery.isotope.js", "~/Scripts/NewDesign/masonry.js"));

            bundles.Add(new ScriptBundle("~/NewDesign/Scripts").Include("~/Scripts/NewDesign/scripts.js"));
            #endregion

            #region Styles

            bundles.Add(new StyleBundle("~/bootstrap/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/MyStyles/myBootstrap.css"));

            bundles.Add(new StyleBundle("~/blog/css").Include(
                      "~/Content/NewDesign/blog.css"));

            bundles.Add(new StyleBundle("~/owl-carousel/css").Include(
                      "~/Content/NewDesign/owl-carousel/owl.carousel.css",
                      "~/Content/NewDesign/owl-carousel/owl.theme.css",
                      "~/Content/NewDesign/owl-carousel/owl.transitions.css"));

            bundles.Add(new StyleBundle("~/animate/css").Include("~/Content/NewDesign/animate.css"));

            bundles.Add(new StyleBundle("~/magnific-popup/css").Include("~/Content/NewDesign/magnific-popup.css"));

            bundles.Add(new StyleBundle("~/superslides/css").Include("~/Content/NewDesign/superslides.css"));
            
            bundles.Add(new StyleBundle("~/ColorTheme/darkblue").Include("~/Content/NewDesign/ColorThemes/darkblue.css"));

            bundles.Add(new StyleBundle("~/NewDesign/Essentials").Include("~/Content/NewDesign/Essentials/essentials.css",          
                "~/Content/NewDesign/Essentials/layout.css", "~/Content/NewDesign/Essentials/layout-responsive.css"));

            bundles.Add(new StyleBundle("~/DarkLayout").Include("~/Content/NewDesign/Essentials/layout-dark.css"));
            
            #endregion

            #endregion

            #region For DateTime Picker
            bundles.Add(new ScriptBundle("~/DateTimePicker").Include("~/Scripts/MyScripts/moment-with-locales.min.js", 
                "~/Scripts/MyScripts/bootstrap-datetimepicker.min.js"));
            bundles.Add(new StyleBundle("~/DateTimePickerCss").Include("~/Content/MyStyles/bootstrap-datetimepicker.min.css"));
            #endregion

            #region  For Chat
            bundles.Add(new StyleBundle("~/Chat").Include("~/Content/MyStyles/Chat.css"));
            bundles.Add(new ScriptBundle("~/SignalR").Include("~/Scripts/jquery.signalR-2.2.1.min.js"));
            bundles.Add(new ScriptBundle("~/ChatScript").Include("~/Scripts/MyScripts/ChatScripts"));
            #endregion
            //Social Login Buttons
            bundles.Add(new StyleBundle("~/Social").Include("~/Content/MyStyles/bootstrap-social.css"));

            #region  Jasny Bootstrap file input extensions
            bundles.Add(new StyleBundle("~/Jasny").Include("~/Content/MyStyles/jasny-bootstrap.min.css"));
            bundles.Add(new ScriptBundle("~/JasnyScript").Include("~/Scripts/MyScripts/jasny-bootstrap.min.js"));
            #endregion

            //OrdersList stylesheet call to action
            bundles.Add(new StyleBundle("~/OrdersList").Include("~/Content/MyStyles/bootsnip-calltoaction.css"));

            #region RatingBox
            bundles.Add(new ScriptBundle("~/RatingBoxScript").Include("~/Scripts/MyScripts/RatingBoxScript.js"));
            bundles.Add(new StyleBundle("~/RatingBox").Include("~/Content/MyStyles/RatingBox.css"));
            #endregion

            #region Comments
            bundles.Add(new StyleBundle("~/Comments").Include("~/Content/MyStyles/Comments.css"));
            bundles.Add(new ScriptBundle("~/CoomentsScript").Include("~/Scripts/MyScripts/CommentsScript.js"));
            #endregion

            #region CustomFile Input
            bundles.Add(new StyleBundle("~/CustomFileInputCSS")
                .Include(
                "~/Content/Plugins/FileInput/component.css",
                "~/Content/Plugins/FileInput/normalize.css"));

            bundles.Add(new ScriptBundle("~/CustomFileInputJS")
                .Include(
                //"~/Scripts/Plugins/FileInput/custom-file-input.js",
                "~/Scripts/Plugins/FileInput/jquery.custom-file-input.js"));
            #endregion
        }
    }
}
