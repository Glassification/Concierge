// <copyright file="MagicClass.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Spellcasting
{
    using System;
    using System.Linq;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using Concierge.Leveling.Dtos.Definitions;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    using Constants = Concierge.Common.Constants;

    public sealed class MagicClass : ICopyable<MagicClass>, IUnique
    {
        public MagicClass()
        {
            this.Name = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public Abilities Ability { get; set; }

        [JsonIgnore]
        public int Attack => Program.CcsFile.Character.CalculateBonusFromAbility(this.Ability);

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(MagicClass);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.Goldenrod;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.BookAccount;

        public Guid Id { get; set; }

        public bool IsCustom { get; set; }

        public int KnownCantrips { get; set; }

        public int KnownSpells { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public int PreparedSpells => Program.CcsFile.Character.Magic.Spells.Where(x => (x.Class?.Equals(this.Name) ?? false) && x.Level > 0 && x.Prepared).ToList().Count;

        [JsonIgnore]
        public int Save => Program.CcsFile.Character.CalculateBonusFromAbility(this.Ability) + Constants.BaseDC;

        public int SpellSlots { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

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
                IsCustom = this.IsCustom,
            };
        }

        public void LevelUp(SpellSlotDto spellSlotDto)
        {
            this.Level++;
            this.KnownSpells += spellSlotDto.Known;
            this.KnownCantrips += spellSlotDto.Cantrip;
            this.SpellSlots += spellSlotDto.Slots;
        }

        public CategoryDto GetCategory()
        {
            throw new NotImplementedException();
        }
    }
}
