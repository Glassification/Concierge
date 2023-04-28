// <copyright file="EnumExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    public static class EnumExtensions
    {
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
