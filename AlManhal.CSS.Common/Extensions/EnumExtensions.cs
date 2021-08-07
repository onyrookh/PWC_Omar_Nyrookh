using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return enumValue.ToString();
        }

        /// <summary>
		/// Gets the localized string for an enum value. 
		/// Format of the resource key: "EnumType.EnumValue".
		/// <example>
		///	For enum value <c>DayOfWeek.Sunday</c>, the resource key will be "DayOfWeek.Sunday"
		/// </example>
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="resourceType">Type of the resource. If no resource specified, tries to find [LocalizationEnum]</param>
		/// <param name="value">The enum value.</param>
		/// <returns></returns>
		public static string GetLocalizedString(this Enum value, Type resourceType = null)
        {
            // if no resource specified, tries to find [LocalizationEnum]
            if (resourceType == null)
                resourceType = EnumHelper.GetResourceType(value.GetType());
            return EnumHelper.GetLocalizedString(resourceType, value);
        }
    }
}
