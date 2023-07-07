// <copyright file="ObjectExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Concierge.Common.Attributes;

    /// <summary>
    /// Provides extension methods for working with objects.
    /// </summary>
    public static class ObjectExtensions
    {
        private static int Depth { get; set; }

        /// <summary>
        /// Checks if the value's type is assignable to the specified type.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="type">The type to compare against.</param>
        /// <returns><c>true</c> if the value's type is assignable to the specified type; otherwise, <c>false</c>.</returns>
        public static bool IsTypeOf(this object value, Type type)
        {
            return value
                .GetType()
                .GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == type);
        }

        /// <summary>
        /// Checks if the object is a generic list.
        /// </summary>
        /// <param name="value">The object to check.</param>
        /// <returns><c>true</c> if the object is a generic list; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Copies the properties of the <paramref name="itemToCopy"/> object to the <paramref name="originalItem"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the original item.</typeparam>
        /// <param name="originalItem">The original item to copy the properties to.</param>
        /// <param name="itemToCopy">The item containing the properties to be copied.</param>
        public static void SetProperties<T>(this object originalItem, object itemToCopy)
        {
            if (originalItem == null)
            {
                return;
            }

            Depth = 0;
            SetPropertiesHelper<T>(itemToCopy, originalItem);
        }

        /// <summary>
        /// Gets the value of a property from an object.
        /// </summary>
        /// <param name="item">The object to get the property value from.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the property as a string, or an empty string if the property is null.</returns>
        public static string GetProperty(this object item, string name)
        {
            var properties = item.GetType().GetProperties();
            var property = properties.Where(x => x.Name.Equals(name)).FirstOrDefault();

            return property?.GetValue(item)?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Searches for a specific string value in the properties of an object.
        /// </summary>
        /// <param name="item">The object to search within.</param>
        /// <param name="filter">The string value to search for.</param>
        /// <returns><c>true</c> if the filter string is found in any of the properties of the object; otherwise, <c>false</c>.</returns>
        public static bool SearchObject(this object item, string filter)
        {
            try
            {
                var properties = item.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(item);
                    if (propertyValue is null || HasIgnoreAttribute(property))
                    {
                        continue;
                    }

                    if (propertyValue?.ToString()?.Contains(filter, StringComparison.InvariantCultureIgnoreCase) ?? false)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        /// <summary>
        /// Checks if a property has the <paramref name="SearchIgnore"/> attribute.
        /// </summary>
        /// <param name="propertyInfo">The object representing the property to check.</param>
        /// <returns><c>true</c> if the property has the <paramref name="SearchIgnore"/> attribute; otherwise, <c>false</c>.</returns>
        private static bool HasIgnoreAttribute(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute(typeof(SearchIgnore)) is not null;
        }

        /// <summary>
        ///  Recursively sets the properties of the <paramref name="originalItem"/> object with the corresponding values from the <paramref name="item"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the original item.</typeparam>
        /// <param name="item">The object containing the properties to be copied.</param>
        /// <param name="originalItem">The original item to copy the properties to.</param>
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
