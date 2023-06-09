// <copyright file="EnumExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for working with enumerations.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Retrieves the description attribute value of the specified enumeration value.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <param name="e">The enumeration value.</param>
        /// <returns>The description attribute value of the enumeration value, or an empty string if not found.</returns>
        /// <remarks>
        /// This method retrieves the description attribute value associated with the specified enumeration value.
        /// It can be used to provide human-readable descriptions for enumeration values.
        /// </remarks>
        public static string GetDescription<T>(this T e)
            where T : IConvertible
        {
            if (e is not Enum)
            {
                return string.Empty;
            }

            Type type = e.GetType();
            Array values = Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val == e.ToInt32(CultureInfo.InvariantCulture))
                {
                    var memInfo = type.GetMember(type.GetEnumName(val) ?? string.Empty);
                    if (memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                    {
                        return descriptionAttribute.Description;
                    }
                }
            }

            return string.Empty;
        }
    }
}
