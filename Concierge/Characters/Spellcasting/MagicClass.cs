// <copyright file="MagicClass.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Spellcasting
{
    using System;
    using System.Linq;

    using Concierge.Characters.Enums;
    using Concierge.Utility;
    using Newtonsoft.Json;

    public class MagicClass
    {
        public MagicClass()
        {
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public Abilities Ability { get; set; }

        public int Level { get; set; }

        public int KnownCantrips { get; set; }

        public int KnownSpells { get; set; }

        public Guid Id { get; private set; }

        [JsonIgnore]
        public int PreparedSpells => Program.CcsFile.Character.Spells.Where(x => x.Class?.Equals(this.Name) ?? false && x.Prepared).ToList().Count;

        [JsonIgnore]
        public int Attack
        {
            get
            {
                int bonus = Program.CcsFile.Character.ProficiencyBonus;

                switch (this.Ability)
                {
                    default:
                    case Abilities.STR:
                        return Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Strength) + bonus;
                    case Abilities.DEX:
                        return Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Dexterity) + bonus;
                    case Abilities.CON:
                        return Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Constitution) + bonus;
                    case Abilities.INT:
                        return Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Intelligence) + bonus;
                    case Abilities.WIS:
                        return Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Wisdom) + bonus;
                    case Abilities.CHA:
                        return Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Charisma) + bonus;
                }
            }
        }

        [JsonIgnore]
        public int Save
        {
            get
            {
                int bonus = Program.CcsFile.Character.ProficiencyBonus + Constants.BaseDC;

                switch (this.Ability)
                {
                    default:
                    case Abilities.STR:
                        return Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Strength) + bonus;
                    case Abilities.DEX:
                        return Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Dexterity) + bonus;
                    case Abilities.CON:
                        return Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Constitution) + bonus;
                    case Abilities.INT:
                        return Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Intelligence) + bonus;
                    case Abilities.WIS:
                        return Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Wisdom) + bonus;
                    case Abilities.CHA:
                        return Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Charisma) + bonus;
                }
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
