// <copyright file="IUsable.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    public interface IUsable
    {
        UsedItem Use(IUsable? usableItem = null);
    }
}
