using System.Text.RegularExpressions;

namespace Extensions.String
{
    public static class StringHtmlStripper
    {
        /// <summary>
        /// Удаляет все Html теги из строки
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}
