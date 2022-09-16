// <copyright file="EvaluationException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    using Concierge.Exceptions.Enums;

    public sealed class EvaluationException : ConciergeException
    {
        public EvaluationException(Severity severity = Severity.Release)
            : base("There is nothing to evaluate.", severity, false)
        {
        }
    }
}
