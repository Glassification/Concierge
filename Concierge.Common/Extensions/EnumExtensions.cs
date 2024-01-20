// <copyright file="EnumExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

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
        /// <exception cref="ArgumentException">
        /// Thrown if <param name="e"/> is not an enumerated type.
        /// </exception>
        public static string GetDescription<T>(this T e)
            where T : IConvertible
        {
            if (e is not Enum)
            {
                throw new ArgumentException("e must be an enumerated type");
            }

            var type = e.GetType();
            var values = Enum.GetValues(type);

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

        /// <summary>
        /// Determines the visibility based on whether a specified flag is set in the given enumerated value.
        /// </summary>
        /// <typeparam name="T">The enumerated type.</typeparam>
        /// <param name="e">The enumerated value to check for the presence of the flag.</param>
        /// <param name="flag">The flag to check for in the enumerated value.</param>
        /// <returns>
        /// <see cref="Visibility.Visible"/> if the specified flag is set in the enumerated value; otherwise, <see cref="Visibility.Collapsed"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if either <paramref name="e"/> or <paramref name="flag"/> is not an enumerated type.
        /// </exception>
        public static Visibility HasFlagVisibility<T>(this T e, T flag)
            where T : IConvertible
        {
            if (e is not Enum en || flag is not Enum eFlag)
            {
                throw new ArgumentException("e must be an enumerated type");
            }

            return en.HasFlag(eFlag) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
