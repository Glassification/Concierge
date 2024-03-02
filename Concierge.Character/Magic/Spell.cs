// <copyright file="Spell.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Magic
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using Concierge.Tools;
    using Concierge.Tools.DiceRoller;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a spell in Dungeons &amp; Dragons.
    /// </summary>
    public sealed class Spell : ICopyable<Spell>, IUnique, IUsable
    {
        public const int Levels = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spell"/> class with default values.
        /// </summary>
        public Spell()
        {
            this.Area = string.Empty;
            this.Class = string.Empty;
            this.Components = string.Empty;
            this.Damage = string.Empty;
            this.Description = string.Empty;
            this.Duration = string.Empty;
            this.Id = Guid.NewGuid();
            this.Name = string.Empty;
            this.Range = string.Empty;
            this.Save = string.Empty;
        }

        public string Area { get; set; }

        public string Class { get; set; }

        public string Components { get; set; }

        public bool Concentration { get; set; }

        public bool CurrentConcentration { get; set; }

        public string Damage { get; set; }

        public string Description { get; set; }

        public string Duration { get; set; }

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

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind ConcentrationIcon => GetCheckBox(this.Concentration);

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(Spell);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.Plum;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.AutoFix;

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public string Information => this.Level == 0 ? $"{this.School} Cantrip" : $"{this.Level.GetPostfix(true)} Level {this.School}";

        [JsonIgnore]
        [SearchIgnore]
        public Brush PreparedIconColor => this.GetPreparedValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind PreparedIconKind => this.GetPreparedValue().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind RitualIcon => GetCheckBox(this.Ritual);

        /// <summary>
        /// Returns the name of the spell.
        /// </summary>
        /// <returns>The name of the spell.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Creates a deep copy of the spell.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Spell"/>.</returns>
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
                CurrentConcentration = this.CurrentConcentration,
            };
        }

        /// <summary>
        /// Uses the spell to perform an action.
        /// </summary>
        /// <param name="useItem">The item to use.</param>
        /// <returns>An object representing the result of using the spell.</returns>
        public UsedItem Use(UseItem useItem)
        {
            var cleanedInput = DiceParser.Clean(this.Damage, Enum.GetNames(typeof(DamageTypes)));
            if (!DiceParser.IsValidInput(cleanedInput))
            {
                cleanedInput = "0";
            }

            var attack = new DiceRoll(Dice.D20, 1, useItem.Bonus);
            var damage = new CustomDiceRoll(cleanedInput);

            return new UsedItem(attack, damage, this.Name, string.Empty, $"[Damage: {damage.Min} - {damage.Max}] {this.Description}");
        }

        /// <summary>
        /// Retrieves the category of the spell based on its school of magic.
        /// </summary>
        /// <returns>The category of the spell.</returns>
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
