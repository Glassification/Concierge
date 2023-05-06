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
    using Concierge.Common.Exceptions;
    using Concierge.Data;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    using Constants = Concierge.Common.Constants;

    public sealed class Weapon : ICopyable<Weapon>, IUnique, IEquipable
    {
        public Weapon()
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
        }

        public Weapon(ICreature creature)
            : this()
        {
            this.Creature = creature;
        }

        public int Amount { get; set; }

        public bool Attuned { get; set; }

        public bool IsEquipped { get; set; }

        public EquipmentSlot EquipmentSlot { get; set; }

        public Abilities Ability { get; set; }

        public CoinType CoinType { get; set; }

        public string Damage { get; set; }

        public DamageTypes DamageType { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategory().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategory().IconKind;

        public Guid Id { get; set; }

        public bool IgnoreWeight { get; set; }

        public string Misc { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Brush ProficiencyIconColor => this.GetProficientValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind ProficiencyIconKind => this.GetProficientValue().IconKind;

        [JsonIgnore]
        [SearchIgnore]
        public Brush AttunedIconColor => this.GetAttunedValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind AttunedIconKind => this.GetAttunedValue().IconKind;

        public bool ProficiencyOverride { get; set; }

        public string Range { get; set; }

        public WeaponTypes Type { get; set; }

        public int Value { get; set; }

        public UnitDouble Weight { get; set; }

        [JsonIgnore]
        public int Attack
        {
            get
            {
                var bonus = 0;

                if (this.Creature?.IsWeaponProficient(this) ?? false)
                {
                    bonus = Program.CcsFile.Character.ProficiencyBonus;
                }

                return this.Ability switch
                {
                    Abilities.STR => Constants.CalculateBonus(this.Creature?.Characteristic.Attributes.Strength ?? 10) + bonus,
                    Abilities.DEX => Constants.CalculateBonus(this.Creature?.Characteristic.Attributes.Dexterity ?? 10) + bonus,
                    Abilities.CON => Constants.CalculateBonus(this.Creature?.Characteristic.Attributes.Constitution ?? 10) + bonus,
                    Abilities.INT => Constants.CalculateBonus(this.Creature?.Characteristic.Attributes.Intelligence ?? 10) + bonus,
                    Abilities.WIS => Constants.CalculateBonus(this.Creature?.Characteristic.Attributes.Wisdom ?? 10) + bonus,
                    Abilities.CHA => Constants.CalculateBonus(this.Creature?.Characteristic.Attributes.Charisma ?? 10) + bonus,
                    Abilities.NONE => bonus,
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private ICreature? Creature { get; set; }

        public Weapon DeepCopy()
        {
            return new Weapon(this.Creature ?? throw new NullValueException(nameof(this.Creature)))
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
            };
        }

        public void Initialize(ICreature creature)
        {
            this.Creature = creature;
        }

        public override string ToString()
        {
            return this.Name;
        }

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
            return Program.CcsFile.Character.IsWeaponProficient(this) ?
                (IconKind: PackIconKind.RadioButtonChecked, Brush: ConciergeBrushes.Mint) :
                (IconKind: PackIconKind.RadioButtonUnchecked, Brush: ConciergeBrushes.Deer);
        }
    }
}
