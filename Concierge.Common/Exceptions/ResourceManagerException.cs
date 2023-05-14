// <copyright file="ResourceManagerException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Exceptions
{
    using System;

    using Concierge.Common.Enums;

    public sealed class ResourceManagerException : ConciergeException
    {
        public ResourceManagerException(Exception ex, Severity severity = Severity.Release)
            : base("The resource manager was not generated correctly.", severity, true, ex)
        {
        }
    }
}
