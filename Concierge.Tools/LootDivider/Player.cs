// <copyright file="Player.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.LootDivider
{
    /// <summary>
    /// Represents a sealed class for managing a player with an associated name and reward pool.
    /// </summary>
    public sealed class Player : RewardPool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class with a specified name.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        public Player(string name)
            : base()
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the player.
        /// </summary>
        public string Name { get; init; }
    }
}
