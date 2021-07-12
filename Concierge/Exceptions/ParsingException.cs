// <copyright file="ParsingException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    public class ParsingException : ConciergeException
    {
        public ParsingException(string value, string fileName, int fileLine)
            : base(
                  string.Format(
                      "File '{0}' near line {1} has non numerical string '{2}' which must be replaced with a number.",
                      fileName,
                      fileLine,
                      value))
        {
        }
    }
}
