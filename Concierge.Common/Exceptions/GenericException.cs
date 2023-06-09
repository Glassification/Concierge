// <copyright file="GenericException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using System;

    using Concierge.Common.Enums;

    /// <summary>
    /// Represents a generic exception that occurred within the application.
    /// </summary>
    public sealed class GenericException : ConciergeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericException"/> class with the specified exception, severity level, and optional inner exception.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="severity">The severity level of the exception. Defaults to Severity.Release.</param>
        public GenericException(Exception ex, Severity severity = Severity.Release)
            : base($"A generic error occurred: {ex.Message}", severity, false, ex)
        {
        }
    }
}
