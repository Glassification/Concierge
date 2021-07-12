// <copyright file="Guard.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;

    using Concierge.Exceptions;
    using Concierge.Utility.Extensions;

    public static class Guard
    {
        /// =========================================
        /// IsNull()
        /// =========================================
        public static void IsNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// =========================================
        /// IsNullOrEmpty()
        /// =========================================
        public static void IsNullOrEmpty(string value, string name)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentNullException(name);
            }
        }

        /// =========================================
        /// IsNullOrWhiteSpace()
        /// =========================================
        public static void IsNullOrWhiteSpace(string value, string name)
        {
            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(name);
            }
        }

        /// =========================================
        /// HasTag()
        /// =========================================
        public static void HasTag(object value, string tagName, string fileName, int fileLine)
        {
            if (value == null)
            {
                throw new XmlTagException(tagName, fileName, fileLine);
            }
        }

        /// =========================================
        /// HasAttribute()
        /// =========================================
        public static void HasAttribute(object value, string attributeName, string fileName, int fileLine, bool onlyCheckNull = false)
        {
            if (value == null)
            {
                throw new XmlMissingAttributeException(attributeName, fileName, fileLine);
            }
            else if (value is string strValue)
            {
                if (!onlyCheckNull && strValue.IsNullOrWhiteSpace())
                {
                    throw new XmlAttributeException(attributeName, fileName, fileLine);
                }
            }
        }

        /// =========================================
        /// CanParse()
        /// =========================================
        public static void CanParse(bool result, string value, string fileName, int fileLine)
        {
            if (!result)
            {
                throw new ParsingException(value, fileName, fileLine);
            }
        }
    }
}
