using System.ComponentModel.DataAnnotations;
using UI.Settings.Entities;

namespace UI.Settings
{
    /// <summary>
    /// Статический класс настроек шаблона Atropos Responsive
    /// </summary>
    public static class AtroposHeaderSettings
    {
        #region Поля

        public static int HeaderHeight = 60;

        public static bool UseMyColor = false;


        public static RGBAColor HeaderColor = null;

        public static bool HasFirstLine = true;

        
        public static bool UseLeftLineHtml = false;

       
        public static string LeftLineHtml = "";

        
        public static bool UseRightLineHtml = false;

        
        public static string RightLineHtml = "";
        #endregion

        #region Методы

        public static void ChangeSettings(AtroposHeaderSettingsModel model)
        {
            UseMyColor = model.UseMyColor;
            HeaderColor = model.HeaderColor;
            HeaderHeight = model.HeaderHeight;
            HasFirstLine = model.HasFirstLine;
            UseLeftLineHtml = model.UseLeftLineHtml;
            LeftLineHtml = model.LeftLineHtml;

            UseRightLineHtml = model.UseRightLineHtml;
            RightLineHtml = model.RightLineHtml;
        }

        public static AtroposHeaderSettingsModel GetSettings()
        {
            return new AtroposHeaderSettingsModel
            {
                HeaderHeight = HeaderHeight,
                UseMyColor = UseMyColor,

                HeaderColor = HeaderColor,
                HasFirstLine = HasFirstLine,

                LeftLineHtml = LeftLineHtml,
                UseLeftLineHtml = UseLeftLineHtml,

                UseRightLineHtml = UseRightLineHtml,
                RightLineHtml = RightLineHtml
            }; 
        }

        public static string GetStylesString()
        {
            string styleString = string.Empty;

            if (UseMyColor)
            {
                styleString += $"background:rgba({HeaderColor.R}, {HeaderColor.G}, {HeaderColor.B}, {HeaderColor.A});";
            }

            return styleString;
        }

        #endregion
    }

    public class AtroposHeaderSettingsModel
    {
        [Display(Name = "Высота шапки")]
        public int HeaderHeight { get; set; }

        public bool UseMyColor { get; set; }

        public RGBAColor HeaderColor { get; set; }

        [Display(Name = "Показывать верхний кусок")]
        public bool HasFirstLine { get; set; }

        [Display(Name = "Использовать свой HTML слева")]
        public bool UseLeftLineHtml { get; set; }

        [Display(Name = "HTML слева")]
        public string LeftLineHtml { get; set; }

        [Display(Name = "Использовать свой HTML справа")]
        public bool UseRightLineHtml { get; set; }

        [Display(Name = "HTML справа")]
        public string RightLineHtml { get; set; }
    }
}