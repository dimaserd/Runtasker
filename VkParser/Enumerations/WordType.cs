using System.ComponentModel;

namespace VkParser.Enumerations
{
    //добавляй новые строго с конца иначе проебем целостность в базе
    public enum WordType
    {
        [Description("Другое слово")]
        OtherWord,

        [Description("Иностранный язык")]
        ForeignLanguage,

        [Description("Высшая математика")]
        AdvancedMathematics,

        [Description("Химия")]
        Chemistry,

        [Description("Теоретическая механика")]
        TheoreticalMechanics,

        [Description("Физика")]
        Physics,

        [Description("Сопротивление материалов")]
        StrengthOfMaterials,

        [Description("Информатика")]
        Informatics,

        [Description("Программирование")]
        Programming,

        [Description("Проектирование")]
        Projecting
    }

    #region Extensions
    public static class WordTypeExtensions
    {
        public static string ToDescriptionString(this WordType val)
        {
            DescriptionAttribute[] attributes = 
                (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
    #endregion
}
