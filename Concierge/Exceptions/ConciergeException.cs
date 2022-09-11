// <copyright file="ConciergeException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    using System;

    using Concierge.Exceptions.Enums;

    public abstract class ConciergeException : Exception
    {
        public ConciergeException(string message, Severity severity, bool isFatal, Exception? innerException = null)
            : base(message, innerException)
        {
            this.Severity = severity;
            this.IsFatal = isFatal;
        }

        public bool IsFatal { get; init; }

        public Severity Severity { get; }
    }
}
