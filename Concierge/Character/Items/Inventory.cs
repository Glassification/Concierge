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
    using Concierge.Utility.Extensions;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public sealed class Inventory : ICopyable<Inventory>
    {
        public Inventory()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Weight = UnitDouble.Empty;
            this.Notes = string.Empty;
            this.ItemCategory = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public int Amount { get; set; }

        public bool Attuned { get; set; }

        [JsonIgnore]
        public string AttunedText => this.Attuned ? "Yes" : "No";

        public CoinType CoinType { get; set; }

        public string Description { get; set; }

        public Guid EquppedId { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public Brush IconColor => this.GetCategoryValue().Brush;

        [JsonIgnore]
        public PackIconKind IconKind => this.GetCategoryValue().IconKind;

        public bool IgnoreWeight { get; set; }

        public int Index { get; set; }

        public string ItemCategory { get; set; }

        public string Notes { get; set; }

        public int Value { get; set; }

        [JsonIgnore]
        public string ValueText => $"{this.Value} {this.CoinType.GetDescription()}";

        public UnitDouble Weight { get; set; }

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
            };
        }

        private (PackIconKind IconKind, Brush Brush) GetCategoryValue()
        {
            return this.ItemCategory switch
            {
                "Adventuring Gear" => (IconKind: PackIconKind.Mountain, Brush: Brushes.LightBlue),
                "Arcane Focus" => (IconKind: PackIconKind.MagicWand, Brush: Brushes.LightBlue),
                "Clothing" => (IconKind: PackIconKind.TshirtCrew, Brush: Brushes.LightBlue),
                "Consumables" => (IconKind: PackIconKind.FoodDrumstick, Brush: Brushes.LightBlue),
                "Druidic Focus" => (IconKind: PackIconKind.MagicStaff, Brush: Brushes.LightBlue),
                "Equipment Pack" => (IconKind: PackIconKind.Toolbox, Brush: Brushes.LightBlue),
                "Gemstone" => (IconKind: PackIconKind.DiamondStone, Brush: Brushes.LightBlue),
                "Heavy Armor" => (IconKind: PackIconKind.Wall, Brush: Brushes.LightBlue),
                "Holy Symbol" => (IconKind: PackIconKind.Christianity, Brush: Brushes.LightBlue),
                "Light Armor" => (IconKind: PackIconKind.Wall, Brush: Brushes.LightBlue),
                "Medium Armor" => (IconKind: PackIconKind.Wall, Brush: Brushes.LightBlue),
                "Mount" => (IconKind: PackIconKind.HorsebackRiding, Brush: Brushes.LightBlue),
                "Poison" => (IconKind: PackIconKind.Poison, Brush: Brushes.LightBlue),
                "Potion" => (IconKind: PackIconKind.HealthPotion, Brush: Brushes.LightBlue),
                "Shield" => (IconKind: PackIconKind.ShieldPerson, Brush: Brushes.LightBlue),
                "Tool" => (IconKind: PackIconKind.Screwdriver, Brush: Brushes.LightBlue),
                "Vehicle (Land)" => (IconKind: PackIconKind.Caravan, Brush: Brushes.LightBlue),
                "Vehicle (Water)" => (IconKind: PackIconKind.SailBoat, Brush: Brushes.LightBlue),
                _ => (IconKind: PackIconKind.Error, Brush: Brushes.IndianRed),
            };
        }
    }
}
