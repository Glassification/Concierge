// <copyright file="Weapon.cs" company="Thomas Beckett">
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
    using Concierge.Utility.Utilities;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public sealed class Weapon : ICopyable<Weapon>, IUnique
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
            this.ProficiencyOverride = false;
        }

        public Weapon(ICreature creature)
            : this()
        {
            this.Creature = creature;
        }

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
                    Abilities.STR => CharacterUtility.CalculateBonus(this.Creature?.Attributes.Strength ?? 10) + bonus,
                    Abilities.DEX => CharacterUtility.CalculateBonus(this.Creature?.Attributes.Dexterity ?? 10) + bonus,
                    Abilities.CON => CharacterUtility.CalculateBonus(this.Creature?.Attributes.Constitution ?? 10) + bonus,
                    Abilities.INT => CharacterUtility.CalculateBonus(this.Creature?.Attributes.Intelligence ?? 10) + bonus,
                    Abilities.WIS => CharacterUtility.CalculateBonus(this.Creature?.Attributes.Wisdom ?? 10) + bonus,
                    Abilities.CHA => CharacterUtility.CalculateBonus(this.Creature?.Attributes.Charisma ?? 10) + bonus,
                    Abilities.NONE => bonus,
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private ICreature? Creature { get; set; }

        public Weapon DeepCopy()
        {
            return new Weapon(this.Creature)
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
                Value = this.Value,
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

        public (PackIconKind IconKind, Brush Brush, string Name) GetCategory()
        {
            return this.Type switch
            {
                WeaponTypes.Battleaxe => (IconKind: PackIconKind.AxeBattle, Brush: Brushes.IndianRed, Name: this.Type.ToString()),
                WeaponTypes.Blowgun => (IconKind: PackIconKind.SignPole, Brush: Brushes.Magenta, Name: this.Type.ToString()),
                WeaponTypes.Club => (IconKind: PackIconKind.Oar, Brush: Brushes.Cyan, Name: this.Type.ToString()),
                WeaponTypes.Dagger => (IconKind: PackIconKind.KnifeMilitary, Brush: Brushes.LightGreen, Name: this.Type.ToString()),
                WeaponTypes.Dart => (IconKind: PackIconKind.SignPole, Brush: Brushes.Magenta, Name: this.Type.ToString()),
                WeaponTypes.Flail => (IconKind: PackIconKind.Mace, Brush: Brushes.LightPink, Name: this.Type.ToString()),
                WeaponTypes.Glaive => (IconKind: PackIconKind.AxeBattle, Brush: Brushes.IndianRed, Name: this.Type.ToString()),
                WeaponTypes.Greataxe => (IconKind: PackIconKind.AxeBattle, Brush: Brushes.IndianRed, Name: this.Type.ToString()),
                WeaponTypes.Greatclub => (IconKind: PackIconKind.Oar, Brush: Brushes.Cyan, Name: this.Type.ToString()),
                WeaponTypes.Greatsword => (IconKind: PackIconKind.Sword, Brush: Brushes.LightGreen, Name: this.Type.ToString()),
                WeaponTypes.Halberd => (IconKind: PackIconKind.AxeBattle, Brush: Brushes.IndianRed, Name: this.Type.ToString()),
                WeaponTypes.Handaxe => (IconKind: PackIconKind.Axe, Brush: Brushes.IndianRed, Name: this.Type.ToString()),
                WeaponTypes.HandCrossbow => (IconKind: PackIconKind.BowArrow, Brush: Brushes.Orange, Name: this.Type.ToString()),
                WeaponTypes.HeavyCrossbow => (IconKind: PackIconKind.BowArrow, Brush: Brushes.Orange, Name: this.Type.ToString()),
                WeaponTypes.Javelin => (IconKind: PackIconKind.Spear, Brush: Brushes.LightBlue, Name: this.Type.ToString()),
                WeaponTypes.Lance => (IconKind: PackIconKind.Spear, Brush: Brushes.LightBlue, Name: this.Type.ToString()),
                WeaponTypes.LightCrossbow => (IconKind: PackIconKind.BowArrow, Brush: Brushes.Orange, Name: this.Type.ToString()),
                WeaponTypes.LightHammer => (IconKind: PackIconKind.Hammer, Brush: Brushes.Cyan, Name: this.Type.ToString()),
                WeaponTypes.Longbow => (IconKind: PackIconKind.BowArrow, Brush: Brushes.Orange, Name: this.Type.ToString()),
                WeaponTypes.Longsword => (IconKind: PackIconKind.Sword, Brush: Brushes.LightGreen, Name: this.Type.ToString()),
                WeaponTypes.Mace => (IconKind: PackIconKind.Mace, Brush: Brushes.LightPink, Name: this.Type.ToString()),
                WeaponTypes.Maul => (IconKind: PackIconKind.Mace, Brush: Brushes.LightPink, Name: this.Type.ToString()),
                WeaponTypes.Morningstar => (IconKind: PackIconKind.Mace, Brush: Brushes.LightPink, Name: this.Type.ToString()),
                WeaponTypes.Net => (IconKind: PackIconKind.SpiderWeb, Brush: Brushes.MediumPurple, Name: this.Type.ToString()),
                WeaponTypes.Pike => (IconKind: PackIconKind.Spear, Brush: Brushes.LightBlue, Name: this.Type.ToString()),
                WeaponTypes.Quarterstaff => (IconKind: PackIconKind.MagicStaff, Brush: Brushes.Cyan, Name: this.Type.ToString()),
                WeaponTypes.Rapier => (IconKind: PackIconKind.Fencing, Brush: Brushes.LightGreen, Name: this.Type.ToString()),
                WeaponTypes.Scimitar => (IconKind: PackIconKind.Sword, Brush: Brushes.LightGreen, Name: this.Type.ToString()),
                WeaponTypes.Shortbow => (IconKind: PackIconKind.BowArrow, Brush: Brushes.Orange, Name: this.Type.ToString()),
                WeaponTypes.Shortsword => (IconKind: PackIconKind.Sword, Brush: Brushes.LightGreen, Name: this.Type.ToString()),
                WeaponTypes.Sickle => (IconKind: PackIconKind.Sickle, Brush: Brushes.MediumPurple, Name: this.Type.ToString()),
                WeaponTypes.Sling => (IconKind: PackIconKind.Gesture, Brush: Brushes.MediumPurple, Name: this.Type.ToString()),
                WeaponTypes.Spear => (IconKind: PackIconKind.Spear, Brush: Brushes.LightBlue, Name: this.Type.ToString()),
                WeaponTypes.Trident => (IconKind: PackIconKind.SilverwareFork, Brush: Brushes.MediumPurple, Name: this.Type.ToString()),
                WeaponTypes.Warhammer => (IconKind: PackIconKind.Hammer, Brush: Brushes.Cyan, Name: this.Type.ToString()),
                WeaponTypes.WarPick => (IconKind: PackIconKind.Pickaxe, Brush: Brushes.MediumPurple, Name: this.Type.ToString()),
                WeaponTypes.Whip => (IconKind: PackIconKind.JumpRope, Brush: Brushes.MediumPurple, Name: this.Type.ToString()),
                _ => (IconKind: PackIconKind.ArmFlex, Brush: Brushes.SlateGray, Name: this.Type.ToString()),
            };
        }

        private (PackIconKind IconKind, Brush Brush) GetProficientValue()
        {
            return Program.CcsFile.Character.IsWeaponProficient(this) ?
                (IconKind: PackIconKind.RadioButtonChecked, Brush: ConciergeBrushes.Mint) :
                (IconKind: PackIconKind.RadioButtonUnchecked, Brush: ConciergeBrushes.Deer);
        }
    }
}
