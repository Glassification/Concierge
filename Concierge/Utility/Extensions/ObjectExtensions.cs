// <copyright file="ObjectExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System.Collections;
    using System.Collections.Generic;
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

        public static void SetProperties<T>(this object originalItem, object itemToCopy)
        {
            if (originalItem == null)
            {
                return;
            }

            Depth = 0;
            SetPropertiesHelper<T>(itemToCopy, originalItem);
        }

        private static void SetPropertiesHelper<T>(object item, object? originalItem)
        {
            Depth++;

            var properties = item.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanWrite && property.CanRead)
                {
                    if (property.GetValue(item) is not object propertyValue)
                    {
                        continue;
                    }

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
