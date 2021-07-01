// <copyright file="Companion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters
{
    public class Companion
    {
        public Companion()
        {
            this.Name = string.Empty;
            this.ArmorClass = string.Empty;
            this.HitDice = string.Empty;
            this.Health = string.Empty;
            this.CurrentHealth = string.Empty;
            this.Speed = string.Empty;
            this.Strength = 0;
            this.Dexterity = 0;
            this.Constitution = 0;
            this.Intelligence = 0;
            this.Wisdom = 0;
            this.Charisma = 0;
            this.Perception = string.Empty;
            this.Senses = string.Empty;
            this.Attack = string.Empty;
            this.Type = string.Empty;
            this.AttackBonus = string.Empty;
            this.Damage = string.Empty;
            this.DamageType = string.Empty;
            this.Reach = string.Empty;
            this.Notes = string.Empty;
        }

        public string Name { get; set; }

        public string ArmorClass { get; set; }

        public string HitDice { get; set; }

        public string Health { get; set; }

        public string CurrentHealth { get; set; }

        public string Speed { get; set; }

        public int Strength { get; set; }

        public int Dexterity { get; set; }

        public int Constitution { get; set; }

        public int Intelligence { get; set; }

        public int Wisdom { get; set; }

        public int Charisma { get; set; }

        public string Perception { get; set; }

        public string Senses { get; set; }

        public string Attack { get; set; }

        public string Type { get; set; }

        public string AttackBonus { get; set; }

        public string Damage { get; set; }

        public string DamageType { get; set; }

        public string Reach { get; set; }

        public string Notes { get; set; }
    }
}
