// <copyright file="XmlTagException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    public class XmlTagException : ConciergeException
    {
        public XmlTagException(string tagName, string fileName, int fileLine)
            : base(
                  string.Format(
                      "While processing file '{0}' near line {1}, the xml tag '{2}' could not be found.",
                      fileName,
                      fileLine,
                      tagName))
        {
        }
    }
}
