// <copyright file="InvalidValueException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using Concierge.Common.Enums;

    /// <summary>
    /// Represents an exception that is thrown when an invalid value is encountered.
    /// </summary>
    public sealed class InvalidValueException : ConciergeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidValueException"/> class with the specified name and severity level.
        /// </summary>
        /// <param name="name">The name of the invalid argument.</param>
        /// <param name="severity">The severity level of the exception. Defaults to Severity.Release.</param>
        public InvalidValueException(string name, Severity severity = Severity.Release)
            : base($"'{name}' is an invalid argument.", severity, false)
        {
        }
    }
}
