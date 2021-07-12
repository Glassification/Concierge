// <copyright file="ObjectExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System;
    using System.ComponentModel;

    public static class ObjectExtensions
    {
        public static T ConvertTo<T>(this object value)
        {
            T returnValue;

            if (value is T variable)
            {
                returnValue = variable;
            }
            else
            {
                try
                {
                    if (Nullable.GetUnderlyingType(typeof(T)) != null)
                    {
                        TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                        returnValue = (T)conv.ConvertFrom(value);
                    }
                    else
                    {
                        returnValue = (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch (Exception)
                {
                    returnValue = default;
                }
            }

            return returnValue;
        }
    }
}
