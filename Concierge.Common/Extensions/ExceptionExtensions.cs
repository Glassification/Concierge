// <copyright file="ExceptionExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System;

    using Concierge.Common.Exceptions;

    /// <summary>
    /// Provides extension methods for working with exceptions.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Tries to convert the exception to a ReadWriterException, providing additional information about the file name.
        /// </summary>
        /// <param name="exception">The exception to convert.</param>
        /// <param name="fileName">The name of the file associated with the exception.</param>
        /// <returns>
        /// If the exception is already a ConciergeException, it is returned as is.
        /// Otherwise, a new ReadWriterException is created with the specified file name and the original exception as the inner exception.
        /// </returns>
        public static Exception TryConvertToReadWriterException(this Exception exception, string fileName)
        {
            if (exception is ConciergeException)
            {
                return exception;
            }

            return new ReadWriterException(fileName, exception);
        }
    }
}
