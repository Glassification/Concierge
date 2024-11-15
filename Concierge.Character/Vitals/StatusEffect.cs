// <copyright file="StatusEffect.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using Concierge.Common.Extensions;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a status effect applied to a character.
    /// </summary>
    public sealed class StatusEffect : ICopyable<StatusEffect>, IUnique
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffect"/> class with default values.
        /// </summary>
        public StatusEffect()
        {
            this.Description = string.Empty;
            this.Id = Guid.NewGuid();
            this.Name = string.Empty;
            this.Created = DateTime.Now;
        }

        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the description of the status effect.
        /// </summary>
        public string Description { get; set; }

        public Guid Id { get; set; }

        public bool IsCustom { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the status effect.
        /// </summary>
        public StatusEffectTypes Type { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(StatusEffect).ToPascalCase();

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.LightYellow;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.TextureBox;

        [JsonIgnore]
        public string Display => $"{this.Name}{(this.Type == StatusEffectTypes.None ? string.Empty : $" {this.Type}")}{(this.Description.IsNullOrWhiteSpace() ? string.Empty : " - ")}{this.Description}";

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"";

        /// <summary>
        /// Creates a deep copy of the status effect object.
        /// </summary>
        /// <returns>A new instance of the <see cref="StatusEffect"/> class with the same property values as the original.</returns>
        public StatusEffect DeepCopy()
        {
            return new StatusEffect()
            {
                Name = this.Name,
                Type = this.Type,
                Description = this.Description,
                Id = this.Id,
                IsCustom = this.IsCustom,
                Created = this.Created,
            };
        }

        /// <summary>
        /// Retrieves the category of the status effect.
        /// </summary>
        /// <returns>The category of the status effect based on the name.</returns>
        public CategoryDto GetCategory()
        {
            return this.Name.ToLower().Strip(" ") switch
            {
                "acid" => new CategoryDto(PackIconKind.Flask, Brushes.LightGreen, this.Name),
                "bludgeoning" => new CategoryDto(PackIconKind.AxeBattle, Brushes.IndianRed, this.Name),
                "cold" => new CategoryDto(PackIconKind.Snowflake, Brushes.Silver, this.Name),
                "fire" => new CategoryDto(PackIconKind.Fire, Brushes.OrangeRed, this.Name),
                "force" => new CategoryDto(PackIconKind.Triforce, Brushes.IndianRed, this.Name),
                "lightning" => new CategoryDto(PackIconKind.LightningBolt, Brushes.Goldenrod, this.Name),
                "magicweapons" => new CategoryDto(PackIconKind.MagicStaff, Brushes.Cyan, this.Name),
                "necrotic" => new CategoryDto(PackIconKind.Coffin, Brushes.YellowGreen, this.Name),
                "nonmagical" => new CategoryDto(PackIconKind.HumanHandsdown, Brushes.IndianRed, this.Name),
                "piercing" => new CategoryDto(PackIconKind.ArrowProjectile, Brushes.IndianRed, this.Name),
                "poison" => new CategoryDto(PackIconKind.Poison, Brushes.LightGreen, this.Name),
                "psychic" => new CategoryDto(PackIconKind.CrystalBall, Brushes.Cyan, this.Name),
                "radiant" => new CategoryDto(PackIconKind.WeatherSunny, Brushes.Goldenrod, this.Name),
                "slashing" => new CategoryDto(PackIconKind.Sword, Brushes.IndianRed, this.Name),
                "spells" => new CategoryDto(PackIconKind.Creation, Brushes.Cyan, this.Name),
                "thunder" => new CategoryDto(PackIconKind.WeatherLightning, Brushes.Goldenrod, this.Name),
                "healing" => new CategoryDto(PackIconKind.HeartPlus, ConciergeBrushes.Mint, this.Name),
                "damage" => new CategoryDto(PackIconKind.HeartMinus, Brushes.IndianRed, this.Name),
                _ => new CategoryDto(PackIconKind.TextureBox, Brushes.MediumPurple, this.Name),
            };
        }

        public override string ToString()
        {
            return $"{this.Name} {this.Type}";
        }
    }
}
