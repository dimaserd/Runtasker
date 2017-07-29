using Extensions.Reflection;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Extensions.String
{
    public static class StringHtmlWrapper
    {
        #region Wrappers

        /// <summary>
        /// Получает словарь свойств объекта
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetPropertiesDictionary(this object attributes)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (attributes != null)
            {
                foreach (var prop in attributes.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    result.Add(prop.Name, prop.GetValue(attributes, null));
                }
            }

            return result;
        }

        public static string GetAttributesString(this object attributes)
        {
            Dictionary<string, object> result = GetPropertiesDictionary(attributes);

            StringBuilder sb = new StringBuilder();

            foreach (var item in result)
            {
                sb.Append($"{item.Key}=\"{item.Value}\" ");
            }

            return sb.ToString();
        }

        public static string WrapToHtmlTag(this string s, string tagName, object attributes = null)
        {

            string attrs_String = attributes.GetAttributesString();

            return $"<{tagName} {attrs_String}>{s}</{tagName}>";
        }

        /// <summary>
        /// Заворачивает входную строку в HTML-тег em
        /// который наклоняет текст
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string WrapToEm(this string s)
        {
            return $"<em>{s}</em>";
        }

        /// <summary>
        /// Заворачивает входную строку в тег strong
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string WrapToStrong(this string s)
        {
            return $"<strong>{s}</strong>";
        }

        public static string WrapToSpan(this string s, object htmlAttributes = null)
        {
            string keyValueAttributesPair = string.Empty;
            if(htmlAttributes != null)
            {
                keyValueAttributesPair = ReflectionExtensionMethods.RenderAttributesKeyValuePair(htmlAttributes);
            }

            return $"<span{keyValueAttributesPair}>{s}</span>";
        }

        #region Links a
        public static string WrapToA(this string s, string href)
        {
            return $"<a href=\"{href}\">{s}</a>";
        }

        public static string WrapToA(this string s, object htmlAttributes)
        {
            string keyValueAttributesPair = ReflectionExtensionMethods.RenderAttributesKeyValuePair(htmlAttributes);
            return $"<a{keyValueAttributesPair}>{s}</a>";
        }

        #endregion

        #region Headers h1, h2...

        public static string WrapToH1(this string s)
        {
            return $"<h1>{s}</h1>";
        }

        public static string WrapToH2(this string s)
        {
            return $"<h2>{s}</h2>";
        }

        public static string WrapToH3(this string s)
        {
            return $"<h3>{s}</h3>";
        }

        #endregion

        

        
        #endregion

    }
}
