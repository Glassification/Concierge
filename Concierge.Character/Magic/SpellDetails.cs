// <copyright file="SpellDetails.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Magic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Data;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Provides methods to add various spell-related details to a list of <see cref="SpellDetail"/>.
    /// </summary>
    public sealed class SpellDetails
    {
        private readonly SpellCasting spellCasting;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellDetails"/> class with the specified <see cref="SpellCasting"/>.
        /// </summary>
        /// <param name="spellCasting">The spell casting information.</param>
        public SpellDetails(SpellCasting spellCasting)
        {
            this.spellCasting = spellCasting;
        }

        /// <summary>
        /// Adds header details such as spellcasting level, total spells, and prepared spells to the list.
        /// </summary>
        /// <param name="list">The list of spell details to add to.</param>
        public void AddHeaderDetails(List<SpellDetail> list)
        {
            list.Add(new SpellDetail("Spellcasting Level", this.spellCasting.CasterLevel.ToString(), PackIconKind.WizardHat, Brushes.SteelBlue));
            list.Add(new SpellDetail("Total Spells", this.spellCasting.Spells.StrCount(), PackIconKind.Counter, ConciergeBrushes.Mint));
            list.Add(new SpellDetail("Prepared Spells", this.spellCasting.PreparedSpells.StrCount(), PackIconKind.Counter, ConciergeBrushes.Mint));
        }

        /// <summary>
        /// Adds details for spells of a specific level to the list.
        /// </summary>
        /// <param name="list">The list of spell details to add to.</param>
        /// <param name="level">The spell level.</param>
        public void AddSpellLevelDetails(List<SpellDetail> list, int level)
        {
            var spells = this.spellCasting.Spells.Where(x => x.Level == level).ToList();
            if (spells.IsEmpty())
            {
                return;
            }

            list.Add(new SpellDetail($"{(level == 0 ? "Cantips" : $"Level {level} Spells")}", spells.StrCount(), PackIconKind.AutoFix, Brushes.Plum));
        }

        /// <summary>
        /// Adds details for spells of a specific class to the list.
        /// </summary>
        /// <param name="list">The list of spell details to add to.</param>
        /// <param name="name">The name of the magical class.</param>
        public void AddSpellClassDetails(List<SpellDetail> list, string name)
        {
            var spells = this.spellCasting.Spells.Where(x => x.Class?.Equals(name) ?? false).ToList();
            if (spells.IsEmpty())
            {
                return;
            }

            var magicClass = new MagicalClass()
            {
                Name = name,
            };
            var category = magicClass.GetCategory();

            list.Add(new SpellDetail($"{(name.Equals(string.Empty) ? "Unclassed" : name)} Spells", spells.StrCount(), category.IconKind, category.Brush));
        }

        /// <summary>
        /// Adds the most frequent school of spells to the list.
        /// </summary>
        /// <param name="list">The list of spell details to add to.</param>
        public void AddMostFrequentSchool(List<SpellDetail> list)
        {
            var spell = this.spellCasting.Spells
                .GroupBy(x => x.School)
                .OrderByDescending(y => y.Count())
                .SelectMany(z => z)
                .First();
            var category = spell.GetCategory();

            list.Add(new SpellDetail("Most Common School", spell.School.ToString(), category.IconKind, category.Brush));
        }

        /// <summary>
        /// Adds details about the currently concentrated spell to the list.
        /// </summary>
        /// <param name="list">The list of spell details to add to.</param>
        public void AddConcentrationDetails(List<SpellDetail> list)
        {
            var spell = this.spellCasting.ConcentratedSpell;
            if (spell is null)
            {
                return;
            }

            list.Add(new SpellDetail("Concentrated Spell", spell.Name, PackIconKind.ImageFilterCenterFocus, ConciergeBrushes.DarkPink));
        }
    }
}
