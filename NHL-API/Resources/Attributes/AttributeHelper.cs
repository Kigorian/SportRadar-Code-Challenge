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
    }
}
