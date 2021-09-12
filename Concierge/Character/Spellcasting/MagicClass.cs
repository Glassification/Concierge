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

        public Guid Id { get; init; }

        [JsonIgnore]
        public int PreparedSpells => Program.CcsFile.Character.Spells.Where(x => (x.Class?.Equals(this.Name) ?? false) && x.Prepared).ToList().Count;

        [JsonIgnore]
        public int Attack => Utilities.CalculateBonusFromAbility(this.Ability, Program.CcsFile.Character);

        [JsonIgnore]
        public int Save => Utilities.CalculateBonusFromAbility(this.Ability, Program.CcsFile.Character) + Constants.BaseDC;

        public override string ToString()
        {
            return this.Name;
        }
    }
}
