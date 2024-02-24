// <copyright file="CompanionWeapon.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Companions
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
    /// Represents a weapon wielded by a companion character.
    /// </summary>
    public sealed class CompanionWeapon : ICopyable<CompanionWeapon>, IUnique
    {
        private CharacterService characterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanionWeapon"/> class with default properties.
        /// </summary>
        public CompanionWeapon()
            : this(new CharacterService(CharacterSheet.Empty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanionWeapon"/> class with the specified character service.
        /// </summary>
        /// <param name="characterService">The character service to use.</param>
        public CompanionWeapon(CharacterService characterService)
        {
            this.Name = string.Empty;
            this.Damage = string.Empty;
            this.Misc = string.Empty;
            this.Range = string.Empty;
            this.Note = string.Empty;
            this.Id = Guid.NewGuid();

            this.characterService = characterService;
        }

        /// <summary>
        /// Gets or sets the ability associated with the weapon.
        /// </summary>
        public Abilities Ability { get; set; }

        /// <summary>
        /// Gets or sets the damage inflicted by the weapon.
        /// </summary>
        public string Damage { get; set; }

        /// <summary>
        /// Gets or sets the damage type of the weapon.
        /// </summary>
        public DamageTypes DamageType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the weapon is custom or not.
        /// </summary>
        public bool IsCustom { get; set; }

        /// <summary>
        /// Gets or sets miscellaneous information about the weapon.
        /// </summary>
        public string Misc { get; set; }

        /// <summary>
        /// Gets or sets the name of the weapon.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets additional notes about the weapon.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether proficiency is overridden for the weapon.
        /// </summary>
        public bool ProficiencyOverride { get; set; }

        /// <summary>
        /// Gets or sets the range of the weapon.
        /// </summary>
        public string Range { get; set; }

        /// <summary>
        /// Gets the attack bonus of the weapon.
        /// </summary>
        [JsonIgnore]
        public int Attack
        {
            get
            {
                var bonus = this.characterService.GetProficiencyBonus(this);
                return this.characterService.CalculateCompanionBonus(this.Ability) + bonus;
            }
        }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(CompanionWeapon).FormatFromPascalCase();

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.RosyBrown;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.Paw;

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        [SearchIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"{this.Damage} {this.DamageType} Attack";

        [JsonIgnore]
        [SearchIgnore]
        public Brush ProficiencyIconColor => this.GetProficientValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind ProficiencyIconKind => this.GetProficientValue().IconKind;

        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Creates a deep copy of the weapon instance.
        /// </summary>
        /// <returns>A deep copy of the <see cref="CompanionWeapon"/> instance.</returns>
        public CompanionWeapon DeepCopy()
        {
            return new CompanionWeapon(this.characterService)
            {
                Name = this.Name,
                Ability = this.Ability,
                Damage = this.Damage,
                Misc = this.Misc,
                DamageType = this.DamageType,
                Range = this.Range,
                Note = this.Note,
                ProficiencyOverride = this.ProficiencyOverride,
                Id = this.Id,
                IsCustom = this.IsCustom,
            };
        }

        /// <summary>
        /// Gets the category information of the weapon.
        /// </summary>
        public CategoryDto GetCategory()
        {
            return new CategoryDto(PackIconKind.Paw, Brushes.IndianRed, "TODO");
        }

        /// <summary>
        /// Initializes the weapon with the specified character service.
        /// </summary>
        /// <param name="characterService">The character service to use.</param>
        public void Initialize(CharacterService characterService)
        {
            this.characterService = characterService;
        }

        private (PackIconKind IconKind, Brush Brush) GetProficientValue()
        {
            return this.ProficiencyOverride ?
                (IconKind: PackIconKind.RadioButtonChecked, Brush: ConciergeBrushes.Mint) :
                (IconKind: PackIconKind.RadioButtonUnchecked, Brush: ConciergeBrushes.Deer);
        }
    }
}
