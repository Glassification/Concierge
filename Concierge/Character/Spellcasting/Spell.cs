// <copyright file="Spell.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Spellcasting
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Attributes;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public sealed class Spell : ICopyable<Spell>, IUnique
    {
        public Spell()
        {
            this.Name = string.Empty;
            this.Components = string.Empty;
            this.Range = string.Empty;
            this.Duration = string.Empty;
            this.Area = string.Empty;
            this.Save = string.Empty;
            this.Damage = string.Empty;
            this.Description = string.Empty;
            this.Class = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Area { get; set; }

        public string Class { get; set; }

        public string Components { get; set; }

        public bool Concentration { get; set; }

        [JsonIgnore]
        public string ConcentrationDisplay => this.Concentration ? "Yes" : "No";

        public string Damage { get; set; }

        public string Description { get; set; }

        public string Duration { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategoryValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategoryValue().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public Brush PreparedIconColor => this.GetPreparedValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind PreparedIconKind => this.GetPreparedValue().IconKind;

        public Guid Id { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        public int Page { get; set; }

        public bool Prepared { get; set; }

        public string Range { get; set; }

        public bool Ritual { get; set; }

        [JsonIgnore]
        public string RitualDisplay => this.Ritual ? "Yes" : "No";

        public string Save { get; set; }

        public ArcaneSchools School { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

        public Spell DeepCopy()
        {
            return new Spell()
            {
                Prepared = this.Prepared,
                Level = this.Level,
                Page = this.Page,
                School = this.School,
                Ritual = this.Ritual,
                Components = this.Components,
                Concentration = this.Concentration,
                Range = this.Range,
                Duration = this.Duration,
                Area = this.Area,
                Save = this.Save,
                Damage = this.Damage,
                Description = this.Description,
                Class = this.Class,
                Id = this.Id,
                Name = this.Name,
            };
        }

        private (PackIconKind IconKind, Brush Brush) GetCategoryValue()
        {
            return this.School switch
            {
                ArcaneSchools.Abjuration => (IconKind: PackIconKind.ShieldSun, Brush: Brushes.LightBlue),
                ArcaneSchools.Conjuration => (IconKind: PackIconKind.Flare, Brush: Brushes.LightYellow),
                ArcaneSchools.Divination => (IconKind: PackIconKind.EyeCircle, Brush: Brushes.SlateGray),
                ArcaneSchools.Enchantment => (IconKind: PackIconKind.HeadCog, Brush: Brushes.LightPink),
                ArcaneSchools.Evocation => (IconKind: PackIconKind.Flash, Brush: Brushes.IndianRed),
                ArcaneSchools.Illusion => (IconKind: PackIconKind.AppleIcloud, Brush: Brushes.MediumPurple),
                ArcaneSchools.Necromancy => (IconKind: PackIconKind.Coffin, Brush: Brushes.LightGreen),
                ArcaneSchools.Transmutation => (IconKind: PackIconKind.CircleOpacity, Brush: Brushes.Orange),
                ArcaneSchools.Universal => (IconKind: PackIconKind.Earth, Brush: Brushes.White),
                _ => (IconKind: PackIconKind.Error, Brush: Brushes.Red),
            };
        }

        private (PackIconKind IconKind, Brush Brush) GetPreparedValue()
        {
            return this.Prepared ?
                (IconKind: PackIconKind.RadioButtonChecked, Brush: Brushes.PaleGreen) :
                (IconKind: PackIconKind.RadioButtonUnchecked, Brush: Brushes.PaleGoldenrod);
        }
    }
}
