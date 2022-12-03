// <copyright file="GitException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    using System;

    using Concierge.Exceptions.Enums;

    public sealed class GitException : ConciergeException
    {
        public GitException(Exception ex)
            : base("An error occurred while reading form Git.", Severity.Debug, false, ex)
        {
        }
    }
}
