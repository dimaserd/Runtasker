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
        #endregion

        #region Методы

        public static void ChangeSettings(AtroposHeaderSettingsModel model)
        {
            UseMyColor = model.UseMyColor;
            HeaderColor = model.HeaderColor;
            HeaderHeight = model.HeaderHeight;
            HasFirstLine = model.HasFirstLine;
        }

        public static AtroposHeaderSettingsModel GetSettings()
        {
            return new AtroposHeaderSettingsModel
            {
                HeaderHeight = HeaderHeight,
                UseMyColor = UseMyColor,
                HeaderColor = HeaderColor,
                HasFirstLine = HasFirstLine
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
        public int HeaderHeight { get; set; }

        public bool UseMyColor { get; set; }

        public RGBAColor HeaderColor { get; set; }

        public bool HasFirstLine { get; set; }
    }
}