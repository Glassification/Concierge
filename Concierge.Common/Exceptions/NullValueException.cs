// <copyright file="NullValueException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using Concierge.Common.Enums;

    public sealed class NullValueException : ConciergeException
    {
        public NullValueException(string name, Severity severity = Severity.Release)
            : base($"'{name}' is null.", severity, false)
        {
        }
    }
}
