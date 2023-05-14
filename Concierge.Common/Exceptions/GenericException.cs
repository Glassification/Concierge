// <copyright file="GenericException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using System;

    using Concierge.Common.Enums;

    public sealed class GenericException : ConciergeException
    {
        public GenericException(Exception ex, Severity severity = Severity.Release)
            : base($"A generic error occurred: {ex.Message}", severity, false, ex)
        {
        }
    }
}
