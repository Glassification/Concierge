// <copyright file="Armor.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using Concierge.Data;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a type of armor that can be worn by characters.
    /// </summary>
    public sealed class Armor : ICopyable<Armor>, IUnique
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Armor"/> class.
        /// </summary>
        public Armor()
        {
            this.Name = string.Empty;
            this.Weight = UnitDouble.Empty;
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the armor class (AC) provided by the armor.
        /// </summary>
        public int Ac { get; set; }

        public Guid Id { get; set; }

        public bool IsCustom { get; set; }

        /// <summary>
        /// Gets or sets the name of the armor.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the stealth disadvantage imposed by the armor.
        /// </summary>
        public ArmorStealth Stealth { get; set; }

        /// <summary>
        /// Gets or sets the strength requirement for wearing the armor.
        /// </summary>
        public int Strength { get; set; }

        /// <summary>
        /// Gets or sets the type of the armor.
        /// </summary>
        public ArmorType Type { get; set; }

        /// <summary>
        /// Gets or sets the weight of the armor.
        /// </summary>
        public UnitDouble Weight { get; set; }

        [JsonIgnore]
        public string CustomType => nameof(Armor);

        [JsonIgnore]
        public Brush CustomTypeColor => Brushes.Silver;

        [JsonIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.Wall;

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"{this.Type}, {this.Ac} AC - {this.Stealth} Stealth";

        /// <summary>
        /// Creates a deep copy of the <see cref="Armor"/> object.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Armor"/> object.</returns>
        public Armor DeepCopy()
        {
            return new Armor()
            {
                Name = this.Name,
                Type = this.Type,
                Ac = this.Ac,
                Strength = this.Strength,
                Weight = this.Weight.DeepCopy(),
                Stealth = this.Stealth,
                Id = this.Id,
                IsCustom = this.IsCustom,
            };
        }

        /// <summary>
        /// Retrieves the category information associated with the armor, based on the type.
        /// </summary>
        /// <returns>The category information associated with the armor.</returns>
        public CategoryDto GetCategory()
        {
            var color = this.Type switch
            {
                ArmorType.Light => ConciergeBrushes.LightCarryCapacity,
                ArmorType.Medium => ConciergeBrushes.MediumCarryCapacity,
                ArmorType.Heavy => ConciergeBrushes.HeavyCarryCapacity,
                ArmorType.Massive => ConciergeBrushes.Verdigris,
                _ => ConciergeBrushes.Silver,
            };

            return new CategoryDto(PackIconKind.Wall, color, this.Name);
        }
    }
}
