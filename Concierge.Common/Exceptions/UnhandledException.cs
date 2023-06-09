// <copyright file="UnhandledException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using System;

    using Concierge.Common.Enums;

    /// <summary>
    /// Represents an exception that is thrown when an unhandled exception occurs.
    /// </summary>
    public sealed class UnhandledException : ConciergeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledException"/> class with the specified exception and severity level.
        /// </summary>
        /// <param name="exception">The unhandled exception that occurred.</param>
        /// <param name="severity">The severity level of the exception.</param>
        public UnhandledException(Exception exception, Severity severity = Severity.Release)
            : base($"An unhandled exception occurred '{exception.Message}'", severity, true, exception)
        {
        }
    }
}
