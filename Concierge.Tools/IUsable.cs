﻿// <copyright file="IUsable.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    /// <summary>
    /// Represents an interface for items that can be used.
    /// </summary>
    public interface IUsable
    {
        UsedItem Use(UseItem useItem);
    }
}
