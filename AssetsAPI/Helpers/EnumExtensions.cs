using AssetsAPI.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace AssetsAPI.Helpers
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the DisplayName attribute value of the given enum value
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <param name="value">Enum value of type 'T'</param>
        /// <returns></returns>
        public static string GetDisplayAttribute(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?.Name;
        }

        public static Enum GetEnumFromDisplayValue<T>(string displayValue)
            where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList().Find(x => x.GetDisplayAttribute() == displayValue);
        }
    }
}
