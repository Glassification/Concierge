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

    /// <summary>
    /// Represents a magical class in a Dungeons &amp; Dragons character.
    /// </summary>
    public sealed class MagicalClass : ICopyable<MagicalClass>, IUnique
    {
        private CharacterService characterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagicalClass"/> class with default values.
        /// </summary>
        public MagicalClass()
            : this(new CharacterService(CharacterSheet.Empty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagicalClass"/> class with a specified name.
        /// </summary>
        public MagicalClass(string name)
            : this(new CharacterService(CharacterSheet.Empty))
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagicalClass"/> class with the specified <see cref="CharacterService"/>.
        /// </summary>
        /// <param name="characterService">The character service associated with the magical class.</param>
        public MagicalClass(CharacterService characterService)
        {
            this.characterService = characterService;
            this.Name = string.Empty;
            this.Id = Guid.NewGuid();
            this.Created = DateTime.Now;
        }

        public Abilities Ability { get; set; }

        [JsonIgnore]
        public int Attack => this.characterService.CalculateBonusWithProficiency(this.Ability);

        public DateTime Created { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(MagicalClass).ToPascalCase();

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

        /// <summary>
        /// Returns the name of the magical class.
        /// </summary>
        /// <returns>The name of the magical class.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Initializes the magical class with the specified <see cref="CharacterService"/>.
        /// </summary>
        /// <param name="characterService">The character service to initialize with.</param>
        public void Initialize(CharacterService characterService)
        {
            this.characterService = characterService;
        }

        /// <summary>
        /// Creates a deep copy of the magical class.
        /// </summary>
        /// <returns>A deep copy of the <see cref="MagicalClass"/>.</returns>
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
                Created = this.Created,
            };
        }

        /// <summary>
        /// Retrieves the category information for the magical class based on its name.
        /// </summary>
        /// <returns>The category information for the magical class.</returns>
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
