// <copyright file="Ability.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Attributes;
    using Concierge.Utility.Dtos;
    using Concierge.Utility.Extensions;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public sealed class Ability : ICopyable<Ability>, IUnique
    {
        public Ability()
        {
            this.Type = AbilityTypes.None;
            this.Name = string.Empty;
            this.Uses = string.Empty;
            this.Recovery = string.Empty;
            this.Requirements = string.Empty;
            this.Action = string.Empty;
            this.Description = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public AbilityTypes Type { get; set; }

        [JsonIgnore]
        public string TypeDisplay => this.Type.ToString().FormatFromEnum();

        public string Name { get; set; }

        public int Level { get; set; }

        public string Uses { get; set; }

        public string Recovery { get; set; }

        public string Requirements { get; set; }

        public string Action { get; set; }

        public string Description { get; set; }

        public Guid Id { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        public Ability DeepCopy()
        {
            return new Ability()
            {
                Type = this.Type,
                Name = this.Name,
                Level = this.Level,
                Uses = this.Uses,
                Recovery = this.Recovery,
                Requirements = this.Requirements,
                Action = this.Action,
                Description = this.Description,
                Id = this.Id,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }

        public CategoryDto GetCategory()
        {
            return this.Type switch
            {
                AbilityTypes.Background => new CategoryDto(PackIconKind.ArrangeSendBackward, Brushes.LightBlue, this.Type.ToString()),
                AbilityTypes.Feat => new CategoryDto(PackIconKind.StarCircleOutline, Brushes.MediumPurple, this.Type.ToString()),
                AbilityTypes.ClassFeature => new CategoryDto(PackIconKind.BookVariant, Brushes.Orange, this.Type.ToString()),
                AbilityTypes.RaceFeature => new CategoryDto(PackIconKind.BookVariant, Brushes.IndianRed, this.Type.ToString()),
                AbilityTypes.None => new CategoryDto(PackIconKind.BorderNone, Brushes.SlateGray, this.Type.ToString()),
                _ => new CategoryDto(),
            };
        }
    }
}
