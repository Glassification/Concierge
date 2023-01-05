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
        public Brush IconColor => this.GetCategoryValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategoryValue().IconKind;

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

        private (PackIconKind IconKind, Brush Brush) GetCategoryValue()
        {
            return this.Type switch
            {
                AbilityTypes.Background => (IconKind: PackIconKind.ArrangeSendBackward, Brush: Brushes.LightBlue),
                AbilityTypes.Feat => (IconKind: PackIconKind.StarCircleOutline, Brush: Brushes.MediumPurple),
                AbilityTypes.ClassFeature => (IconKind: PackIconKind.BookVariant, Brush: Brushes.Orange),
                AbilityTypes.RaceFeature => (IconKind: PackIconKind.BookVariant, Brush: Brushes.IndianRed),
                AbilityTypes.None => (IconKind: PackIconKind.BorderNone, Brush: Brushes.SlateGray),
                _ => (IconKind: PackIconKind.Error, Brush: Brushes.Red),
            };
        }
    }
}
