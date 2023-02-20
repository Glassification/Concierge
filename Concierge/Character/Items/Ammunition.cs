// <copyright file="Ammunition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Items
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Attributes;
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

        public (PackIconKind IconKind, Brush Brush, string Name) GetCategory()
        {
            return this.Name switch
            {
                string a when a.Contains("arrows", StringComparison.InvariantCultureIgnoreCase) => (IconKind: PackIconKind.ArrowProjectile, Brush: Brushes.IndianRed, this.Name),
                string b when b.Contains("needles", StringComparison.InvariantCultureIgnoreCase) => (IconKind: PackIconKind.SignPole, Brush: Brushes.LightGreen, this.Name),
                string c when c.Contains("bolts", StringComparison.InvariantCultureIgnoreCase) => (IconKind: PackIconKind.RayStartArrow, Brush: Brushes.LightBlue, this.Name),
                string d when d.Contains("shuriken", StringComparison.InvariantCultureIgnoreCase) => (IconKind: PackIconKind.Shuriken, Brush: Brushes.Orange, this.Name),
                string e when e.Contains("bullets", StringComparison.InvariantCultureIgnoreCase) => (IconKind: PackIconKind.SquareSmall, Brush: Brushes.MediumPurple, this.Name),
                _ => (IconKind: PackIconKind.ArrowProjectileMultiple, Brush: Brushes.SlateGray, this.Name),
            };
        }
    }
}
