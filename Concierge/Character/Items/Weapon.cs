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

        public Abilities Ability { get; set; }

        public CoinType CoinType { get; set; }

        public string Damage { get; set; }

        public DamageTypes DamageType { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public Brush IconColor => this.GetCategoryValue().Brush;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind IconKind => this.GetCategoryValue().IconKind;

        public Guid Id { get; set; }

        public bool IgnoreWeight { get; set; }

        public string Misc { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

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
                var character = Program.CcsFile.Character;

                if (character.IsWeaponProficient(this))
                {
                    bonus = character.ProficiencyBonus;
                }

                return this.Ability switch
                {
                    Abilities.STR => CharacterUtility.CalculateBonus(character.Attributes.Strength) + bonus,
                    Abilities.DEX => CharacterUtility.CalculateBonus(character.Attributes.Dexterity) + bonus,
                    Abilities.CON => CharacterUtility.CalculateBonus(character.Attributes.Constitution) + bonus,
                    Abilities.INT => CharacterUtility.CalculateBonus(character.Attributes.Intelligence) + bonus,
                    Abilities.WIS => CharacterUtility.CalculateBonus(character.Attributes.Wisdom) + bonus,
                    Abilities.CHA => CharacterUtility.CalculateBonus(character.Attributes.Charisma) + bonus,
                    Abilities.NONE => bonus,
                    _ => throw new NotImplementedException(),
                };
            }
        }

        public Weapon DeepCopy()
        {
            return new Weapon()
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

        public override string ToString()
        {
            return this.Name;
        }

        private (PackIconKind IconKind, Brush Brush) GetCategoryValue()
        {
            return this.Type switch
            {
                WeaponTypes.Battleaxe => (IconKind: PackIconKind.AxeBattle, Brush: Brushes.IndianRed),
                WeaponTypes.Blowgun => (IconKind: PackIconKind.SignPole, Brush: Brushes.Magenta),
                WeaponTypes.Club => (IconKind: PackIconKind.Oar, Brush: Brushes.Cyan),
                WeaponTypes.Dagger => (IconKind: PackIconKind.KnifeMilitary, Brush: Brushes.LightGreen),
                WeaponTypes.Dart => (IconKind: PackIconKind.SignPole, Brush: Brushes.Magenta),
                WeaponTypes.Flail => (IconKind: PackIconKind.Mace, Brush: Brushes.LightPink),
                WeaponTypes.Glaive => (IconKind: PackIconKind.AxeBattle, Brush: Brushes.IndianRed),
                WeaponTypes.Greataxe => (IconKind: PackIconKind.AxeBattle, Brush: Brushes.IndianRed),
                WeaponTypes.Greatclub => (IconKind: PackIconKind.Oar, Brush: Brushes.Cyan),
                WeaponTypes.Greatsword => (IconKind: PackIconKind.Sword, Brush: Brushes.LightGreen),
                WeaponTypes.Halberd => (IconKind: PackIconKind.AxeBattle, Brush: Brushes.IndianRed),
                WeaponTypes.Handaxe => (IconKind: PackIconKind.Axe, Brush: Brushes.IndianRed),
                WeaponTypes.HandCrossbow => (IconKind: PackIconKind.BowArrow, Brush: Brushes.Orange),
                WeaponTypes.HeavyCrossbow => (IconKind: PackIconKind.BowArrow, Brush: Brushes.Orange),
                WeaponTypes.Javelin => (IconKind: PackIconKind.Spear, Brush: Brushes.LightBlue),
                WeaponTypes.Lance => (IconKind: PackIconKind.Spear, Brush: Brushes.LightBlue),
                WeaponTypes.LightCrossbow => (IconKind: PackIconKind.BowArrow, Brush: Brushes.Orange),
                WeaponTypes.LightHammer => (IconKind: PackIconKind.Hammer, Brush: Brushes.Cyan),
                WeaponTypes.Longbow => (IconKind: PackIconKind.BowArrow, Brush: Brushes.Orange),
                WeaponTypes.Longsword => (IconKind: PackIconKind.Sword, Brush: Brushes.LightGreen),
                WeaponTypes.Mace => (IconKind: PackIconKind.Mace, Brush: Brushes.LightPink),
                WeaponTypes.Maul => (IconKind: PackIconKind.Mace, Brush: Brushes.LightPink),
                WeaponTypes.Morningstar => (IconKind: PackIconKind.Mace, Brush: Brushes.LightPink),
                WeaponTypes.Net => (IconKind: PackIconKind.SpiderWeb, Brush: Brushes.MediumPurple),
                WeaponTypes.Pike => (IconKind: PackIconKind.Spear, Brush: Brushes.LightBlue),
                WeaponTypes.Quarterstaff => (IconKind: PackIconKind.MagicStaff, Brush: Brushes.Cyan),
                WeaponTypes.Rapier => (IconKind: PackIconKind.Fencing, Brush: Brushes.LightGreen),
                WeaponTypes.Scimitar => (IconKind: PackIconKind.Sword, Brush: Brushes.LightGreen),
                WeaponTypes.Shortbow => (IconKind: PackIconKind.BowArrow, Brush: Brushes.Orange),
                WeaponTypes.Shortsword => (IconKind: PackIconKind.Sword, Brush: Brushes.LightGreen),
                WeaponTypes.Sickle => (IconKind: PackIconKind.Sickle, Brush: Brushes.MediumPurple),
                WeaponTypes.Sling => (IconKind: PackIconKind.Gesture, Brush: Brushes.MediumPurple),
                WeaponTypes.Spear => (IconKind: PackIconKind.Spear, Brush: Brushes.LightBlue),
                WeaponTypes.Trident => (IconKind: PackIconKind.SilverwareFork, Brush: Brushes.MediumPurple),
                WeaponTypes.Warhammer => (IconKind: PackIconKind.Hammer, Brush: Brushes.Cyan),
                WeaponTypes.WarPick => (IconKind: PackIconKind.Pickaxe, Brush: Brushes.MediumPurple),
                WeaponTypes.Whip => (IconKind: PackIconKind.JumpRope, Brush: Brushes.MediumPurple),
                _ => (IconKind: PackIconKind.ArmFlex, Brush: Brushes.SlateGray),
            };
        }
    }
}
