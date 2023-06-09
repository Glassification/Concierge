// <copyright file="ReadWriterException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using System;
    using System.IO;

    using Concierge.Common.Enums;

    /// <summary>
    /// Represents an exception that is thrown when a read or write operation encounters an error.
    /// </summary>
    public sealed class ReadWriterException : ConciergeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadWriterException"/> class with the specified file name and inner exception.
        /// </summary>
        /// <param name="fileName">The name of the file being read or written.</param>
        /// <param name="innerException">The inner exception that caused the read or write operation to fail.</param>
        public ReadWriterException(string fileName, Exception innerException)
            : base($"Unable to load '{Path.GetFileName(fileName)}'. {innerException.Message}", Severity.Release, false, innerException)
        {
        }
    }
}
