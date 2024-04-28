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

    /// <summary>
    /// Represents an ability possessed by a character, such as a class feature, racial trait, or feat.
    /// </summary>
    public sealed class Ability : ICopyable<Ability>, IUnique, IUsable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ability"/> class with default values.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the type of the ability.
        /// </summary>
        public AbilityTypes Type { get; set; }

        /// <summary>
        /// Gets a display-friendly representation of the ability type.
        /// </summary>
        [JsonIgnore]
        public string TypeDisplay => this.Type.PascalCase();

        /// <summary>
        /// Gets or sets the name of the ability.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the level of the ability.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the usage details of the ability.
        /// </summary>
        public string Uses { get; set; }

        /// <summary>
        /// Gets or sets the recovery details of the ability.
        /// </summary>
        public string Recovery { get; set; }

        /// <summary>
        /// Gets or sets the requirements for using the ability.
        /// </summary>
        public string Requirements { get; set; }

        /// <summary>
        /// Gets or sets the action required to use the ability.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the description of the ability.
        /// </summary>
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

        /// <summary>
        /// Creates a deep copy of the ability.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Ability"/>.</returns>
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

        /// <summary>
        /// Returns the name of the ability.
        /// </summary>
        /// <returns>The name of the ability.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Gets the category of the ability based on its type.
        /// </summary>
        /// <returns>The category of the ability.</returns>
        public CategoryDto GetCategory()
        {
            return this.Type switch
            {
                AbilityTypes.Background => new CategoryDto(PackIconKind.ArrangeSendBackward, Brushes.LightBlue, this.Type.ToString()),
                AbilityTypes.Feat => new CategoryDto(PackIconKind.StarCircleOutline, Brushes.MediumPurple, this.Type.ToString()),
                AbilityTypes.ClassFeature => new CategoryDto(PackIconKind.BookVariant, Brushes.Orange, this.Type.ToString()),
                AbilityTypes.RaceFeature => new CategoryDto(PackIconKind.BookVariant, Brushes.IndianRed, this.Type.ToString()),
                AbilityTypes.None => new CategoryDto(PackIconKind.BorderNone, Brushes.SlateGray, this.Type.ToString()),
                _ => CategoryDto.Empty,
            };
        }

        /// <summary>
        /// Uses the ability and returns the result as a used item.
        /// </summary>
        /// <param name="useItem">The use item associated with the ability use.</param>
        /// <returns>The result of using the ability as a used item.</returns>
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
