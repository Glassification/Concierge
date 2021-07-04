// <copyright file="MagicClass.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System;
    using System.Linq;

    using Concierge.Characters.Enums;
    using Concierge.Utility;

    public class MagicClass
    {
        public MagicClass()
        {
            this.ID = Guid.NewGuid();
        }

        public MagicClass(Guid id)
        {
            this.ID = id;
        }

        public string Name { get; set; }

        public Abilities Ability { get; set; }

        public int Level { get; set; }

        public int KnownCantrips { get; set; }

        public int KnownSpells { get; set; }

        public Guid ID { get; private set; }

        public int PreparedSpells => Program.Character.Spells.Where(x => x.Class?.Equals(this.Name) ?? false && x.Prepared).ToList().Count;

        public int Attack
        {
            get
            {
                int bonus = Program.Character.ProficiencyBonus;

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

        public int Save
        {
            get
            {
                int bonus = Program.Character.ProficiencyBonus + Constants.BaseDC;

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

        public override string ToString()
        {
            return this.Name;
        }
    }
}
