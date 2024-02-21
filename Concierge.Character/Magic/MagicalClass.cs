// <copyright file="MagicalClass.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Magic
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

    using Constants = Concierge.Common.Constants;

    public sealed class MagicalClass : ICopyable<MagicalClass>, IUnique
    {
        private CharacterService characterService;

        public MagicalClass()
            : this(new CharacterService(CharacterSheet.Empty))
        {
        }

        public MagicalClass(CharacterService characterService)
        {
            this.characterService = characterService;
            this.Name = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public Abilities Ability { get; set; }

        [JsonIgnore]
        public int Attack => this.characterService.CalculateBonusWithProficiency(this.Ability);

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(MagicalClass).FormatFromPascalCase();

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.Goldenrod;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.BookAccount;

        public Guid Id { get; set; }

        public bool IsCustom { get; set; }

        public int KnownCantrips { get; set; }

        public int KnownSpells { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public int PreparedSpells => this.characterService.ListPreparedSpells(this.Name).Count;

        [JsonIgnore]
        public int Save => this.characterService.CalculateBonusWithProficiency(this.Ability) + Constants.BaseDC;

        public int SpellSlots { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"{(this.Ability == Abilities.NONE ? "No Default" : $"{this.Ability} Based")}. Attack: {this.Attack}, Save: {this.Save}";

        public override string ToString()
        {
            return this.Name;
        }

        public void Initialize(CharacterService characterService)
        {
            this.characterService = characterService;
        }

        public MagicalClass DeepCopy()
        {
            return new MagicalClass(this.characterService)
            {
                Name = this.Name,
                Ability = this.Ability,
                Level = this.Level,
                KnownCantrips = this.KnownCantrips,
                KnownSpells = this.KnownSpells,
                SpellSlots = this.SpellSlots,
                Id = this.Id,
                IsCustom = this.IsCustom,
            };
        }

        public CategoryDto GetCategory()
        {
            return this.Name switch
            {
                "Artificer" => new CategoryDto(PackIconKind.Notebook, Brushes.MediumPurple, this.Name),
                "Barbarian" => new CategoryDto(PackIconKind.Notebook, Brushes.LightGreen, this.Name),
                "Bard" => new CategoryDto(PackIconKind.Notebook, Brushes.IndianRed, this.Name),
                "Blood Hunter" => new CategoryDto(PackIconKind.Notebook, Brushes.Goldenrod, this.Name),
                "Cleric" => new CategoryDto(PackIconKind.Notebook, Brushes.Cyan, this.Name),
                "Druid" => new CategoryDto(PackIconKind.Notebook, Brushes.OrangeRed, this.Name),
                "Fighter" => new CategoryDto(PackIconKind.Notebook, Brushes.LightPink, this.Name),
                "Monk" => new CategoryDto(PackIconKind.Notebook, Brushes.YellowGreen, this.Name),
                "Paladin" => new CategoryDto(PackIconKind.Notebook, Brushes.Magenta, this.Name),
                "Ranger" => new CategoryDto(PackIconKind.Notebook, Brushes.Orange, this.Name),
                "Rogue" => new CategoryDto(PackIconKind.Notebook, Brushes.Coral, this.Name),
                "Sorcerer" => new CategoryDto(PackIconKind.Notebook, Brushes.SteelBlue, this.Name),
                "Warlock" => new CategoryDto(PackIconKind.Notebook, Brushes.Yellow, this.Name),
                "Wizard" => new CategoryDto(PackIconKind.Notebook, Brushes.MediumSeaGreen, this.Name),
                _ => new CategoryDto(PackIconKind.Notebook, Brushes.Silver, this.Name),
            };
        }
    }
}
