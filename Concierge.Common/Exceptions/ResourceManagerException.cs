// <copyright file="ResourceManagerException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using System;

    using Concierge.Common.Enums;

    /// <summary>
    /// Represents an exception that is thrown when the resource manager is not generated correctly.
    /// </summary>
    public sealed class ResourceManagerException : ConciergeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerException"/> class with the specified inner exception and severity level.
        /// </summary>
        /// <param name="ex">The inner exception that caused the resource manager to fail.</param>
        /// <param name="severity">The severity level of the exception.</param>
        public ResourceManagerException(Exception ex, Severity severity = Severity.Release)
            : base("The resource manager was not generated correctly.", severity, true, ex)
        {
        }
    }
}
