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

    public sealed class StatusEffect : ICopyable<StatusEffect>, IUnique
    {
        public StatusEffect()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public StatusEffectTypes Type { get; set; }

        public string Description { get; set; }

        public Guid Id { get; set; }

        public bool IsCustom { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(StatusEffect);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.LightYellow;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.ListStatus;

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        [JsonIgnore]
        public string Display => $"{this.Name}{(this.Type == StatusEffectTypes.None ? string.Empty : $" {this.Type}")}{(this.Description.IsNullOrWhiteSpace() ? string.Empty : " - ")}{this.Description}";

        public StatusEffect DeepCopy()
        {
            return new StatusEffect()
            {
                Name = this.Name,
                Type = this.Type,
                Description = this.Description,
                Id = this.Id,
                IsCustom = this.IsCustom,
            };
        }

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
                _ => new CategoryDto(PackIconKind.ListStatus, Brushes.MediumPurple, this.Name),
            };
        }

        public override string ToString()
        {
            return $"{this.Name} {this.Type}";
        }
    }
}
