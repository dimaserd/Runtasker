using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Extensions.Reflection
{
    public static class ReflectionExtensionMethods
    {
        #region Reflection

        /// <summary>
        /// Получает словарь свойство - атрибуты для данного типа объекта
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public static Dictionary<string, object[]> GetPropertyAttributesDictionary(Type T)
        {
            PropertyInfo[] props = T.GetProperties();

            return props.ToDictionary(x => x.Name, x => x.GetCustomAttributes(false));
        }

        public static T GetAttribute<T>(this MemberInfo member, bool isRequired = true) where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }


        public static Dictionary<string,object> GetPropertiesDictionary(this object attributes)
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

        public static string GetPropertyName<T>(this Expression<Func<T, object>> lambda)
        {
            var member = lambda.Body as MemberExpression;
            var prop = member.Member as PropertyInfo;
            return prop.Name;
        }

        public static PropertyInfo GetPropertyInfo<T>(this Expression<Func<T, object>> lambda)
        {
            var member = lambda.Body as MemberExpression;
            return member.Member as PropertyInfo;
        }


        public static string GetPropertyValue(this object attributes, string propertyName)
        {
            if (attributes == null)
            {
                return null;
            }

            string result = "";

            foreach (var prop in attributes.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.Name == propertyName)
                {
                    result = prop.GetValue(attributes, null).ToString();
                }
            }

            return result;
        }

        public static string RenderAttributesKeyValuePair(this object attributes)
        {
            string result = "";
            if (attributes != null)
            {
                foreach (var prop in attributes.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    result += $" {prop.Name}=\"{prop.GetValue(attributes, null)}\"";
                }
            }

            return result;
        }

        public static string RenderAttributesKeyValuePairExcept(this object attributes, params string[] exceptNames)
        {
            string result = "";
            if (attributes != null)
            {
                foreach (var prop in attributes.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (!exceptNames.Any(x => x == prop.Name))
                    {
                        result += $" {prop.Name}='{prop.GetValue(attributes, null)}'";
                    }

                }
            }

            return result;
        }
        #endregion
    }
}
