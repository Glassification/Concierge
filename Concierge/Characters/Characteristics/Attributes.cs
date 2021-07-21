// <copyright file="Attributes.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Characteristics
{
    using Concierge.Utility;

    public class Attributes
    {
        private int _strength;
        private int _dexterity;
        private int _constitution;
        private int _intelligence;
        private int _wisdom;
        private int _charisma;

        public Attributes()
        {
            this._strength = 0;
            this._dexterity = 0;
            this._constitution = 0;
            this._intelligence = 0;
            this._wisdom = 0;
            this._charisma = 0;
        }

        public int Strength
        {
            get => this._strength;
            set
            {
                if (value >= Constants.MinScore && value <= Constants.MaxScore)
                {
                    this._strength = value;
                }
            }
        }

        public int Dexterity
        {
            get => this._dexterity;
            set
            {
                if (value >= Constants.MinScore && value <= Constants.MaxScore)
                {
                    this._dexterity = value;
                }
            }
        }

        public int Constitution
        {
            get => this._constitution;
            set
            {
                if (value >= Constants.MinScore && value <= Constants.MaxScore)
                {
                    this._constitution = value;
                }
            }
        }

        public int Intelligence
        {
            get => this._intelligence;
            set
            {
                if (value >= Constants.MinScore && value <= Constants.MaxScore)
                {
                    this._intelligence = value;
                }
            }
        }

        public int Wisdom
        {
            get => this._wisdom;
            set
            {
                if (value >= Constants.MinScore && value <= Constants.MaxScore)
                {
                    this._wisdom = value;
                }
            }
        }

        public int Charisma
        {
            get => this._charisma;
            set
            {
                if (value >= Constants.MinScore && value <= Constants.MaxScore)
                {
                    this._charisma = value;
                }
            }
        }
    }
}
