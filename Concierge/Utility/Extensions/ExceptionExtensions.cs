// <copyright file="ExceptionExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System;

    using Concierge.Exceptions;

    public static class ExceptionExtensions
    {
        public static Exception TryConvertToReadWriterException(this Exception exception, string fileName)
        {
            if (exception is ConciergeException)
            {
                return exception;
            }

            return new ReadWriterException(fileName, exception);
        }
    }
}
