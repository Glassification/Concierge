// <copyright file="EvaluationException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using Concierge.Common.Enums;

    /// <summary>
    /// Represents an exception that is thrown when there is nothing to evaluate.
    /// </summary>
    public sealed class EvaluationException : ConciergeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationException"/> class with the specified severity level.
        /// </summary>
        /// <param name="severity">The severity level of the exception. Defaults to Severity.Release.</param>
        public EvaluationException(Severity severity = Severity.Release)
            : base("No valid expression was detected to evaluate.", severity, false)
        {
        }
    }
}
