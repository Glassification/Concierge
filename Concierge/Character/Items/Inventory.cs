// <copyright file="Inventory.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Items
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Primitives;
    using Concierge.Utility;
    using Concierge.Utility.Attributes;
    using Concierge.Utility.Dtos;
    using Concierge.Utility.Extensions;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public sealed class Inventory : ICopyable<Inventory>, IUnique, IEquipable
    {
        public Inventory()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Weight = UnitDouble.Empty;
            this.Notes = string.Empty;
            this.ItemCategory = "Adventuring Gear";
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.Now;
            this.EquipmentSlot = EquipmentSlot.None;
        }

        public int Amount { get; set; }

        public bool Attuned { get; set; }

        public CoinType CoinType { get; set; }

        public DateTime CreationDate { get; set; }

        public string Description { get; set; }

        [SearchIgnore]
        public Guid EquppedId { get; set; }

        [SearchIgnore]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Brush AttunedIconColor => this.GetAttunedValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind AttunedIconKind => this.GetAttunedValue().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IgnoreWeightIcon => this.GetWeightIgnoreValue();

        public bool IgnoreWeight { get; set; }

        public int Index { get; set; }

        public string ItemCategory { get; set; }

        public string Notes { get; set; }

        public int Value { get; set; }

        [JsonIgnore]
        public string ValueText => $"{this.Value} {this.CoinType.GetDescription()}";

        public UnitDouble Weight { get; set; }

        public bool IsEquipped { get; set; }

        public EquipmentSlot EquipmentSlot { get; set; }

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }

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
                CreationDate = this.CreationDate,
                IsEquipped = this.IsEquipped,
                EquipmentSlot = this.EquipmentSlot,
            };
        }

        public CategoryDto GetCategory()
        {
            return this.ItemCategory switch
            {
                "Adventuring Gear" => new CategoryDto(PackIconKind.MapLegend, Brushes.LightBlue, this.ItemCategory),
                "Arcane Focus" => new CategoryDto(PackIconKind.MagicWand, Brushes.Magenta, this.ItemCategory),
                "Clothing" => new CategoryDto(PackIconKind.TshirtV, Brushes.Coral, this.ItemCategory),
                "Druidic Focus" => new CategoryDto(PackIconKind.MagicStaff, Brushes.Magenta, this.ItemCategory),
                "Equipment Pack" => new CategoryDto(PackIconKind.Toolbox, Brushes.RosyBrown, this.ItemCategory),
                "Food/Drink" => new CategoryDto(PackIconKind.SilverwareVariant, Brushes.PaleVioletRed, this.ItemCategory),
                "Gemstone" => new CategoryDto(PackIconKind.DiamondStone, Brushes.Cyan, this.ItemCategory),
                "Heavy Armor" => new CategoryDto(PackIconKind.Wall, Brushes.LightGray, this.ItemCategory),
                "Holy Symbol" => new CategoryDto(PackIconKind.Christianity, Brushes.PaleGoldenrod, this.ItemCategory),
                "Light Armor" => new CategoryDto(PackIconKind.Wall, Brushes.LightGray, this.ItemCategory),
                "Medium Armor" => new CategoryDto(PackIconKind.Wall, Brushes.LightGray, this.ItemCategory),
                "Mount" => new CategoryDto(PackIconKind.HorsebackRiding, Brushes.SandyBrown, this.ItemCategory),
                "Poison" => new CategoryDto(PackIconKind.Poison, Brushes.LightGreen, this.ItemCategory),
                "Potion" => new CategoryDto(PackIconKind.HealthPotion, Brushes.Pink, this.ItemCategory),
                "Shield" => new CategoryDto(PackIconKind.ShieldPerson, Brushes.LightSlateGray, this.ItemCategory),
                "Tool" => new CategoryDto(PackIconKind.RulerSquareCompass, Brushes.LightYellow, this.ItemCategory),
                "Vehicle (Land)" => new CategoryDto(PackIconKind.Caravan, Brushes.SaddleBrown, this.ItemCategory),
                "Vehicle (Water)" => new CategoryDto(PackIconKind.SailBoat, Brushes.LightSkyBlue, this.ItemCategory),
                _ => new CategoryDto(),
            };
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
    }
}
