using System.ComponentModel.DataAnnotations;

namespace UI.Settings.Enumerations
{
    public enum UIFormType
    {
        [Display(Name = "Обычная форма")]
        Ordinary,

        [Display(Name = "Плавающий лейбл")]
        FloatLabel
    }
}
