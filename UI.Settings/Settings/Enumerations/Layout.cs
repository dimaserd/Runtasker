using System.ComponentModel.DataAnnotations;

namespace UI.Settings.Settings.Enumerations
{
    public enum LayoutType
    {
        [Display(Name = "Полный")]
        Full,

        [Display(Name = "Шаблонный")]
        AtroposStandard
    }
}
