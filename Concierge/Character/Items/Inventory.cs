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
    using Concierge.Utility.Extensions;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public sealed class Inventory : ICopyable<Inventory>, IUnique
    {
        public Inventory()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Weight = UnitDouble.Empty;
            this.Notes = string.Empty;
            this.ItemCategory = string.Empty;
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.Now;
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
        public Brush IconColor => this.GetCategoryValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategoryValue().IconKind;

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
            };
        }

        private (PackIconKind IconKind, Brush Brush) GetCategoryValue()
        {
            return this.ItemCategory switch
            {
                "Adventuring Gear" => (IconKind: PackIconKind.MapLegend, Brush: Brushes.LightBlue),
                "Arcane Focus" => (IconKind: PackIconKind.MagicWand, Brush: Brushes.Magenta),
                "Clothing" => (IconKind: PackIconKind.TshirtV, Brush: Brushes.Coral),
                "Druidic Focus" => (IconKind: PackIconKind.MagicStaff, Brush: Brushes.Magenta),
                "Equipment Pack" => (IconKind: PackIconKind.Toolbox, Brush: Brushes.RosyBrown),
                "Food/Drink" => (IconKind: PackIconKind.SilverwareVariant, Brush: Brushes.PaleVioletRed),
                "Gemstone" => (IconKind: PackIconKind.DiamondStone, Brush: Brushes.Cyan),
                "Heavy Armor" => (IconKind: PackIconKind.Wall, Brush: Brushes.LightGray),
                "Holy Symbol" => (IconKind: PackIconKind.Christianity, Brush: Brushes.PaleGoldenrod),
                "Light Armor" => (IconKind: PackIconKind.Wall, Brush: Brushes.LightGray),
                "Medium Armor" => (IconKind: PackIconKind.Wall, Brush: Brushes.LightGray),
                "Mount" => (IconKind: PackIconKind.HorsebackRiding, Brush: Brushes.SandyBrown),
                "Poison" => (IconKind: PackIconKind.Poison, Brush: Brushes.LightGreen),
                "Potion" => (IconKind: PackIconKind.HealthPotion, Brush: Brushes.Pink),
                "Shield" => (IconKind: PackIconKind.ShieldPerson, Brush: Brushes.LightSlateGray),
                "Tool" => (IconKind: PackIconKind.RulerSquareCompass, Brush: Brushes.LightYellow),
                "Vehicle (Land)" => (IconKind: PackIconKind.Caravan, Brush: Brushes.SaddleBrown),
                "Vehicle (Water)" => (IconKind: PackIconKind.SailBoat, Brush: Brushes.LightSkyBlue),
                _ => (IconKind: PackIconKind.Error, Brush: Brushes.IndianRed),
            };
        }

        private (PackIconKind IconKind, Brush Brush) GetAttunedValue()
        {
            return this.Attuned ?
                (IconKind: PackIconKind.RadioButtonChecked, Brush: Brushes.PaleGreen) :
                (IconKind: PackIconKind.RadioButtonUnchecked, Brush: Brushes.PaleGoldenrod);
        }

        private PackIconKind GetWeightIgnoreValue()
        {
            return this.IgnoreWeight ?
                PackIconKind.CheckboxBlank :
                PackIconKind.CheckBox;
        }
    }
}
