using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Extensions.String
{
    public static class StringExtension
    {
        

        public static string GetShortName(this string s)
        {
            if (s.Length > 18)
            {
                return $"{s.Substring(0, 18)}..";
            }

            return s;
        }

        public static string GetNameAndSurname(this string s)
        {
            int count = 0;
            int surnamePosition = 0;
            char c;
            for (int i = 0; i < s.Length; i++)
            {
                c = s[i];
                if (char.IsUpper(c))
                {
                    count++;
                    if (count == 2)
                    {
                        surnamePosition = i;
                    }
                }
            }

            return s.Substring(0, surnamePosition) + " " + s.Substring(surnamePosition, s.Length - surnamePosition);
        }

        public static string ToId(this string s)
        {
            string first = s.Substring(0, 1).ToLowerInvariant();

            return first + s.Substring(1);
        }

        public static MvcHtmlString ToHtml(this string s)
        {
            return MvcHtmlString.Create(s);
        }

        #region Методы выделения и обертывания
        public static string MarkManyText(this string Text, string toMarkMany)
        {
            string[] words = toMarkMany.Split(separator: new string[] { "," }, options: StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                Text = Text.MarkText(word);
            }
            return Text;
        }

        public static string MarkText(this string Text, string toMark)
        {
            int start = Text.IndexOf(toMark);

            if (start == -1)
            {
                return Text;
            }
            return $"{Text.Substring(0, start)}<mark>{Text.Substring(start, toMark.Length)}</mark>{Text.Substring(start + toMark.Length)}";
        }

        public static string WrapUrls(this string Text)
        {
            string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            Regex urlRegex = new Regex(pattern);
            foreach (Match ItemMatch in urlRegex.Matches(Text))
            {
                Uri uri = new Uri(ItemMatch.Value);
                //creating a uri to get just host to link
                string n = $"<a href='{ItemMatch}'>{uri.Host}</a>";
                Text = Text.Substring(0, ItemMatch.Index) + n + Text.Substring(ItemMatch.Index + ItemMatch.Length);


            }

            return Text;
        }


        public static string WrapEmails(this string Text)
        {
            Regex urlRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            foreach (Match ItemMatch in urlRegex.Matches(Text))
            {
                Uri uri = new Uri(ItemMatch.Value);
                //creating a uri object from spotted email
                string n = $"<a href='{ItemMatch}'>{uri.Host}</a>";
                Text = Text.Substring(0, ItemMatch.Index) + n + Text.Substring(ItemMatch.Index + ItemMatch.Length);
            }

            return Text;
        }
        #endregion

        #region Lefters
        public static List<string> LeftJustNames(this List<string> filenames)
        {
            List<string> files = new List<string>();
            char[] separators = new char[] { '/', '\\' };

            foreach (string filename in filenames)
            {
                string[] bites = filename.Split(separators);
                files.Add(bites.Last());
            }

            return files;
        }

        /// <summary>
        /// Разбивает строку по символам / и \ и берет последний кусок
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string LeftJustFileName(this string filePath)
        {
            return filePath.Split('/', '\\').Last();
        }
        #endregion

        public static List<string> RemoveZip(this List<string> filenames)
        {
            List<string> files = new List<string>();

            foreach (string filename in filenames)
            {
                string[] bites = filename.Split(new char[] { '.' });
                if (bites[bites.Length - 1] != "zip")
                {
                    files.Add(filename);
                }
            }

            return files;
        }

        public static string MakeFileNameUniqueAtList(this string s, List<string> filenamesList, bool host = true)
        {
            if (host)
            {
                filenamesList = filenamesList.LeftJustNames();
            }

            string a, b, filename = s;
            if (!s.TryGetNameandExt(out a, out b))
            {
                throw new Exception();
            }

            while (filenamesList.Contains(filename))
            {
                filename = filename.IncrementFileVersion();
            }
            return filename;
        }

        public static string RemoveSymbols(this string s, params char[] symbols)
        {
            string result = s;

            foreach(char symbol in symbols)
            {
                result = result.Replace(symbol.ToString(), string.Empty);
            }
            return result;
        }

        public static string LeftJustOneDot(this string s)
        {
            string result = s;
            int index;
            while (result.Count(a => a == '.') > 1)
            {
                index = result.IndexOf('.');
                if (index > 0 && index < result.Length - 1)
                {
                    result = result.buildFromStartToEnd(0, index - 1) + result.buildFromStartToEnd(index + 1, result.Length - 1);
                }
                else if (index == 0)
                {
                    result = result.buildFromStartToEnd(1, result.Length - 1);
                }
                else if (index == result.Length - 1)
                {
                    result = result.buildFromStartToEnd(0, result.Length - 2);
                }
            }
            return result;
        }

        /// <summary>
        /// Получает обратную строку из текущей
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetReverse(this string s)
        {
            string new_string = string.Empty;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                new_string += s[i];
            }
            return new_string;
        }

        public static bool TryGetNameandExt(this string s, out string fileName, out string fileExtension)
        {
            char[] sep = new char[1] { '.' };
            string[] temp = s.Split(sep);
            if (temp.Length == 2)
            {
                fileName = temp[0];
                fileExtension = temp[1];
                return true;
            }
            else if(temp.Length > 2)
            {
                int firstIndexOfDot = 0;
                for(int i = 0; i < s.Length; i++)
                {
                    if(s[i] == '.')
                    {
                        firstIndexOfDot = i;
                        break;
                    }
                }


                fileName = s.Substring(0, firstIndexOfDot);
                fileExtension = s.Substring(firstIndexOfDot + 1);
                return true;
            }
            else
            {
                fileName = "";
                fileExtension = "";
                return false;
            }
        }

        public static string buildFromStartToEnd(this string s, int start, int end)
        {
            if (end < start || start < 0 || start > s.Length - 1 || end < 0 || end > s.Length - 1)
            {
                throw new NullReferenceException();
            }

            StringBuilder sb = new StringBuilder();
            for (int i = start; i <= end; i++)
            {
                sb.Append(s[i]);
            }
            return sb.ToString();
        }

        public static bool TryGetFileVersionfromBrackets(this string str, out int result)
        {
            
            if (!str.TryGetNameandExt(out string s, out string ext))
            {
                result = 0;
                return false;
            }

            int f = 0, l = 0, count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '(')
                {
                    count++;
                    f = i;
                }

                if (s[i] == ')')
                {
                    count++;
                    l = i;
                }
            }

            if (count != 2)
            {
                result = 0;
                return false;
            }

            int temp;
            if (int.TryParse(s.buildFromStartToEnd(f + 1, l - 1), out temp))
            {
                result = temp;
                return true;
            }
            else
            {
                result = 0;
                return false;
            }

        }



        //доделать
        public static string RemoveVersion(this string s)
        {


            int temp;
            if (s.TryGetFileVersionfromBrackets(out temp))
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                string name, ext;
                s.TryGetNameandExt(out name, out ext);
                for (i = 0; i < name.Length; i++)
                {
                    sb.Append(s[i]);
                    if (s[i] == '(')
                    {
                        sb.Remove(startIndex: i, length: 1);
                        break;
                    }
                }
                return sb.ToString() + "." + ext;
            }
            else
            {
                throw new Exception();
            }
        }

        public static string IncrementFileVersion(this string s)
        {
            int version;
            string name, ext;
            if (!s.TryGetNameandExt(out name, out ext))
            {
                throw new Exception();
            }

            if (s.TryGetFileVersionfromBrackets(out version))
            {
                version++;
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < name.Length; i++)
                {
                    sb.Append(name[i]);
                    if (name[i] == '(')
                    {
                        sb.Remove(startIndex: i, length: 1);
                        break;
                    }
                }
                return string.Format("{0}{1}{2}{3}{4}", sb, "(", version, ").", ext);
            }
            else
            {
                return string.Format("{0}{1}{2}", name, "(1).", ext);
            }
        }

    }
}
