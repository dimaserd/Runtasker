using Extensions.Reflection;

namespace Extensions.String
{
    public static class StringHtmlWrapper
    {
        #region Wrappers

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
