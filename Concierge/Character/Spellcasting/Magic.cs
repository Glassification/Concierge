// <copyright file="Magic.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Spellcasting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Exceptions;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    public class Magic : ICopyable<Magic>
    {
        public Magic()
        {
            this.MagicClasses = new List<MagicClass>();
            this.Spells = new List<Spell>();
            this.SpellSlots = new SpellSlots();
        }

        public List<MagicClass> MagicClasses { get; set; }

        public List<Spell> Spells { get; set; }

        public SpellSlots SpellSlots { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Spell? ConcentratedSpell => this.Spells.Where(x => x.CurrentConcentration).FirstOrDefault();

        [JsonIgnore]
        [SearchIgnore]
        public List<Spell> PreparedSpells => this.Spells.Where(x => x.Prepared).ToList();

        [JsonIgnore]
        public int CasterLevel
        {
            get
            {
                int level = 0;
                foreach (var magicClass in this.MagicClasses)
                {
                    level += magicClass.Level;
                }

                return level;
            }
        }

        public void SetConcentration(Spell spell)
        {
            this.ClearConcentration();
            spell.CurrentConcentration = true;
        }

        public void ClearConcentration()
        {
            try
            {
                var concentration = this.Spells.SingleOrDefault(x => x.CurrentConcentration);
                if (concentration is not null)
                {
                    concentration.CurrentConcentration = false;
                }
            }
            catch (Exception)
            {
                Program.ErrorService.LogError(new InvalidListException($"This {nameof(this.Spells)} list has {this.Spells.Count} elements. There should only be 1 or 0."));
            }
        }

        public int GetSpellAttack(string className)
        {
            return this.MagicClasses.Where(x => x.Name.Equals(className, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()?.Attack ?? 0;
        }

        public Magic DeepCopy()
        {
            return new Magic()
            {
                MagicClasses = this.MagicClasses.DeepCopy().ToList(),
                Spells = this.Spells.DeepCopy().ToList(),
                SpellSlots = this.SpellSlots.DeepCopy(),
            };
        }
    }
}
