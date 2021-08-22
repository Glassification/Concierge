// <copyright file="Attributes.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using Concierge.Utility;

    public class Attributes
    {
        private int strength;
        private int dexterity;
        private int constitution;
        private int intelligence;
        private int wisdom;
        private int charisma;

        public Attributes()
        {
            this.strength = 0;
            this.dexterity = 0;
            this.constitution = 0;
            this.intelligence = 0;
            this.wisdom = 0;
            this.charisma = 0;
        }

        public int Strength
        {
            get => this.strength;
            set
            {
                this.strength = Truncate(value);
            }
        }

        public int Dexterity
        {
            get => this.dexterity;
            set
            {
                this.dexterity = Truncate(value);
            }
        }

        public int Constitution
        {
            get => this.constitution;
            set
            {
                this.constitution = Truncate(value);
            }
        }

        public int Intelligence
        {
            get => this.intelligence;
            set
            {
                this.intelligence = Truncate(value);
            }
        }

        public int Wisdom
        {
            get => this.wisdom;
            set
            {
                this.wisdom = Truncate(value);
            }
        }

        public int Charisma
        {
            get => this.charisma;
            set
            {
                this.charisma = Truncate(value);
            }
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
