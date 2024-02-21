// <copyright file="HitDice.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using Concierge.Common;
    using Concierge.Common.Enums;

    public sealed class HitDice : ICopyable<HitDice>
    {
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

        public int TotalD6 { get; set; }

        public int TotalD8 { get; set; }

        public int TotalD10 { get; set; }

        public int TotalD12 { get; set; }

        public int SpentD6 { get; set; }

        public int SpentD8 { get; set; }

        public int SpentD10 { get; set; }

        public int SpentD12 { get; set; }

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

        public (Dice dice, int used, int total) Increment(string name)
        {
            if (name.Contains("d6", System.StringComparison.InvariantCultureIgnoreCase) && this.SpentD6 < this.TotalD6)
            {
                this.SpentD6++;
                return (Dice.D6, this.SpentD6, this.TotalD6);
            }

            if (name.Contains("d8", System.StringComparison.InvariantCultureIgnoreCase) && this.SpentD8 < this.TotalD8)
            {
                this.SpentD8++;
                return (Dice.D8, this.SpentD8, this.TotalD8);
            }

            if (name.Contains("d10", System.StringComparison.InvariantCultureIgnoreCase) && this.SpentD10 < this.TotalD10)
            {
                this.SpentD10++;
                return (Dice.D10, this.SpentD10, this.TotalD10);
            }

            if (name.Contains("d12", System.StringComparison.InvariantCultureIgnoreCase) && this.SpentD12 < this.TotalD12)
            {
                this.SpentD12++;
                return (Dice.D12, this.SpentD12, this.TotalD12);
            }

            return (Dice.None, 0, 0);
        }

        public void RegainHitDice()
        {
            this.SpentD6 = Constants.Regain(this.SpentD6);
            this.SpentD8 = Constants.Regain(this.SpentD8);
            this.SpentD10 = Constants.Regain(this.SpentD10);
            this.SpentD12 = Constants.Regain(this.SpentD12);
        }

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
