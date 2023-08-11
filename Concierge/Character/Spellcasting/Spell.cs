// <copyright file="Spell.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Spellcasting
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using Concierge.Tools;
    using Concierge.Tools.DiceRoller;
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
        [SearchIgnore]
        public string CustomType => nameof(Spell);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.Plum;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.AutoFix;

        public string Damage { get; set; }

        public string Description { get; set; }

        public string Duration { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public Brush PreparedIconColor => this.GetPreparedValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind PreparedIconKind => this.GetPreparedValue().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind ConcentrationIcon => GetCheckBox(this.Concentration);

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind RitualIcon => GetCheckBox(this.Ritual);

        public Guid Id { get; set; }

        public bool IsCustom { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        public int Page { get; set; }

        public bool Prepared { get; set; }

        public string Range { get; set; }

        public bool Ritual { get; set; }

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
                IsCustom = this.IsCustom,
            };
        }

        public UsedItem Use()
        {
            var cleanedInput = DiceParser.Clean(this.Damage, Enum.GetNames(typeof(DamageTypes)));
            if (!DiceParser.IsValidInput(cleanedInput))
            {
                cleanedInput = "0";
            }

            var attackBonus = Program.CcsFile.Character.Magic.GetSpellAttack(this.Class);
            var attack = new DiceRoll(20, 1, attackBonus);
            var damage = new CustomDiceRoll(DiceParser.Parse(cleanedInput));

            return new UsedItem(attack, damage, this.Name, this.Description);
        }

        public CategoryDto GetCategory()
        {
            return this.School switch
            {
                ArcaneSchools.Abjuration => new CategoryDto(PackIconKind.ShieldSun, Brushes.LightBlue, this.School.ToString()),
                ArcaneSchools.Conjuration => new CategoryDto(PackIconKind.Flare, Brushes.LightYellow, this.School.ToString()),
                ArcaneSchools.Divination => new CategoryDto(PackIconKind.EyeCircle, Brushes.SlateGray, this.School.ToString()),
                ArcaneSchools.Enchantment => new CategoryDto(PackIconKind.HeadCog, Brushes.LightPink, this.School.ToString()),
                ArcaneSchools.Evocation => new CategoryDto(PackIconKind.Flash, Brushes.IndianRed, this.School.ToString()),
                ArcaneSchools.Illusion => new CategoryDto(PackIconKind.AppleIcloud, Brushes.MediumPurple, this.School.ToString()),
                ArcaneSchools.Necromancy => new CategoryDto(PackIconKind.Coffin, Brushes.LightGreen, this.School.ToString()),
                ArcaneSchools.Transmutation => new CategoryDto(PackIconKind.CircleOpacity, Brushes.Orange, this.School.ToString()),
                ArcaneSchools.Universal => new CategoryDto(PackIconKind.Earth, Brushes.White, this.School.ToString()),
                _ => new CategoryDto(),
            };
        }

        private static PackIconKind GetCheckBox(bool isChecked)
        {
            return isChecked ?
                PackIconKind.CheckBox :
                PackIconKind.CheckboxBlank;
        }

        private (PackIconKind IconKind, Brush Brush) GetPreparedValue()
        {
            return this.Prepared ?
                (IconKind: PackIconKind.RadioButtonChecked, Brush: ConciergeBrushes.Mint) :
                (IconKind: PackIconKind.RadioButtonUnchecked, Brush: ConciergeBrushes.Deer);
        }
    }
}
