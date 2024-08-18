// <copyright file="Inventory.cs" company="Thomas Beckett">
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
    using Concierge.Common.Extensions;
    using Concierge.Data;
    using Concierge.Tools;
    using Concierge.Tools.DiceRoller;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an item in the inventory, which can be equipped and used.
    /// </summary>
    public sealed class Inventory : ICopyable<Inventory>, IUnique, IEquipable, IUsable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Inventory"/> class with default values.
        /// </summary>
        public Inventory()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Weight = UnitDouble.Empty;
            this.Notes = string.Empty;
            this.ItemCategory = "Adventuring Gear";
            this.Id = Guid.NewGuid();
            this.Created = DateTime.Now;
            this.EquipmentSlot = EquipmentSlot.None;
        }

        /// <summary>
        /// Gets or sets the amount of the item.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is attuned to.
        /// </summary>
        public bool Attuned { get; set; }

        /// <summary>
        /// Gets or sets the type of coin used for the item.
        /// </summary>
        public CoinType CoinType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is consumable.
        /// </summary>
        public bool Consumable { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the item.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        public string Description { get; set; }

        public EquipmentSlot EquipmentSlot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the weight of the item is ignored.
        /// </summary>
        public bool IgnoreWeight { get; set; }

        /// <summary>
        /// Gets or sets the index of the item.
        /// </summary>
        public int Index { get; set; }

        public bool IsCustom { get; set; }

        public bool IsEquipped { get; set; }

        public string ItemCategory { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the notes about the item.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the value of the item.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the weight of the item.
        /// </summary>
        public UnitDouble Weight { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Brush AttunedIconColor => this.GetAttunedValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind AttunedIconKind => this.GetAttunedValue().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(Inventory);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.LightSkyBlue;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.BagPersonal;

        [SearchIgnore]
        public Guid EquppedId { get; set; }

        [SearchIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IgnoreWeightIcon => this.GetWeightIgnoreValue();

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"{this.ItemCategory} - {this.Value}{this.CoinType.GetDescription()}";

        /// <summary>
        /// Gets the displayable notes about the item.
        /// </summary>
        [JsonIgnore]
        public string NotesDisplay => this.GetNotesDisplay();

        /// <summary>
        /// Gets the text representation of the value of the item.
        /// </summary>
        [JsonIgnore]
        public string ValueText => $"{this.Value} {this.CoinType.GetDescription()}";

        /// <summary>
        /// Determines whether the specified object is equal to the current inventory item.
        /// </summary>
        /// <param name="obj">The object to compare with the current inventory item.</param>
        /// <returns>
        ///   <c>true</c> if the specified object is equal to the current inventory item; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (obj is not Inventory item)
            {
                return false;
            }

            return item.Name.Equals(this.Name) &&
                item.Weight == this.Weight &&
                item.CoinType == this.CoinType &&
                item.Value == this.Value &&
                item.ItemCategory.Equals(this.ItemCategory) &&
                item.Notes.Equals(this.Notes) &&
                item.Description.Equals(this.Description);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current inventory item.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current inventory item.
        /// </summary>
        /// <returns>The name of the current inventory item.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Copies the inventory item.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Inventory"/> item.</returns>
        public Inventory DeepCopy()
        {
            return new Inventory()
            {
                Name = this.Name,
                Amount = this.Amount,
                Weight = this.Weight.DeepCopy(),
                IgnoreWeight = this.IgnoreWeight,
                Attuned = this.Attuned,
                Notes = this.Notes,
                CoinType = this.CoinType,
                Value = this.Value,
                ItemCategory = this.ItemCategory,
                Description = this.Description,
                Index = this.Index,
                EquppedId = this.EquppedId,
                Id = this.Id,
                Created = this.Created,
                IsEquipped = this.IsEquipped,
                EquipmentSlot = this.EquipmentSlot,
                IsCustom = this.IsCustom,
                Consumable = this.Consumable,
            };
        }

        /// <summary>
        /// Gets the category of the item.
        /// </summary>
        /// <returns>The category information of the item.</returns>
        public CategoryDto GetCategory()
        {
            return this.ItemCategory switch
            {
                "Adventuring Gear" => new CategoryDto(PackIconKind.CompassRose, Brushes.LightBlue, this.ItemCategory),
                "Arcane Focus" => new CategoryDto(PackIconKind.MagicWand, Brushes.Magenta, this.ItemCategory),
                "Clothing" => new CategoryDto(PackIconKind.TshirtV, Brushes.Coral, this.ItemCategory),
                "Druidic Focus" => new CategoryDto(PackIconKind.MagicStaff, Brushes.Magenta, this.ItemCategory),
                "Equipment Pack" => new CategoryDto(PackIconKind.Toolbox, Brushes.RosyBrown, this.ItemCategory),
                "Food/Drink" => new CategoryDto(PackIconKind.SilverwareVariant, Brushes.PaleVioletRed, this.ItemCategory),
                "Gemstone" => new CategoryDto(PackIconKind.DiamondStone, Brushes.Cyan, this.ItemCategory),
                "Heavy Armor" => new CategoryDto(PackIconKind.Wall, Brushes.LightGray, this.ItemCategory),
                "Holy Symbol" => new CategoryDto(PackIconKind.Christianity, Brushes.PaleGoldenrod, this.ItemCategory),
                "Key" => new CategoryDto(PackIconKind.KeyVariant, Brushes.PaleGreen, this.ItemCategory),
                "Light Armor" => new CategoryDto(PackIconKind.Wall, Brushes.LightGray, this.ItemCategory),
                "Medium Armor" => new CategoryDto(PackIconKind.Wall, Brushes.LightGray, this.ItemCategory),
                "Mount" => new CategoryDto(PackIconKind.HorsebackRiding, Brushes.SandyBrown, this.ItemCategory),
                "Poison" => new CategoryDto(PackIconKind.Poison, Brushes.LightGreen, this.ItemCategory),
                "Potion" => new CategoryDto(PackIconKind.HealthPotion, Brushes.Pink, this.ItemCategory),
                "Scroll" => new CategoryDto(PackIconKind.Scroll, Brushes.MediumPurple, this.ItemCategory),
                "Shield" => new CategoryDto(PackIconKind.ShieldPerson, Brushes.LightSlateGray, this.ItemCategory),
                "Tool" => new CategoryDto(PackIconKind.RulerSquareCompass, Brushes.LightYellow, this.ItemCategory),
                "Vehicle (Land)" => new CategoryDto(PackIconKind.Caravan, Brushes.SaddleBrown, this.ItemCategory),
                "Vehicle (Water)" => new CategoryDto(PackIconKind.SailBoat, Brushes.LightSkyBlue, this.ItemCategory),
                _ => CategoryDto.Empty,
            };
        }

        /// <summary>
        /// Uses the item.
        /// </summary>
        /// <param name="useItem">The item to use.</param>
        /// <returns>The used item.</returns>
        public UsedItem Use(UseItem useItem)
        {
            var dice = DiceParser.Find(this.Description);
            var attack = DiceRoll.Empty;
            IDiceRoll damage = dice.IsNullOrWhiteSpace() ? DiceRoll.Empty : new CustomDiceRoll(dice);

            this.Amount += this.Consumable && !this.IsEquipped ? -1 : 0;

            return new UsedItem(attack, damage, this.Name, this.Name, this.Description);
        }

        private (PackIconKind IconKind, Brush Brush) GetAttunedValue()
        {
            return this.Attuned ?
                (IconKind: PackIconKind.RadioButtonChecked, Brush: ConciergeBrushes.Mint) :
                (IconKind: PackIconKind.RadioButtonUnchecked, Brush: ConciergeBrushes.Deer);
        }

        private PackIconKind GetWeightIgnoreValue()
        {
            return this.IgnoreWeight ?
                PackIconKind.CheckboxBlank :
                PackIconKind.CheckBox;
        }

        private string GetNotesDisplay()
        {
            if (!this.Consumable || this.Notes.ContainsIgnoreCase("Consum"))
            {
                return this.Notes;
            }

            return $"{this.Notes}{(this.Notes.Length == 0 ? string.Empty : ", ")}Consumable";
        }
    }
}
