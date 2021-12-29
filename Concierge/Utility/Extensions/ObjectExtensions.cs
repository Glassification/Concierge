// <copyright file="ObjectExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public static class ObjectExtensions
    {
        private static int Depth { get; set; }

        public static bool IsList(this object value)
        {
            if (value == null)
            {
                return false;
            }

            return value is IList &&
                   value.GetType().IsGenericType &&
                   value.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

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

        public static void SetProperties<T>(this object originalItem, object itemToCopy)
        {
            Depth = 0;
            if (originalItem == null)
            {
                return;
            }

            SetPropertiesHelper<T>(itemToCopy, originalItem);
        }

        private static void SetPropertiesHelper<T>(object item, object originalItem)
        {
            Depth++;

            var properties = item.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanWrite && property.CanRead)
                {
                    var propertyValue = property.GetValue(item);
                    var isCopyable = propertyValue
                        .GetType()
                        .GetInterfaces()
                        .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICopyable<>));

                    if (isCopyable && Depth < Constants.MaxDepth)
                    {
                        SetPropertiesHelper<T>(propertyValue, property.GetValue(originalItem));
                    }
                    else
                    {
                        property.SetValue(originalItem, propertyValue);
                    }
                }
            }

            Depth--;
        }
    }
}
