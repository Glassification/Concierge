// <copyright file="XmlAttributeException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    public class XmlAttributeException : ConciergeException
    {
        public XmlAttributeException(string attributeName, string fileName, int fileLine)
            : base(
                  string.Format(
                      "While processing file '{0}' near line {1}, the xml attribute '{2}' is null or empty.",
                      fileName,
                      fileLine,
                      attributeName))
        {
        }
    }
}
