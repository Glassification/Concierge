// <copyright file="Weapon.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System;

    using Concierge.Characters.Enums;
    using Concierge.Utility;

    public class Weapon
    {
        public Weapon()
        {
            this.ID = Guid.NewGuid();
            this.ProficiencyOverride = false;
        }

        public Weapon(Guid id)
        {
            this.ID = id;
            this.ProficiencyOverride = false;
        }

        public string Name { get; set; }

        public int Attack
        {
            get
            {
                int bonus = 0;

                if (Program.Character.IsWeaponProficient(this))
                {
                    bonus = Program.Character.ProficiencyBonus;
                }

                switch (this.Ability)
                {
                    default:
                    case Abilities.STR:
                        return Utilities.CalculateBonus(Program.Character.Attributes.Strength) + bonus;
                    case Abilities.DEX:
                        return Utilities.CalculateBonus(Program.Character.Attributes.Dexterity) + bonus;
                    case Abilities.CON:
                        return Utilities.CalculateBonus(Program.Character.Attributes.Constitution) + bonus;
                    case Abilities.INT:
                        return Utilities.CalculateBonus(Program.Character.Attributes.Intelligence) + bonus;
                    case Abilities.WIS:
                        return Utilities.CalculateBonus(Program.Character.Attributes.Wisdom) + bonus;
                    case Abilities.CHA:
                        return Utilities.CalculateBonus(Program.Character.Attributes.Charisma) + bonus;
                }
            }
        }

        public Abilities Ability { get; set; }

        public string Damage { get; set; }

        public string Misc { get; set; }

        public DamageTypes DamageType { get; set; }

        public string Range { get; set; }

        public string Note { get; set; }

        public double Weight { get; set; }

        public WeaponTypes WeaponType { get; set; }

        public bool ProficiencyOverride { get; set; }

        public Guid ID { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
