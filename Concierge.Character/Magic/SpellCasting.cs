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
    using Concierge.Data;
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

        /// <summary>
        /// Retrieves details about spells based on the provided magical classes.
        /// </summary>
        /// <param name="magicalClasses">The list of magical classes.</param>
        /// <returns>A list of <see cref="SpellDetails"/> containing spell-related information.</returns>
        public List<SpellDetails> GetSpellDetails(List<MagicalClass> magicalClasses)
        {
            var list = new List<SpellDetails>();
            var classes = magicalClasses.Select(x => x.Name).ToList();
            classes.Add(string.Empty);

            list.Add(new SpellDetails("Spellcasting Level", this.CasterLevel.ToString()));
            list.Add(new SpellDetails("Total Spells", this.Spells.Count.ToString()));
            list.Add(new SpellDetails("Prepared Spells", this.PreparedSpells.Count.ToString()));
            list.Add(this.MostFrequentSchool());

            for (int i = 0; i < Spell.Levels; i++)
            {
                AddIfNotNull(list, this.SpellLevelDetails(i));
            }

            foreach (var magicalClass in classes)
            {
                AddIfNotNull(list, this.SpellClassDetails(magicalClass));
            }

            return list;
        }

        private static void AddIfNotNull(List<SpellDetails> list, SpellDetails? fancyString)
        {
            if (fancyString is not null)
            {
                list.Add(fancyString);
            }
        }

        private SpellDetails? SpellLevelDetails(int level)
        {
            var spells = this.Spells.Where(x => x.Level == level).ToList();
            if (spells.Count == 0)
            {
                return null;
            }

            return new SpellDetails($"{(level == 0 ? "Cantips" : $"Level {level} Spells")}", spells.Count.ToString());
        }

        private SpellDetails? SpellClassDetails(string name)
        {
            var spells = this.Spells.Where(x => x.Class?.Equals(name) ?? false).ToList();
            if (spells.Count == 0)
            {
                return null;
            }

            return new SpellDetails($"{(name.Equals(string.Empty) ? "Unclassed" : name)} Spells", spells.Count.ToString());
        }

        private SpellDetails MostFrequentSchool()
        {
            var school = this.Spells
                .GroupBy(x => x.School)
                .OrderByDescending(y => y.Count())
                .Take(1)
                .Select(z => z.Key).First();

            return new SpellDetails("Most Common School", school.ToString());
        }
    }
}
