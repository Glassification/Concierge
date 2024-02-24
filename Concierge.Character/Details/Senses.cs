// <copyright file="Senses.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Details
{
    using Concierge.Character.Enums;
    using Concierge.Common;

    /// <summary>
    /// Represents the senses and perception attributes of a character.
    /// </summary>
    public sealed class Senses : ICopyable<Senses>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Senses"/> class.
        /// </summary>
        public Senses()
        {
            this.InitiativeBonus = 0;
            this.PerceptionBonus = 0;
            this.MovementBonus = 0;
            this.BaseMovement = 30;
            this.Vision = VisionTypes.Normal;
        }

        /// <summary>
        /// Gets or sets the bonus to initiative checks.
        /// </summary>
        public int InitiativeBonus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the character has inspiration.
        /// </summary>
        public bool Inspiration { get; set; }

        /// <summary>
        /// Gets or sets the bonus to perception checks.
        /// </summary>
        public int PerceptionBonus { get; set; }

        /// <summary>
        /// Gets or sets the base movement speed.
        /// </summary>
        public int BaseMovement { get; set; }

        /// <summary>
        /// Gets or sets the bonus to movement speed.
        /// </summary>
        public int MovementBonus { get; set; }

        /// <summary>
        /// Gets or sets the type of vision the character has.
        /// </summary>
        public VisionTypes Vision { get; set; }

        /// <summary>
        /// Creates a deep copy of the <see cref="Senses"/> object.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Senses"/> object.</returns>
        public Senses DeepCopy()
        {
            return new Senses()
            {
                InitiativeBonus = this.InitiativeBonus,
                Inspiration = this.Inspiration,
                PerceptionBonus = this.PerceptionBonus,
                MovementBonus = this.MovementBonus,
                BaseMovement = this.BaseMovement,
                Vision = this.Vision,
            };
        }
    }
}
