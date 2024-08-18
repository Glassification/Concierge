// <copyright file="HitDice.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents the hit dice of a character, including the total and spent hit dice for each type.
    /// </summary>
    public sealed class HitDice : ICopyable<HitDice>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HitDice"/> class with default values.
        /// </summary>
        public HitDice()
        {
            this.TotalD6 = 0;
            this.TotalD8 = 0;
            this.TotalD10 = 0;
            this.TotalD12 = 0;
            this.SpentD6 = 0;
            this.SpentD8 = 0;
            this.SpentD10 = 0;
            this.SpentD12 = 0;
        }

        /// <summary>
        /// Gets or sets the total number of d6 hit dice.
        /// </summary>
        public int TotalD6 { get; set; }

        /// <summary>
        /// Gets or sets the total number of d8 hit dice.
        /// </summary>
        public int TotalD8 { get; set; }

        /// <summary>
        /// Gets or sets the total number of d10 hit dice.
        /// </summary>
        public int TotalD10 { get; set; }

        /// <summary>
        /// Gets or sets the total number of d12 hit dice.
        /// </summary>
        public int TotalD12 { get; set; }

        /// <summary>
        /// Gets or sets the number of spent d6 hit dice.
        /// </summary>
        public int SpentD6 { get; set; }

        /// <summary>
        /// Gets or sets the number of spent d8 hit dice.
        /// </summary>
        public int SpentD8 { get; set; }

        /// <summary>
        /// Gets or sets the number of spent d10 hit dice.
        /// </summary>
        public int SpentD10 { get; set; }

        /// <summary>
        /// Gets or sets the number of spent d12 hit dice.
        /// </summary>
        public int SpentD12 { get; set; }

        /// <summary>
        /// Gets the type of hit dice associated with a given class.
        /// </summary>
        /// <param name="className">The name of the class.</param>
        /// <returns>The type of hit dice associated with the class.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "Improve Readability.")]
        public static Dice GetHitDice(string className)
        {
            switch (className)
            {
                case "Sorcerer":
                case "Wizard":
                default:
                    return Dice.D6;
                case "Artificer":
                case "Bard":
                case "Cleric":
                case "Druid":
                case "Gunslinger":
                case "Monk":
                case "Rogue":
                case "Warlock":
                    return Dice.D8;
                case "Blood Hunter":
                case "Fighter":
                case "Paladin":
                case "Ranger":
                    return Dice.D10;
                case "Barbarian":
                    return Dice.D12;
            }
        }

        /// <summary>
        /// Gets the first available hit dice type based on the current state.
        /// </summary>
        /// <returns>The first available hit dice type.</returns>
        public Dice GetFirstAvailable()
        {
            if (this.TotalD6 > 0)
            {
                return Dice.D6;
            }

            if (this.TotalD8 > 0)
            {
                return Dice.D8;
            }

            if (this.TotalD10 > 0)
            {
                return Dice.D10;
            }

            if (this.TotalD12 > 0)
            {
                return Dice.D12;
            }

            return Dice.None;
        }

        /// <summary>
        /// Increments the number of spent hit dice of a certain type.
        /// </summary>
        /// <param name="name">The name of the hit dice type.</param>
        /// <returns>A tuple containing the dice type, the number of used hit dice, and the total number of hit dice.</returns>
        public (Dice dice, int used, int total) Increment(string name)
        {
            if (name.ContainsIgnoreCase("d6") && this.SpentD6 < this.TotalD6)
            {
                this.SpentD6++;
                return (Dice.D6, this.SpentD6, this.TotalD6);
            }

            if (name.ContainsIgnoreCase("d8") && this.SpentD8 < this.TotalD8)
            {
                this.SpentD8++;
                return (Dice.D8, this.SpentD8, this.TotalD8);
            }

            if (name.ContainsIgnoreCase("d10") && this.SpentD10 < this.TotalD10)
            {
                this.SpentD10++;
                return (Dice.D10, this.SpentD10, this.TotalD10);
            }

            if (name.ContainsIgnoreCase("d12") && this.SpentD12 < this.TotalD12)
            {
                this.SpentD12++;
                return (Dice.D12, this.SpentD12, this.TotalD12);
            }

            return (Dice.None, 0, 0);
        }

        /// <summary>
        /// Resets the spent hit dice back to their maximum values.
        /// </summary>
        public void RegainHitDice()
        {
            this.SpentD6 = ConciergeMath.Regain(this.SpentD6);
            this.SpentD8 = ConciergeMath.Regain(this.SpentD8);
            this.SpentD10 = ConciergeMath.Regain(this.SpentD10);
            this.SpentD12 = ConciergeMath.Regain(this.SpentD12);
        }

        /// <summary>
        /// Creates a deep copy of the hit dice object.
        /// </summary>
        /// <returns>A new instance of the <see cref="HitDice"/> class with the same property values as the original.</returns>
        public HitDice DeepCopy()
        {
            return new HitDice()
            {
                TotalD6 = this.TotalD6,
                TotalD8 = this.TotalD8,
                TotalD10 = this.TotalD10,
                TotalD12 = this.TotalD12,
                SpentD6 = this.SpentD6,
                SpentD8 = this.SpentD8,
                SpentD10 = this.SpentD10,
                SpentD12 = this.SpentD12,
            };
        }
    }
}
