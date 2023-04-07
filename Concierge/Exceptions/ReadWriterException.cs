// <copyright file="ReadWriterException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    using System;
    using System.IO;

    using Concierge.Exceptions.Enums;

    public sealed class ReadWriterException : ConciergeException
    {
        public ReadWriterException(string fileName, Exception innerException)
            : base($"Unable to load '{Path.GetFileName(fileName)}'. {innerException.Message}", Severity.Release, false, innerException)
        {
        }
    }
}
