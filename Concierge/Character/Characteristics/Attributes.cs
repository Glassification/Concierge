// <copyright file="Attributes.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using Concierge.Common;

    public sealed class Attributes : ICopyable<Attributes>
    {
        private int strength;
        private int dexterity;
        private int constitution;
        private int intelligence;
        private int wisdom;
        private int charisma;

        public Attributes()
        {
            this.strength = 10;
            this.dexterity = 10;
            this.constitution = 10;
            this.intelligence = 10;
            this.wisdom = 10;
            this.charisma = 10;
        }

        public int Strength
        {
            get => this.strength;
            set => this.strength = Truncate(value);
        }

        public int Dexterity
        {
            get => this.dexterity;
            set => this.dexterity = Truncate(value);
        }

        public int Constitution
        {
            get => this.constitution;
            set => this.constitution = Truncate(value);
        }

        public int Intelligence
        {
            get => this.intelligence;
            set => this.intelligence = Truncate(value);
        }

        public int Wisdom
        {
            get => this.wisdom;
            set => this.wisdom = Truncate(value);
        }

        public int Charisma
        {
            get => this.charisma;
            set => this.charisma = Truncate(value);
        }

        public Attributes DeepCopy()
        {
            return new Attributes()
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
            if (value < Constants.MinScore)
            {
                return Constants.MinScore;
            }
            else if (value > Constants.MaxScore)
            {
                return Constants.MaxScore;
            }

            return value;
        }
    }
}
