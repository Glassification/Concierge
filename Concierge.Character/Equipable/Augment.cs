// <copyright file="Augment.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using System;
    using System.Text;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using Concierge.Common.Extensions;
    using Concierge.Data;
    using Concierge.Tools;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an augment that can be used by various attacks.
    /// </summary>
    public sealed class Augment : ICopyable<Augment>, IUnique, IUsable
    {
        private int used;

        /// <summary>
        /// Initializes a new instance of the <see cref="Augment"/> class.
        /// </summary>
        public Augment()
        {
            this.Name = string.Empty;
            this.Icon = CustomIcon.Empty;
            this.Damage = string.Empty;
            this.Description = string.Empty;
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the damage dealt by the augment.
        /// </summary>
        public string Damage { get; set; }

        /// <summary>
        /// Gets or sets the type of damage dealt by the augment.
        /// </summary>
        public DamageTypes DamageType { get; set; }

        /// <summary>
        /// Gets or sets the description of the augment.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the icon of the augment.
        /// </summary>
        public CustomIcon Icon { get; set; }

        public Guid Id { get; set; }

        public bool IsCustom { get; set; }

        /// <summary>
        /// Gets or sets the name of the augment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the augment.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the augment is recoverable.
        /// </summary>
        public bool Recoverable { get; set; }

        /// <summary>
        /// Gets or sets the total number of times the augment has been used.
        /// </summary>
        public int Total
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
        /// Gets or sets the type of augment.
        /// </summary>
        public AugmentType Type { get; set; }

        /// <summary>
        /// Gets the display quantity of the augment.
        /// </summary>
        [JsonIgnore]
        public string DisplayQuantity => $"{this.Total} / {this.Quantity}";

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(Augment);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.Goldenrod;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.OctagramPlus;

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"{this.Damage} {this.DamageType} {this.Type}".Strip("None").Trim();

        /// <summary>
        /// Builds a tuple containing augment details for a given <paramref name="useItem"/>.
        /// </summary>
        /// <param name="useItem">The item to build augment details from.</param>
        /// <returns>A tuple containing the damage, damage type, and description of the augment.</returns>
        public static (string damage, string damageType, string description) Build(UseItem useItem)
        {
            var augmentDamage = new StringBuilder();
            var augmentDamageType = new StringBuilder();
            var augmentDescription = new StringBuilder();
            foreach (var item in useItem.Items)
            {
                if (item is not Augment augment)
                {
                    continue;
                }

                augmentDamage.Append(augment.Damage).Append(' ');
                augmentDamageType.Append(augment.DamageType.ToString()).Append(", ");
                augmentDescription.Append(augment.Description).Append(' ');
            }

            return (augmentDamage.ToString(), augmentDamageType.ToString(), augmentDescription.ToString());
        }

        /// <summary>
        /// Creates a deep copy of the augment.
        /// </summary>
        /// <returns>A new instance of the <see cref="Augment"/> class that is a copy of this instance.</returns>
        public Augment DeepCopy()
        {
            return new Augment()
            {
                Name = this.Name,
                Type = this.Type,
                Quantity = this.Quantity,
                Damage = this.Damage,
                DamageType = this.DamageType,
                Total = this.Total,
                Id = this.Id,
                IsCustom = this.IsCustom,
                Recoverable = this.Recoverable,
                Description = this.Description,
                Icon = this.Icon.DeepCopy(),
            };
        }

        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Gets the category of the augment based on its name.
        /// </summary>
        /// <returns>The category of the augment.</returns>
        public CategoryDto GetCategory()
        {
            return new CategoryDto(this.Icon.Kind, this.Icon.Color.Brush, this.Name);
        }

        /// <summary>
        /// Recovers the augment's quantity.
        /// </summary>
        public void Recover()
        {
            this.Quantity = this.Recoverable ? this.Total + ConciergeMath.Recover(this.Quantity - this.Total) : this.Total;
            this.Total = this.Quantity;
        }

        /// <summary>
        /// Uses the augment.
        /// </summary>
        /// <param name="useItem">The item to use.</param>
        /// <returns>An instance of <see cref="UsedItem"/>.</returns>
        public UsedItem Use(UseItem useItem)
        {
            if (this.Recoverable)
            {
                this.Total--;
            }

            return UsedItem.Empty;
        }
    }
}
