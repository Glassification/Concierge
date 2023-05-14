// <copyright file="Player.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.LootDivider
{
    public sealed class Player : RewardPool
    {
        public Player(string name)
            : base()
        {
            this.Name = name;
        }

        public string Name { get; init; }
    }
}
