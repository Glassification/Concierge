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
        public Brush IconColor => this.GetPackIconColor();

        [JsonIgnore]
        public PackIconKind IconKind => this.GetPackIconKind();

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

        private PackIconKind GetPackIconKind()
        {
            return this.ItemCategory switch
            {
                "Adventuring Gear" => PackIconKind.Mountain,
                "Arcane Focus" => PackIconKind.MagicWand,
                "Clothing" => PackIconKind.TshirtCrew,
                "Consumables" => PackIconKind.FoodDrumstick,
                "Druidic Focus" => PackIconKind.MagicStaff,
                "Equipment Pack" => PackIconKind.Toolbox,
                "Gemstone" => PackIconKind.DiamondStone,
                "Heavy Armor" => PackIconKind.Wall,
                "Holy Symbol" => PackIconKind.Christianity,
                "Light Armor" => PackIconKind.Wall,
                "Medium Armor" => PackIconKind.Wall,
                "Mount" => PackIconKind.HorsebackRiding,
                "Poison" => PackIconKind.Poison,
                "Potion" => PackIconKind.HealthPotion,
                "Shield" => PackIconKind.ShieldPerson,
                "Tool" => PackIconKind.Screwdriver,
                "Vehicle (Land)" => PackIconKind.Caravan,
                "Vehicle (Water)" => PackIconKind.SailBoat,
                _ => PackIconKind.Error,
            };
        }

        private Brush GetPackIconColor()
        {
            return Brushes.Purple;
        }
    }
}
