// <copyright file="InvalidValueException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    using Concierge.Exceptions.Enums;

    public class InvalidValueException : ConciergeException
    {
        public InvalidValueException(string name, Severity severity = Severity.Release)
            : base($"'{name}' is an invalid argument.", severity, false)
        {
        }
    }
}
