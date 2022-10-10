// <copyright file="MagicClass.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Spellcasting
{
    using System;
    using System.Linq;

    using Concierge.Character.Enums;
    using Concierge.Leveling.Dtos;
    using Concierge.Utility;
    using Concierge.Utility.Utilities;
    using Newtonsoft.Json;

    public sealed class MagicClass : ICopyable<MagicClass>
    {
        public MagicClass()
        {
            this.Name = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public Abilities Ability { get; set; }

        public int Level { get; set; }

        public int KnownCantrips { get; set; }

        public int KnownSpells { get; set; }

        public int SpellSlots { get; set; }

        public Guid Id { get; init; }

        [JsonIgnore]
        public int PreparedSpells => Program.CcsFile.Character.Spells.Where(x => (x.Class?.Equals(this.Name) ?? false) && x.Prepared).ToList().Count;

        [JsonIgnore]
        public int Attack => CharacterUtility.CalculateBonusFromAbility(this.Ability, Program.CcsFile.Character);

        [JsonIgnore]
        public int Save => CharacterUtility.CalculateBonusFromAbility(this.Ability, Program.CcsFile.Character) + Constants.BaseDC;

        public MagicClass DeepCopy()
        {
            return new MagicClass()
            {
                Name = this.Name,
                Ability = this.Ability,
                Level = this.Level,
                KnownCantrips = this.KnownCantrips,
                KnownSpells = this.KnownSpells,
                SpellSlots = this.SpellSlots,
                Id = this.Id,
            };
        }

        public void LevelUp(SpellSlotDto spellSlotDto)
        {
            this.Level++;
            this.KnownSpells += spellSlotDto.Known;
            this.KnownCantrips += spellSlotDto.Cantrip;
            this.SpellSlots += spellSlotDto.Slots;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
