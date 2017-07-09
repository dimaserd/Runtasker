using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Extensions.Enumerations
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        //public static string ToDisplayName<TAttribute>(this Enum enumValue)
        //{
        //    DisplayAttribute displayAttr = enumValue.GetAttribute<DisplayAttribute>();

        //    return displayAttr.GetName();
        //}

        public static string ToDisplayName(this Enum enumValue)
        {
            var enumMember = enumValue.GetType()
                            .GetMember(enumValue.ToString());

            DisplayAttribute displayAttrib = null;
            if (enumMember.Any())
            {
                displayAttrib = enumMember
                            .First()
                            .GetCustomAttribute<DisplayAttribute>();
            }

            return displayAttrib.GetName();
        }
    }
}
