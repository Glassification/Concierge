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

    public sealed class ClassResource : ICopyable<ClassResource>, IUnique
    {
        private int spent;

        public ClassResource()
        {
            this.Id = Guid.NewGuid();
            this.Note = string.Empty;
            this.Recovery = Recovery.None;
            this.Type = string.Empty;
        }

        public bool IsCustom { get; set; }

        public Guid Id { get; set; }

        public string Note { get; set; }

        public Recovery Recovery { get; set; }

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

        public string Type { get; set; }

        public int Total { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(ClassResource);

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

        public CategoryDto GetCategory()
        {
            return this.Name switch
            {
                "Bardic Inspiration" => new CategoryDto(PackIconKind.LightbulbAlert, Brushes.Yellow, this.Name),
                "Divine Sense" => new CategoryDto(PackIconKind.Leak, Brushes.Cyan, this.Name),
                "Infusions" => new CategoryDto(PackIconKind.Needle, Brushes.Goldenrod, this.Name),
                "Ki Points" => new CategoryDto(PackIconKind.Kabaddi, Brushes.LightGreen, this.Name),
                "Rages" => new CategoryDto(PackIconKind.EmojiAngry, Brushes.IndianRed, this.Name),
                "Sneak Attack Dice" => new CategoryDto(PackIconKind.DiceD6, Brushes.SteelBlue, this.Name),
                "Sorcery Points" => new CategoryDto(PackIconKind.Creation, Brushes.OrangeRed, this.Name),
                "Superiority Dice" => new CategoryDto(PackIconKind.DiceD12, Brushes.SteelBlue, this.Name),
                _ => new CategoryDto(PackIconKind.ListStatus, Brushes.MediumPurple, this.Name),
            };
        }
    }
}
