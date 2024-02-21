// <copyright file="Ability.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Details
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using Concierge.Common.Extensions;
    using Concierge.Tools;
    using Concierge.Tools.DiceRoller;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public sealed class Ability : ICopyable<Ability>, IUnique, IUsable
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
        public string TypeDisplay => this.Type.ToString().FormatFromPascalCase();

        public string Name { get; set; }

        public int Level { get; set; }

        public string Uses { get; set; }

        public string Recovery { get; set; }

        public string Requirements { get; set; }

        public string Action { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(Ability);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.Aquamarine;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.Run;

        public bool IsCustom { get; set; }

        public Guid Id { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"";

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
                IsCustom = this.IsCustom,
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

        public UsedItem Use(UseItem useItem)
        {
            var cleanedInput = DiceParser.Clean(this.Action, Enum.GetNames(typeof(DamageTypes)));
            if (!DiceParser.IsValidInput(cleanedInput))
            {
                cleanedInput = "0";
            }

            var attack = DiceRoll.Empty;
            var damage = new CustomDiceRoll(cleanedInput);

            return new UsedItem(attack, damage, this.Name, this.Type.ToString(), this.Description);
        }
    }
}
