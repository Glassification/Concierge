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
    using Concierge.Common.Extensions;
    using Concierge.Tools;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a type of ammunition that can be used by characters in combat.
    /// </summary>
    public sealed class Ammunition : ICopyable<Ammunition>, IUnique, IUsable
    {
        private int used;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ammunition"/> class.
        /// </summary>
        public Ammunition()
        {
            this.Name = string.Empty;
            this.Bonus = string.Empty;
            this.Description = string.Empty;
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the bonus associated with the ammunition.
        /// </summary>
        public string Bonus { get; set; }

        /// <summary>
        /// Gets or sets the type of currency used to represent the value of the ammunition.
        /// </summary>
        public CoinType CoinType { get; set; }

        /// <summary>
        /// Gets or sets the type of damage inflicted by the ammunition.
        /// </summary>
        public DamageTypes DamageType { get; set; }

        /// <summary>
        /// Gets or sets the description of the ammunition.
        /// </summary>
        public string Description { get; set; }

        public Guid Id { get; set; }

        public bool IsCustom { get; set; }

        /// <summary>
        /// Gets or sets the name of the ammunition.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the ammunition available.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ammunition is recoverable after use.
        /// </summary>
        public bool Recoverable { get; set; }

        /// <summary>
        /// Gets or sets the number of times the ammunition has been used.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the coin value of the ammunition.
        /// </summary>
        public int Value { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(Ammunition);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.BurlyWood;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.ArrowProjectileMultiple;

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"({this.Quantity}) {this.DamageType} - {this.Value}{this.CoinType.GetDescription()}";

        /// <summary>
        /// Creates a deep copy of the <see cref="Ammunition"/> object.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Ammunition"/> object.</returns>
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
                IsCustom = this.IsCustom,
                Recoverable = this.Recoverable,
                Description = this.Description,
            };
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The name of the ammunition.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Retrieves the category information associated with the ammunition, based off the name.
        /// </summary>
        /// <returns>The category information associated with the ammunition.</returns>
        public CategoryDto GetCategory()
        {
            return this.Name switch
            {
                string a when a.Contains("arrow", StringComparison.InvariantCultureIgnoreCase) => new CategoryDto(PackIconKind.ArrowProjectile, Brushes.IndianRed, this.Name),
                string b when b.Contains("needle", StringComparison.InvariantCultureIgnoreCase) => new CategoryDto(PackIconKind.SignPole, Brushes.LightGreen, this.Name),
                string c when c.Contains("bolt", StringComparison.InvariantCultureIgnoreCase) => new CategoryDto(PackIconKind.RayStartArrow, Brushes.LightBlue, this.Name),
                string d when d.Contains("shuriken", StringComparison.InvariantCultureIgnoreCase) => new CategoryDto(PackIconKind.Shuriken, Brushes.Orange, this.Name),
                string e when e.Contains("bullet", StringComparison.InvariantCultureIgnoreCase) => new CategoryDto(PackIconKind.SquareSmall, Brushes.MediumPurple, this.Name),
                _ => new CategoryDto(),
            };
        }

        /// <summary>
        /// Recovers the ammunition after use.
        /// </summary>
        public void Recover()
        {
            this.Quantity -= this.Recoverable ? this.Used - Common.Constants.Recover(this.Used) : this.Used;
            this.Used = 0;
        }

        /// <summary>
        /// Uses the ammunition.
        /// </summary>
        /// <param name="useItem">The item to use.</param>
        /// <returns>The used item.</returns>
        public UsedItem Use(UseItem useItem)
        {
            this.Used++;
            return new UsedItem();
        }
    }
}
