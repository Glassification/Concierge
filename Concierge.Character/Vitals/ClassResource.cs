// <copyright file="ClassResource.cs" company="Thomas Beckett">
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
    /// Represents a class-specific resource that can be expended and potentially recovered.
    /// </summary>
    public sealed class ClassResource : ICopyable<ClassResource>, IUnique
    {
        private int spent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassResource"/> class.
        /// </summary>
        public ClassResource()
        {
            this.Id = Guid.NewGuid();
            this.Note = string.Empty;
            this.Recovery = Recovery.None;
            this.Type = string.Empty;
        }

        public bool IsCustom { get; set; }

        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets additional notes about the resource.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the recovery type of the resource.
        /// </summary>
        public Recovery Recovery { get; set; }

        /// <summary>
        /// Gets or sets the amount spent of the resource.
        /// </summary>
        public int Spent
        {
            get
            {
                return this.spent;
            }

            set
            {
                if (value <= this.Total)
                {
                    this.spent = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the resource.
        /// </summary>
        public int Total { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(ClassResource).FormatFromPascalCase();

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.LightCoral;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.RecycleVariant;

        [JsonIgnore]
        public string Description => $"{this.Type} - {this.Spent}/{this.Total} Used.{(this.Recovery == Recovery.None ? string.Empty : $" Recovers after {this.Recovery.GetDescription()}.")} {this.Note}";

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"";

        [JsonIgnore]
        public string Name
        {
            get => this.Type;
            set => this.Type = value;
        }

        public override string ToString()
        {
            return this.Type;
        }

        /// <summary>
        /// Creates a deep copy of the resource.
        /// </summary>
        /// <returns>A new instance of the <see cref="ClassResource"/> class with the same property values.</returns>
        public ClassResource DeepCopy()
        {
            return new ClassResource()
            {
                Type = this.Type,
                Total = this.Total,
                Spent = this.Spent,
                Note = this.Note,
                Recovery = this.Recovery,
                Id = this.Id,
                IsCustom = this.IsCustom,
            };
        }

        /// <summary>
        /// Gets the category of the resource.
        /// </summary>
        /// <returns>The category of the resource based on its name.</returns>
        public CategoryDto GetCategory()
        {
            return this.Name switch
            {
                "Bardic Inspiration" => new CategoryDto(PackIconKind.LightbulbAlert, Brushes.Yellow, this.Name),
                "Divine Sense" => new CategoryDto(PackIconKind.Leak, Brushes.Cyan, this.Name),
                "Infusions" => new CategoryDto(PackIconKind.Needle, Brushes.Goldenrod, this.Name),
                "Ki Points" => new CategoryDto(PackIconKind.Kabaddi, Brushes.LightGreen, this.Name),
                "Rages" => new CategoryDto(PackIconKind.EmojiAngry, Brushes.IndianRed, this.Name),
                "Sorcery Points" => new CategoryDto(PackIconKind.Creation, Brushes.OrangeRed, this.Name),
                "Superiority Dice" => new CategoryDto(PackIconKind.DiceD12, Brushes.SteelBlue, this.Name),
                _ => new CategoryDto(PackIconKind.ListStatus, Brushes.MediumPurple, this.Name),
            };
        }
    }
}
