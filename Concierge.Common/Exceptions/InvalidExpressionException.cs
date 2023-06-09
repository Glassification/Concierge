// <copyright file="InvalidExpressionException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using Concierge.Common.Enums;

    /// <summary>
    /// Represents an exception that is thrown when an expression is invalid.
    /// </summary>
    public sealed class InvalidExpressionException : ConciergeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidExpressionException"/> class with the specified expression and severity level.
        /// </summary>
        /// <param name="expression">The invalid expression.</param>
        /// <param name="severity">The severity level of the exception. Defaults to Severity.Release.</param>
        public InvalidExpressionException(string expression, Severity severity = Severity.Release)
            : base($"'{expression}' is an invalid expression.", severity, false)
        {
        }
    }
}
