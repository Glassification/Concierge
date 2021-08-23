// <copyright file="MagicClass.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Spellcasting
{
    using System;
    using System.Linq;

    using Concierge.Character.Enums;
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

                return this.Ability switch
                {
                    Abilities.STR => Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Strength) + bonus,
                    Abilities.DEX => Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Dexterity) + bonus,
                    Abilities.CON => Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Constitution) + bonus,
                    Abilities.INT => Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Intelligence) + bonus,
                    Abilities.WIS => Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Wisdom) + bonus,
                    Abilities.CHA => Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Charisma) + bonus,
                    Abilities.NONE => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };
            }
        }

        [JsonIgnore]
        public int Save
        {
            get
            {
                int bonus = Program.CcsFile.Character.ProficiencyBonus + Constants.BaseDC;

                return this.Ability switch
                {
                    Abilities.STR => Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Strength) + bonus,
                    Abilities.DEX => Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Dexterity) + bonus,
                    Abilities.CON => Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Constitution) + bonus,
                    Abilities.INT => Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Intelligence) + bonus,
                    Abilities.WIS => Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Wisdom) + bonus,
                    Abilities.CHA => Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Charisma) + bonus,
                    Abilities.NONE => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
