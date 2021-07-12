// <copyright file="XmlMissingAttributeException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    public class XmlMissingAttributeException : ConciergeException
    {
        public XmlMissingAttributeException(string attributeName, string fileName, int fileLine)
            : base(
                  string.Format(
                      "While processing file '{0}' near line {1}, the xml attribute '{2}' could not be found.",
                      fileName,
                      fileLine,
                      attributeName))
        {
        }
    }
}
