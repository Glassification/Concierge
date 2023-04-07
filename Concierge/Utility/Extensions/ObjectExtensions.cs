// <copyright file="ObjectExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Concierge.Utility.Attributes;

    public static class ObjectExtensions
    {
        private static int Depth { get; set; }

        public static bool IsTypeOf(this object value, Type type)
        {
            return value
                .GetType()
                .GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == type);
        }

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

        public static string GetProperty(this object item, string name)
        {
            var properties = item.GetType().GetProperties();
            var property = properties.Where(x => x.Name.Equals(name)).FirstOrDefault();

            return property?.GetValue(item)?.ToString() ?? string.Empty;
        }

        public static bool SearchObject(this object item, string filter)
        {
            try
            {
                var properties = item.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(item);
                    if (propertyValue is null || HasIgnoreProperty(property))
                    {
                        continue;
                    }

                    if (propertyValue?.ToString()?.Contains(filter, StringComparison.InvariantCultureIgnoreCase) ?? false)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }

            return false;
        }

        private static bool HasIgnoreProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute(typeof(SearchIgnore)) is not null;
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

                    if (propertyValue.IsTypeOf(typeof(ICopyable<>)) && Depth < Constants.MaxDepth)
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
