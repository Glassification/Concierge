// <copyright file="Ammunition.cs" company="Thomas Beckett">
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
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public sealed class Ammunition : ICopyable<Ammunition>, IUnique
    {
        private int used;

        public Ammunition()
        {
            this.Name = string.Empty;
            this.Bonus = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Bonus { get; set; }

        public CoinType CoinType { get; set; }

        public DamageTypes DamageType { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int Used
        {
            get
            {
                return this.used;
            }

            set
            {
                if (value <= this.Quantity)
                {
                    this.used = value;
                }
            }
        }

        public int Value { get; set; }

        public Ammunition DeepCopy()
        {
            return new Ammunition()
            {
                Name = this.Name,
                Quantity = this.Quantity,
                Bonus = this.Bonus,
                DamageType = this.DamageType,
                Used = this.Used,
                Id = this.Id,
                CoinType = this.CoinType,
                Value = this.Value,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }

        public CategoryDto GetCategory()
        {
            return this.Name switch
            {
                string a when a.Contains("arrows", StringComparison.InvariantCultureIgnoreCase) => new CategoryDto(PackIconKind.ArrowProjectile, Brushes.IndianRed, this.Name),
                string b when b.Contains("needles", StringComparison.InvariantCultureIgnoreCase) => new CategoryDto(PackIconKind.SignPole, Brushes.LightGreen, this.Name),
                string c when c.Contains("bolts", StringComparison.InvariantCultureIgnoreCase) => new CategoryDto(PackIconKind.RayStartArrow, Brushes.LightBlue, this.Name),
                string d when d.Contains("shuriken", StringComparison.InvariantCultureIgnoreCase) => new CategoryDto(PackIconKind.Shuriken, Brushes.Orange, this.Name),
                string e when e.Contains("bullets", StringComparison.InvariantCultureIgnoreCase) => new CategoryDto(PackIconKind.SquareSmall, Brushes.MediumPurple, this.Name),
                _ => new CategoryDto(),
            };
        }
    }
}
