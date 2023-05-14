// <copyright file="UnhandledException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using System;

    using Concierge.Common.Enums;

    public sealed class UnhandledException : ConciergeException
    {
        public UnhandledException(Exception exception, Severity severity = Severity.Release)
            : base($"An unhandled exception occurred '{exception.Message}'", severity, true, exception)
        {
        }
    }
}
