// <copyright file="EnumUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides utility methods for working with enumerations.
    /// </summary>
    public static class EnumUtility
    {
        /// <summary>
        /// Retrieves all values of the specified enumeration type.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <returns>An enumerable collection of all values of the specified enumeration type.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified type is not an enumerated type.</exception>
        public static IEnumerable<T> GetValues<T>()
            where T : struct, IConvertible
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return Enum.GetValues(enumType).Cast<T>();
        }
    }
}
