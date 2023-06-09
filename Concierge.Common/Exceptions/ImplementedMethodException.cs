// <copyright file="ImplementedMethodException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using Concierge.Common.Enums;

    /// <summary>
    /// Represents an exception that is thrown when a method is expected to be implemented but is not.
    /// </summary>
    public class ImplementedMethodException : ConciergeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImplementedMethodException"/> class with the specified method name and optional item.
        /// </summary>
        /// <param name="methodName">The name of the method that is not implemented.</param>
        /// <param name="item">An optional object associated with the method. Defaults to null.</param>
        public ImplementedMethodException(string methodName, object? item = null)
            : base($"No implemented {methodName} method{(item is null ? "." : $" for {item}.")}", Severity.Debug, false)
        {
        }
    }
}
