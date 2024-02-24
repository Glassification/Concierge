// <copyright file="Weapon.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using Concierge.Data;
    using Concierge.Tools;
    using Concierge.Tools.DiceRoller;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a weapon that can be equipped, used, and copied.
    /// </summary>
    public sealed class Weapon : ICopyable<Weapon>, IUnique, IEquipable, IUsable
    {
        private CharacterService characterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Weapon"/> class.
        /// </summary>
        public Weapon()
            : this(new CharacterService(CharacterSheet.Empty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Weapon"/> class with the specified <paramref name="characterService"/>.
        /// </summary>
        /// <param name="characterService">The character service associated with the weapon.</param>
        public Weapon(CharacterService characterService)
        {
            this.Name = string.Empty;
            this.Damage = string.Empty;
            this.Misc = string.Empty;
            this.Range = string.Empty;
            this.Note = string.Empty;
            this.Weight = UnitDouble.Empty;
            this.Id = Guid.NewGuid();
            this.EquipmentSlot = EquipmentSlot.None;
            this.Amount = 1;

            this.characterService = characterService;
        }

        /// <summary>
        /// Gets or sets the ability associated with the weapon.
        /// </summary>
        public Abilities Ability { get; set; }

        public int Amount { get; set; }

        public bool Attuned { get; set; }

        /// <summary>
        /// Gets or sets the coin type associated with the weapon.
        /// </summary>
        public CoinType CoinType { get; set; }

        /// <summary>
        /// Gets or sets the damage inflicted by the weapon.
        /// </summary>
        public string Damage { get; set; }

        /// <summary>
        /// Gets or sets the type of damage inflicted by the weapon.
        /// </summary>
        public DamageTypes DamageType { get; set; }

        public EquipmentSlot EquipmentSlot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether weight is ignored for the weapon.
        /// </summary>
        public bool IgnoreWeight { get; set; }

        public bool IsCustom { get; set; }

        public bool IsEquipped { get; set; }

        /// <summary>
        /// Gets or sets additional information about the weapon.
        /// </summary>
        public string Misc { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Gets or sets additional notes about the weapon.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the weapon's proficiency is overridden.
        /// </summary>
        public bool ProficiencyOverride { get; set; }

        /// <summary>
        /// Gets or sets the range of the weapon.
        /// </summary>
        public string Range { get; set; }

        /// <summary>
        /// Gets or sets the type of the weapon.
        /// </summary>
        public WeaponTypes Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the weapon.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the weight of the weapon.
        /// </summary>
        public UnitDouble Weight { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Brush AttunedIconColor => this.GetAttunedValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind AttunedIconKind => this.GetAttunedValue().IconKind;

        /// <summary>
        /// Gets the attack bonus of the weapon based on the associated character's proficiency and ability scores.
        /// </summary>
        [JsonIgnore]
        public int Attack
        {
            get
            {
                var bonus = this.characterService.GetProficiencyBonus(this);
                return this.characterService.CalculateBonus(this.Ability) + bonus;
            }
        }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(Weapon);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.RosyBrown;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.Sword;

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
        public string Information => $"{this.DamageType} {this.Type.ToString().FormatFromPascalCase()} - {this.Value}{this.CoinType.GetDescription()}";

        [JsonIgnore]
        [SearchIgnore]
        public Brush ProficiencyIconColor => this.GetProficientValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind ProficiencyIconKind => this.GetProficientValue().IconKind;

        /// <summary>
        /// Creates a deep copy of the weapon.
        /// </summary>
        /// <returns>A new instance of the <see cref="Weapon"/> class that is a deep copy of this instance.</returns>
        public Weapon DeepCopy()
        {
            return new Weapon(this.characterService)
            {
                Name = this.Name,
                Ability = this.Ability,
                Damage = this.Damage,
                Misc = this.Misc,
                DamageType = this.DamageType,
                Range = this.Range,
                Note = this.Note,
                Weight = this.Weight.DeepCopy(),
                Type = this.Type,
                ProficiencyOverride = this.ProficiencyOverride,
                IgnoreWeight = this.IgnoreWeight,
                Id = this.Id,
                CoinType = this.CoinType,
                Attuned = this.Attuned,
                Value = this.Value,
                IsEquipped = this.IsEquipped,
                Amount = this.Amount,
                EquipmentSlot = this.EquipmentSlot,
                IsCustom = this.IsCustom,
            };
        }

        /// <summary>
        /// Initializes the weapon with the specified character service.
        /// </summary>
        /// <param name="characterService">The character service to initialize the weapon with.</param>
        public void Initialize(CharacterService characterService)
        {
            this.characterService = characterService;
        }

        /// <summary>
        /// Returns a string that represents the current weapon.
        /// </summary>
        /// <returns>A string that represents the current weapon.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Uses the weapon to perform an action.
        /// </summary>
        /// <param name="useItem">The use item action to perform with the weapon.</param>
        /// <returns>A <see cref="UsedItem"/> representing the result of using the weapon.</returns>
        public UsedItem Use(UseItem useItem)
        {
            var ammunition = useItem.Item as Ammunition;

            var damageInput = $"{this.Damage} {this.Misc} {(ammunition is not null ? ammunition.Bonus : string.Empty)}";
            var cleanedInput = DiceParser.Clean(damageInput, Enum.GetNames(typeof(DamageTypes)));
            if (!DiceParser.IsValidInput(cleanedInput))
            {
                cleanedInput = "0";
            }

            var attack = new DiceRoll(Dice.D20, 1, this.Attack);
            var damage = new CustomDiceRoll(cleanedInput);
            var damageType = $"{this.DamageType} {ammunition?.DamageType}";

            return new UsedItem(attack, damage, this.Name, damageType, $"[Damage: {damage.Min} - {damage.Max}] {this.Note}");
        }

        /// <summary>
        /// Gets the category of the weapon based on its type.
        /// </summary>
        /// <returns>A <see cref="CategoryDto"/> containing information about the weapon's category.</returns>
        public CategoryDto GetCategory()
        {
            return this.Type switch
            {
                WeaponTypes.Battleaxe => new CategoryDto(PackIconKind.AxeBattle, Brushes.IndianRed, this.Type.ToString()),
                WeaponTypes.Blowgun => new CategoryDto(PackIconKind.SignPole, Brushes.Magenta, this.Type.ToString()),
                WeaponTypes.Club => new CategoryDto(PackIconKind.Oar, Brushes.Cyan, this.Type.ToString()),
                WeaponTypes.Dagger => new CategoryDto(PackIconKind.KnifeMilitary, Brushes.LightGreen, this.Type.ToString()),
                WeaponTypes.Dart => new CategoryDto(PackIconKind.SignPole, Brushes.Magenta, this.Type.ToString()),
                WeaponTypes.Flail => new CategoryDto(PackIconKind.Mace, Brushes.LightPink, this.Type.ToString()),
                WeaponTypes.Glaive => new CategoryDto(PackIconKind.AxeBattle, Brushes.IndianRed, this.Type.ToString()),
                WeaponTypes.Greataxe => new CategoryDto(PackIconKind.AxeBattle, Brushes.IndianRed, this.Type.ToString()),
                WeaponTypes.Greatclub => new CategoryDto(PackIconKind.Oar, Brushes.Cyan, this.Type.ToString()),
                WeaponTypes.Greatsword => new CategoryDto(PackIconKind.Sword, Brushes.LightGreen, this.Type.ToString()),
                WeaponTypes.Halberd => new CategoryDto(PackIconKind.AxeBattle, Brushes.IndianRed, this.Type.ToString()),
                WeaponTypes.Handaxe => new CategoryDto(PackIconKind.Axe, Brushes.IndianRed, this.Type.ToString()),
                WeaponTypes.HandCrossbow => new CategoryDto(PackIconKind.BowArrow, Brushes.Orange, this.Type.ToString()),
                WeaponTypes.HeavyCrossbow => new CategoryDto(PackIconKind.BowArrow, Brushes.Orange, this.Type.ToString()),
                WeaponTypes.Javelin => new CategoryDto(PackIconKind.Spear, Brushes.LightBlue, this.Type.ToString()),
                WeaponTypes.Lance => new CategoryDto(PackIconKind.Spear, Brushes.LightBlue, this.Type.ToString()),
                WeaponTypes.LightCrossbow => new CategoryDto(PackIconKind.BowArrow, Brushes.Orange, this.Type.ToString()),
                WeaponTypes.LightHammer => new CategoryDto(PackIconKind.Hammer, Brushes.Cyan, this.Type.ToString()),
                WeaponTypes.Longbow => new CategoryDto(PackIconKind.BowArrow, Brushes.Orange, this.Type.ToString()),
                WeaponTypes.Longsword => new CategoryDto(PackIconKind.Sword, Brushes.LightGreen, this.Type.ToString()),
                WeaponTypes.Mace => new CategoryDto(PackIconKind.Mace, Brushes.LightPink, this.Type.ToString()),
                WeaponTypes.Maul => new CategoryDto(PackIconKind.Mace, Brushes.LightPink, this.Type.ToString()),
                WeaponTypes.Morningstar => new CategoryDto(PackIconKind.Mace, Brushes.LightPink, this.Type.ToString()),
                WeaponTypes.Net => new CategoryDto(PackIconKind.SpiderWeb, Brushes.MediumPurple, this.Type.ToString()),
                WeaponTypes.Pike => new CategoryDto(PackIconKind.Spear, Brushes.LightBlue, this.Type.ToString()),
                WeaponTypes.Quarterstaff => new CategoryDto(PackIconKind.MagicStaff, Brushes.Cyan, this.Type.ToString()),
                WeaponTypes.Rapier => new CategoryDto(PackIconKind.Fencing, Brushes.LightGreen, this.Type.ToString()),
                WeaponTypes.Scimitar => new CategoryDto(PackIconKind.Sword, Brushes.LightGreen, this.Type.ToString()),
                WeaponTypes.Shortbow => new CategoryDto(PackIconKind.BowArrow, Brushes.Orange, this.Type.ToString()),
                WeaponTypes.Shortsword => new CategoryDto(PackIconKind.Sword, Brushes.LightGreen, this.Type.ToString()),
                WeaponTypes.Sickle => new CategoryDto(PackIconKind.Sickle, Brushes.MediumPurple, this.Type.ToString()),
                WeaponTypes.Sling => new CategoryDto(PackIconKind.Gesture, Brushes.MediumPurple, this.Type.ToString()),
                WeaponTypes.Spear => new CategoryDto(PackIconKind.Spear, Brushes.LightBlue, this.Type.ToString()),
                WeaponTypes.Trident => new CategoryDto(PackIconKind.SilverwareFork, Brushes.MediumPurple, this.Type.ToString()),
                WeaponTypes.Unarmed => new CategoryDto(PackIconKind.HandFrontLeft, Brushes.Cyan, this.Type.ToString()),
                WeaponTypes.Warhammer => new CategoryDto(PackIconKind.Hammer, Brushes.Cyan, this.Type.ToString()),
                WeaponTypes.WarPick => new CategoryDto(PackIconKind.Pickaxe, Brushes.MediumPurple, this.Type.ToString()),
                WeaponTypes.Whip => new CategoryDto(PackIconKind.JumpRope, Brushes.MediumPurple, this.Type.ToString()),
                _ => new CategoryDto(),
            };
        }

        private (PackIconKind IconKind, Brush Brush) GetAttunedValue()
        {
            return this.Attuned ?
                (IconKind: PackIconKind.RadioButtonChecked, Brush: ConciergeBrushes.Mint) :
                (IconKind: PackIconKind.RadioButtonUnchecked, Brush: ConciergeBrushes.Deer);
        }

        private (PackIconKind IconKind, Brush Brush) GetProficientValue()
        {
            return this.characterService.GetProficiencyBonus(this) > 0 ?
                (IconKind: PackIconKind.RadioButtonChecked, Brush: ConciergeBrushes.Mint) :
                (IconKind: PackIconKind.RadioButtonUnchecked, Brush: ConciergeBrushes.Deer);
        }
    }
}
