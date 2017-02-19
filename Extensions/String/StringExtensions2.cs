using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.String
{
    public static class StringExtensions2
    {
        public static List<string> GetWordsFromText(this string text)
        {
            //var text = "'Oh, you can't help that,' said the Cat: 'we're all mad here. I'm mad. You're mad.'";
            char[] punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();

            List<string> words = text.Split().Select(x => x.Trim(punctuation)).ToList().Where(x => x.Length > 2).ToList();

            //запуск вспомогательного метода
            return HelpForGetWordsFromTextMethod(words);
        }

        public static List<string> HelpForGetWordsFromTextMethod(List<string> words)
        {
            //1.Тонкий
            //стержнем.Дано
            //Решение:Разобьем
            //равна:.Проинтегрировав
            //получим:,.Ответ

            List<string> result = new List<string>();

            foreach (string word in words)
            {
                result.AddRange(GetWordsFromWordWithSymbols(word));
            }
            return result;
        }

        #region Help Methods

        public static List<string> GetWordsFromWordWithSymbols(this string word)
        {
            string[] toReplaces = new string[]
            {
                "0", "1", "2", "3", "4",
                "5", "6", "7", "8", "9",
                ":", ",", ".", "?", ";",
                "\\", "|", "~", "-", "'",
                "\"", ">", "<", "`", "*",
                "(", ")"

            };
            return word.Split(separator: toReplaces, options: StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        #endregion
    }
}
