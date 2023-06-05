// <copyright file="IErrorService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System;

    public interface IErrorService
    {
        void LogError(Exception ex);
    }
}
