// <copyright file="CompanionAttributes.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Companions
{
    using System;

    using Concierge.Common;

    /// <summary>
    /// Represents the attributes of a companion character.
    /// </summary>
    public sealed class CompanionAttributes : ICopyable<CompanionAttributes>
    {
        /// <summary>
        /// The default score for attributes.
        /// </summary>
        public const int DefaultScore = 10;

        private int strength;
        private int dexterity;
        private int constitution;
        private int intelligence;
        private int wisdom;
        private int charisma;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanionAttributes"/> class with default attribute scores.
        /// </summary>
        public CompanionAttributes()
        {
            this.strength = DefaultScore;
            this.dexterity = DefaultScore;
            this.constitution = DefaultScore;
            this.intelligence = DefaultScore;
            this.wisdom = DefaultScore;
            this.charisma = DefaultScore;
        }

        /// <summary>
        /// Gets or sets the strength attribute of the companion.
        /// </summary>
        public int Strength
        {
            get => this.strength;
            set => this.strength = Truncate(value);
        }

        /// <summary>
        /// Gets or sets the dexterity attribute of the companion.
        /// </summary>
        public int Dexterity
        {
            get => this.dexterity;
            set => this.dexterity = Truncate(value);
        }

        /// <summary>
        /// Gets or sets the constitution attribute of the companion.
        /// </summary>
        public int Constitution
        {
            get => this.constitution;
            set => this.constitution = Truncate(value);
        }

        /// <summary>
        /// Gets or sets the intelligence attribute of the companion.
        /// </summary>
        public int Intelligence
        {
            get => this.intelligence;
            set => this.intelligence = Truncate(value);
        }

        /// <summary>
        /// Gets or sets the wisdom attribute of the companion.
        /// </summary>
        public int Wisdom
        {
            get => this.wisdom;
            set => this.wisdom = Truncate(value);
        }

        /// <summary>
        /// Gets or sets the charisma attribute of the companion.
        /// </summary>
        public int Charisma
        {
            get => this.charisma;
            set => this.charisma = Truncate(value);
        }

        /// <summary>
        /// Creates a deep copy of the companion attributes instance.
        /// </summary>
        /// <returns>A deep copy of the <see cref="CompanionAttributes"/> instance.</returns>
        public CompanionAttributes DeepCopy()
        {
            return new CompanionAttributes()
            {
                Strength = this.Strength,
                Dexterity = this.Dexterity,
                Constitution = this.Constitution,
                Intelligence = this.Intelligence,
                Wisdom = this.Wisdom,
                Charisma = this.Charisma,
            };
        }

        private static int Truncate(int value)
        {
            return Math.Min(Constants.MaxScore, Math.Max(Constants.MinScore, value));
        }
    }
}
