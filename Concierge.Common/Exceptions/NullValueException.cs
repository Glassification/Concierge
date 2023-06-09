// <copyright file="NullValueException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using Concierge.Common.Enums;

    /// <summary>
    /// Represents an exception that is thrown when a null value is encountered.
    /// </summary>
    public sealed class NullValueException : ConciergeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullValueException"/> class with the specified name and severity level.
        /// </summary>
        /// <param name="name">The name of the null value.</param>
        /// <param name="severity">The severity level of the exception. Defaults to Severity.Release.</param>
        public NullValueException(string name, Severity severity = Severity.Release)
            : base($"'{name}' is null.", severity, false)
        {
        }
    }
}
