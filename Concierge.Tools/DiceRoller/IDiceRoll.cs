// <copyright file="IDiceRoll.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.DiceRoller
{
    public interface IDiceRoll
    {
        string Dice { get; }

        int Total { get; }

        void ReRoll();
    }
}
