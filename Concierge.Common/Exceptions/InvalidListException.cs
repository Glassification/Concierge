// <copyright file="InvalidListException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using Concierge.Common.Enums;

    /// <summary>
    /// Represents an exception that is thrown when a list has invalid elements.
    /// </summary>
    public sealed class InvalidListException : ConciergeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidListException"/> class with the specified severity level.
        /// </summary>
        /// <param name="listMessage">What is invalid about the list.</param>
        /// <param name="severity">The severity level of the exception. Defaults to Severity.Release.</param>
        public InvalidListException(string listMessage, Severity severity = Severity.Release)
            : base(listMessage, severity, false)
        {
        }
    }
}
