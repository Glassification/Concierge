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

    using Constants = Concierge.Common.Constants;

    public sealed class Armor : ICopyable<Armor>, IUnique
    {
        public Armor()
        {
            this.Name = string.Empty;
            this.Weight = UnitDouble.Empty;
            this.Id = Guid.NewGuid();
        }

        public int Ac { get; set; }

        public string Name { get; set; }

        public ArmorStealth Stealth { get; set; }

        public int Strength { get; set; }

        public ArmorType Type { get; set; }

        public UnitDouble Weight { get; set; }

        [JsonIgnore]
        public int TotalAc
        {
            get
            {
                var armorAc = this.Type switch
                {
                    ArmorType.None => Constants.Bonus(Program.CcsFile.Character.Characteristic.Attributes.Dexterity),
                    ArmorType.Light => Constants.Bonus(Program.CcsFile.Character.Characteristic.Attributes.Dexterity),
                    ArmorType.Medium => Math.Min(2, Constants.Bonus(Program.CcsFile.Character.Characteristic.Attributes.Dexterity)),
                    _ => 0,
                };

                return armorAc + this.Ac;
            }
        }

        [JsonIgnore]
        public string CustomType => nameof(Armor);

        [JsonIgnore]
        public Brush CustomTypeColor => Brushes.Silver;

        [JsonIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.Wall;

        public Guid Id { get; set; }

        public bool IsCustom { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"{this.Type}, {this.Ac} AC - {this.Stealth} Stealth";

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
