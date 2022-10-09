// <copyright file="UnhandledException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    using System;

    using Concierge.Exceptions.Enums;

    public sealed class UnhandledException : ConciergeException
    {
        public UnhandledException(Exception exception, Severity severity = Severity.Release)
            : base($"An unhandled exception occurred '{exception.Message}'", severity, true, exception)
        {
        }
    }
}
