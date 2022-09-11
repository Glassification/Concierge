// <copyright file="ObjectUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Utility.Utilities
{
    using System;

    public static class ObjectUtility
    {
        public static T ConvertToType<T>(object value)
        {
            var targetType = typeof(T);
            var conversionType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            return (T)Convert.ChangeType(value, conversionType);
        }
    }
}
