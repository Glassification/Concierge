﻿// <copyright file="SpellCasting.cs" company="Thomas Beckett">
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

    /// <summary>
    /// Represents spellcasting abilities and characteristics of a character.
    /// </summary>
    public sealed class SpellCasting : ICopyable<SpellCasting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpellCasting"/> class with default values.
        /// </summary>
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

        /// <summary>
        /// Sets concentration on the specified spell, clearing concentration from any other spell.
        /// </summary>
        /// <param name="spell">The spell to set concentration on.</param>
        public void SetConcentration(Spell spell)
        {
            this.ClearConcentration();
            spell.CurrentConcentration = true;
        }

        /// <summary>
        /// Clears concentration from any spell currently under concentration.
        /// </summary>
        public void ClearConcentration()
        {
            var concentration = this.Spells.SingleOrDefault(x => x.CurrentConcentration);
            if (concentration is not null)
            {
                concentration.CurrentConcentration = false;
            }
        }

        /// <summary>
        /// Gets the spell attack bonus for a specified magical class.
        /// </summary>
        /// <param name="className">The name of the magical class.</param>
        /// <returns>The spell attack bonus for the specified magical class.</returns>
        public int GetSpellAttack(string className)
        {
            return this.MagicalClasses.Where(x => x.Name.Equals(className, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()?.Attack ?? 0;
        }

        /// <summary>
        /// Creates a deep copy of the spellcasting instance.
        /// </summary>
        /// <returns>A deep copy of the <see cref="SpellCasting"/> instance.</returns>
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