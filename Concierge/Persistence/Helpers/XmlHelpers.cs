// <copyright file="XmlHelpers.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Xml;
    using System.Xml.Linq;

    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public class XmlHelpers
    {
        public XmlHelpers(string fileName)
        {
            this.CultureInfo = Thread.CurrentThread.CurrentCulture;
            this.FileName = fileName;
        }

        public CultureInfo CultureInfo { get; private set; }

        private string FileName { get; }

        public T GetAttribute<T>(XElement element, string attributeName, bool returnDefault = false)
        {
            string strElement;

            try
            {
                Guard.HasAttribute(strElement = (string)element.Attribute(attributeName), attributeName, this.FileName, ((IXmlLineInfo)element).LineNumber);
                var returnValue = typeof(ObjectExtensions).GetMethod("ConvertTo").MakeGenericMethod(typeof(T)).Invoke(this, new object[] { strElement });

                return (T)returnValue;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public T GetElementAndParse<T>(string tagName, XElement element)
        {
            string strElement;

            var innerElem = element.Element(tagName);
            Guard.HasTag(strElement = innerElem?.Value, tagName, this.FileName, ((IXmlLineInfo)element).LineNumber);

            var returnValue = typeof(ObjectExtensions).GetMethod("ConvertTo").MakeGenericMethod(typeof(T)).Invoke(this, new object[] { strElement });

            return (T)returnValue;
        }

        public XElement GetElement(string tagName, XElement element)
        {
            var innerElem = element.Element(tagName);

            Guard.HasTag(innerElem?.Value, tagName, this.FileName, ((IXmlLineInfo)element).LineNumber);

            return innerElem;
        }

        public IEnumerable<XElement> GetElements(string tagName, XElement element)
        {
            var innerElems = element.Elements(tagName);

            Guard.HasTag(innerElems, tagName, this.FileName, ((IXmlLineInfo)element).LineNumber);

            return innerElems;
        }

        public T GetGetAttributeFromElement<T>(XElement rootElement, string elementName, string attributeName, bool returnDefault = false)
        {
            string strElement;

            try
            {
                var innerElem = rootElement.Element(elementName);
                Guard.HasTag(innerElem, elementName, this.FileName, ((IXmlLineInfo)rootElement).LineNumber);

                Guard.HasAttribute(strElement = (string)innerElem.Attribute(attributeName), attributeName, this.FileName, ((IXmlLineInfo)innerElem).LineNumber);

                var returnValue = typeof(ObjectExtensions).GetMethod("ConvertTo").MakeGenericMethod(typeof(T)).Invoke(this, new object[] { strElement });

                return (T)returnValue;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
    }
}
