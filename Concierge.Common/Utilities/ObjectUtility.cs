// <copyright file="ObjectUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Utilities
{
    using System;

    /// <summary>
    /// Provides utility methods for object-related operations.
    /// </summary>
    public static class ObjectUtility
    {
        /// <summary>
        /// Converts the provided value to the specified target type.
        /// </summary>
        /// <typeparam name="T">The target type to which the value should be converted.</typeparam>
        /// <param name="value">The value to be converted.</param>
        /// <returns>The converted value of the specified target type.</returns>
        public static T ConvertToType<T>(object value)
        {
            var targetType = typeof(T);
            var conversionType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            return (T)Convert.ChangeType(value, conversionType);
        }
    }
}
