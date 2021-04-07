using System;
using System.Collections.Generic;
using System.Linq;

namespace NHL_API.Resources.Attributes
{
    public static class AttributeHelper
    {
        /// <summary>
        /// Gets an Enum's Description based on the Description attribute.
        /// If there isn't a Description attribute, returns the enumValue's name.
        /// </summary>
        public static string GetDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = (IEnumerable<DescriptionAttribute>)fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Select(a => a.Description).SingleOrDefault() ?? enumValue.ToString();
        }

        /// <summary>
        /// Gets the enum that matches the given description.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Game type not found for ID.", nameof(description));
        }
    }
}
