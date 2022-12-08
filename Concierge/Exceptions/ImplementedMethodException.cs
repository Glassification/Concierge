// <copyright file="ImplementedMethodException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    using Concierge.Exceptions.Enums;

    public class ImplementedMethodException : ConciergeException
    {
        public ImplementedMethodException(string methodName, object? item = null)
            : base($"No implemented {methodName} method{(item is null ? "." : $" for {item}.")}", Severity.Debug, false)
        {
        }
    }
}
