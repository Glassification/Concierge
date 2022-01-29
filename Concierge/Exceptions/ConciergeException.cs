// <copyright file="ConciergeException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    using System;

    using Concierge.Exceptions.Enums;

    public abstract class ConciergeException : Exception
    {
        public ConciergeException(string message, Severity severity, Exception? innerException = null)
            : base(message, innerException)
        {
            this.Severity = severity;
        }

        public Severity Severity { get; }
    }
}
