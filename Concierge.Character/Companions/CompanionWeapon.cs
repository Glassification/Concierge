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

    public sealed class CompanionWeapon : ICopyable<CompanionWeapon>, IUnique
    {
        private CharacterService characterService;

        public CompanionWeapon()
            : this(new CharacterService(CharacterSheet.Empty))
        {
        }

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

        public Abilities Ability { get; set; }

        public string Damage { get; set; }

        public DamageTypes DamageType { get; set; }

        public bool IsCustom { get; set; }

        public string Misc { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public bool ProficiencyOverride { get; set; }

        public string Range { get; set; }

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

        public CategoryDto GetCategory()
        {
            return new CategoryDto(PackIconKind.Paw, Brushes.IndianRed, "TODO");
        }

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
