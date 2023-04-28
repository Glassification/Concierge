// <copyright file="ExceptionExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System;

    using Concierge.Common.Exceptions;

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
