// <copyright file="ResourceManagerException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Exceptions
{
    using System;

    using Concierge.Exceptions;
    using Concierge.Exceptions.Enums;

    public class ResourceManagerException : ConciergeException
    {
        public ResourceManagerException(Exception ex, Severity severity = Severity.Release)
            : base("The resource manager was not generated correctly.", severity, true, ex)
        {
        }
    }
}
