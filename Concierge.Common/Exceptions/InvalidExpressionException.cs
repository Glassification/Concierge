// <copyright file="InvalidExpressionException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using Concierge.Common.Enums;

    public sealed class InvalidExpressionException : ConciergeException
    {
        public InvalidExpressionException(string expression, Severity severity = Severity.Release)
            : base($"'{expression}' is an invalid expression.", severity, false)
        {
        }
    }
}
