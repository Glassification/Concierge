// <copyright file="ConciergeException.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Exceptions
{
    using System;

    public class ConciergeException : Exception
    {
        public ConciergeException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}
