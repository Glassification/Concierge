// <copyright file="IConciergePage.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces
{
    using Concierge.Interfaces.Enums;

    public interface IConciergePage
    {
        ConciergePage ConciergePage { get; }

        void Draw();
    }
}
