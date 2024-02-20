// <copyright file="SpellCasting.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Magic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    public sealed class SpellCasting : ICopyable<SpellCasting>
    {
        public SpellCasting()
        {
            this.MagicalClasses = [];
            this.Spells = [];
            this.SpellSlots = new SpellSlots();
        }

        public List<MagicalClass> MagicalClasses { get; set; }

        public List<Spell> Spells { get; set; }

        public SpellSlots SpellSlots { get; set; }

        [JsonIgnore]
        public int CasterLevel
        {
            get
            {
                int level = 0;
                foreach (var magicalClass in this.MagicalClasses)
                {
                    level += magicalClass.Level;
                }

                return level;
            }
        }

        [JsonIgnore]
        [SearchIgnore]
        public Spell? ConcentratedSpell => this.Spells.Where(x => x.CurrentConcentration).FirstOrDefault();

        [JsonIgnore]
        [SearchIgnore]
        public List<Spell> PreparedSpells => this.Spells.Where(x => x.Prepared).ToList();

        public void SetConcentration(Spell spell)
        {
            this.ClearConcentration();
            spell.CurrentConcentration = true;
        }

        public void ClearConcentration()
        {
            var concentration = this.Spells.SingleOrDefault(x => x.CurrentConcentration);
            if (concentration is not null)
            {
                concentration.CurrentConcentration = false;
            }
        }

        public int GetSpellAttack(string className)
        {
            return this.MagicalClasses.Where(x => x.Name.Equals(className, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()?.Attack ?? 0;
        }

        public SpellCasting DeepCopy()
        {
            return new SpellCasting()
            {
                MagicalClasses = [.. this.MagicalClasses.DeepCopy()],
                Spells = [.. this.Spells.DeepCopy()],
                SpellSlots = this.SpellSlots.DeepCopy(),
            };
        }
    }
}
