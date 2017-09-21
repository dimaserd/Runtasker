using System.ComponentModel.DataAnnotations;
using UI.Settings.Entities;
using UI.Settings.Enumerations;
using UI.Settings.Settings.Enumerations;

namespace UI.Settings
{
    /// <summary>
    /// Статический класс настроек шаблона Atropos Responsive
    /// </summary>
    public static class AtroposSettings
    {
        public static bool IsFullWidth = true;

        public static bool IsDarkLayout = false;

        public static AtroposColorScheme ColorScheme = AtroposColorScheme.Orange;

        public static bool UseUniqueBackgroundColor = false;

        public static RGBAColor BackgroundColor = null;

        public static JqeryType Jquery = JqeryType.Cdn;

        public static UIFormType FormType = UIFormType.Ordinary;

        public static LayoutType Layout = LayoutType.AtroposStandard;

        public static bool IsMultiLang = false;

        public static bool IsShopEnabled = false;

        public static bool IsRegistrationEnabled = false;

        public static bool WarehousesChangeEnabled = false;

        public static int WrapperPaddingTop = 85;
        
        #region Методы
        public static void ChangeSettings(AtroposSettingsModel model)
        {
            IsFullWidth = model.IsFullWidth;
            IsDarkLayout = model.IsDarkLayout;
            ColorScheme = model.ColorScheme;
            UseUniqueBackgroundColor = model.UseUniqueBackgroundColor;
            BackgroundColor = model.BackgroundColor;
            Jquery = model.Jquery;
            FormType = model.FormType;
            Layout = model.Layout;
            IsMultiLang = model.IsMultiLang;
            WrapperPaddingTop = model.WrapperPaddingTop;
            IsShopEnabled = model.IsShopEnabled;
            IsRegistrationEnabled = model.IsRegistrationEnabled;
        }

        public static AtroposSettingsModel GetSettings()
        {
            return new AtroposSettingsModel
            {
                IsFullWidth = IsFullWidth,
                IsDarkLayout = IsDarkLayout,
                ColorScheme = ColorScheme,
                BackgroundColor = BackgroundColor,
                Jquery = Jquery,
                FormType = FormType,
                Layout = Layout,
                IsMultiLang = IsMultiLang,
                WrapperPaddingTop = WrapperPaddingTop,
                IsShopEnabled = IsShopEnabled,
                IsRegistrationEnabled = IsRegistrationEnabled
            };
        }

        public static string GetFormClass()
        {
            return (FormType == UIFormType.Ordinary) ? "form-horizontal" : "well center-block";
        }

        public static string GetStylesString()
        {
            string styleString = string.Empty;

            if (UseUniqueBackgroundColor)
            {
                styleString += $"background:rgba({BackgroundColor.R}, {BackgroundColor.G}, {BackgroundColor.B}, {BackgroundColor.A});";
            }

            return styleString;
        }

        public static string GetWrapperStylesString()
        {
            return $"padding-top:{WrapperPaddingTop}px";
        }
        #endregion
    }

    public class AtroposSettingsModel
    {
        [Display(Name = "На всю ширину")]
        public bool IsFullWidth { get; set; }

        [Display(Name = "Сделать темным")]
        public bool IsDarkLayout { get; set; }

        public AtroposColorScheme ColorScheme { get; set; }

        public bool UseUniqueBackgroundColor { get; set; }

        public RGBAColor BackgroundColor { get; set; }

        public JqeryType Jquery { get; set; }

        public UIFormType FormType { get; set; }

        public LayoutType Layout { get; set; }

        [Display(Name = "Использовать переключение языков")]
        public bool IsMultiLang { get; set; }

        [Display(Name = "Магазин включен")]
        public bool IsShopEnabled { get; set; }

        [Display(Name = "Включено переключение складов")]
        public bool WarehousesChangeEnabled { get; set; }

        [Display(Name = "Отступ сверху")]
        public int WrapperPaddingTop { get; set; }

        [Display(Name = "Включена регистрация")]
        public bool IsRegistrationEnabled { get; set; }
    }
}