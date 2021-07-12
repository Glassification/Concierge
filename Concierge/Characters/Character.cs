// <copyright file="Character.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Characters.Collections;
    using Concierge.Characters.Enums;
    using Concierge.Utility;
    using Newtonsoft.Json;

    public class Character
    {
        private static readonly int[] Levels =
        {
            300, 900, 2700, 6500, 14000, 23000, 34000, 48000, 64000, 85000, 100000,
            120000, 140000, 165000, 195000, 225000, 265000, 305000, 355000, 0,
        };

        private static readonly int[] Proficiencies = { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6 };

        public Character()
        {
            this.Initialize();
        }

        public List<Ability> Abilities { get; set; }

        public List<Ammunition> Ammunitions { get; private set; }

        public Appearance Appearance { get; private set; }

        public Armor Armor { get; private set; }

        public Attributes Attributes { get; private set; }

        public List<Class> Classess { get; private set; }

        public List<ClassResource> ClassResources { get; private set; }

        public Companion Companion { get; private set; }

        public Details Details { get; private set; }

        public List<Chapter> Chapters { get; set; }

        public List<Inventory> Inventories { get; private set; }

        public List<MagicClass> MagicClasses { get; private set; }

        public Personality Personality { get; private set; }

        public Proficiency Proficiency { get; set; }

        public SavingThrow SavingThrow { get; private set; }

        public Skill Skill { get; private set; }

        public List<Spell> Spells { get; private set; }

        public SpellSlots SpellSlots { get; set; }

        public Vitality Vitality { get; private set; }

        public Wealth Wealth { get; private set; }

        public List<Weapon> Weapons { get; private set; }

        [JsonIgnore]
        public double CarryWeight
        {
            get
            {
                double weight = 0.0;

                foreach (var item in this.Inventories)
                {
                    if (!item.IsInBagOfHolding)
                    {
                        weight += item.Weight * item.Amount;
                    }
                }

                foreach (var weapon in this.Weapons)
                {
                    weight += weapon.Weight;
                }

                weight += this.Armor.Weight;
                weight += this.Armor.ShieldWeight;

                if (Program.CcsFile.UseCoinWeight)
                {
                    weight += this.Wealth.TotalCoins / Constants.CoinGroup;
                }

                return weight;
            }
        }

        [JsonIgnore]
        public int ProficiencyBonus => this.Level - 1 > 0 ? Proficiencies[this.Level - 1] : Proficiencies[0];

        [JsonIgnore]
        public int PassivePerception => Constants.BasePerception + this.Skill.Perception.Bonus + this.Details.PerceptionBonus;

        [JsonIgnore]
        public int Initiative => Utilities.CalculateBonus(this.Attributes.Dexterity) + this.Details.InitiativeBonus;

        [JsonIgnore]
        public int Level
        {
            get
            {
                int totalLevel = 0;

                foreach (Class @class in this.Classess)
                {
                    totalLevel += @class.Level;
                }

                return totalLevel;
            }
        }

        [JsonIgnore]
        public int CasterLevel
        {
            get
            {
                int level = 0;

                foreach (var magicClass in this.MagicClasses)
                {
                    level += magicClass.Level;
                }

                return level;
            }
        }

        [JsonIgnore]
        public string ExperienceToLevel => this.Level - 1 > 0 ? Levels[this.Level - 1].ToString() : Levels[0].ToString();

        [JsonIgnore]
        public string GetClasses
        {
            get
            {
                string classes = string.Empty;

                foreach (Class @class in this.Classess)
                {
                    if (!string.IsNullOrEmpty(@class.Name))
                    {
                        classes += @class.Name + ", ";
                    }
                }

                classes = !string.IsNullOrEmpty(classes) ? classes.Remove(classes.Length - 2) : string.Empty;

                return classes;
            }
        }

        [JsonIgnore]
        public int LightCarryCapacity => this.Attributes.Strength * 5;

        [JsonIgnore]
        public int MediumCarryCapacity => this.Attributes.Strength * 10;

        [JsonIgnore]
        public int HeavyCarryCapacity => this.Attributes.Strength * 15;

        public void Reset()
        {
            this.Initialize();
        }

        public bool ValidateClassLevel(int level, Guid id)
        {
            int totalLevel = 0;

            foreach (Class @class in this.Classess)
            {
                if (!@class.ID.Equals(id))
                {
                    totalLevel += @class.Level;
                }
            }

            return totalLevel <= Constants.MaxLevel && totalLevel >= 0;
        }

        public void LongRest()
        {
            this.Vitality.ResetHealth();
            this.Vitality.RegainHitDice();
            this.SpellSlots.Reset();
        }

        public bool IsWeaponProficient(Weapon weapon)
        {
            var weaponName = Utilities.FormatName(weapon.WeaponType.ToString());

            if (weapon.ProficiencyOverride)
            {
                return true;
            }

            if (this.Proficiency.Weapons.ContainsValue(weaponName))
            {
                return true;
            }

            switch (weapon.WeaponType)
            {
                // Simple Ranged Weapons
                case WeaponTypes.LightCrossbow:
                case WeaponTypes.Dart:
                case WeaponTypes.Shortbow:
                case WeaponTypes.Sling:
                    if (this.Proficiency.Weapons.ContainsValue("Simple Ranged Weapons"))
                    {
                        return true;
                    }

                    break;

                // Simple Melee Weapons
                case WeaponTypes.Club:
                case WeaponTypes.Dagger:
                case WeaponTypes.Greatclub:
                case WeaponTypes.Handaxe:
                case WeaponTypes.Javelin:
                case WeaponTypes.LightHammer:
                case WeaponTypes.Mace:
                case WeaponTypes.Quarterstaff:
                case WeaponTypes.Sickle:
                case WeaponTypes.Spear:
                    if (this.Proficiency.Weapons.ContainsValue("Simple Melee Weapons"))
                    {
                        return true;
                    }

                    break;

                // Martial Ranged Weapons
                case WeaponTypes.Blowgun:
                case WeaponTypes.HandCrossbow:
                case WeaponTypes.HeavyCrossbow:
                case WeaponTypes.Longbow:
                case WeaponTypes.Net:
                    if (this.Proficiency.Weapons.ContainsValue("Martial Ranged Weapons"))
                    {
                        return true;
                    }

                    break;

                // Martial Melee Weapons
                case WeaponTypes.Battleaxe:
                case WeaponTypes.Flail:
                case WeaponTypes.Glaive:
                case WeaponTypes.Greataxe:
                case WeaponTypes.Greatsword:
                case WeaponTypes.Halberd:
                case WeaponTypes.Lance:
                case WeaponTypes.Longsword:
                case WeaponTypes.Maul:
                case WeaponTypes.Morningstar:
                case WeaponTypes.Pike:
                case WeaponTypes.Rapier:
                case WeaponTypes.Scimitar:
                case WeaponTypes.Shortsword:
                case WeaponTypes.Trident:
                case WeaponTypes.WarPick:
                case WeaponTypes.Warhammer:
                case WeaponTypes.Whip:
                    if (this.Proficiency.Weapons.ContainsValue("Martial Melee Weapons"))
                    {
                        return true;
                    }

                    break;
                default:
                    return false;
            }

            return false;
        }

        public Chapter GetChapterById(Guid id)
        {
            return this.Chapters.Single(x => x.ID.Equals(id));
        }

        public Inventory GetInventoryById(Guid id)
        {
            return this.Inventories.Single(x => x.ID.Equals(id));
        }

        public Ability GetAbilityById(Guid id)
        {
            return this.Abilities.Single(x => x.ID.Equals(id));
        }

        public Weapon GetWeaponById(Guid id)
        {
            return this.Weapons.Single(x => x.ID.Equals(id));
        }

        public Ammunition GetAmmunitionById(Guid id)
        {
            return this.Ammunitions.Single(x => x.ID.Equals(id));
        }

        public Spell GetSpellById(Guid id)
        {
            return this.Spells.Single(x => x.ID.Equals(id));
        }

        public MagicClass GetMagicClassById(Guid id)
        {
            return this.MagicClasses.Single(x => x.ID.Equals(id));
        }

        private void Initialize()
        {
            this.Abilities = new List<Ability>();
            this.Ammunitions = new List<Ammunition>();
            this.Appearance = new Appearance();
            this.Armor = new Armor();
            this.Attributes = new Attributes();
            this.Chapters = new List<Chapter>();
            this.Classess = new List<Class>();
            this.ClassResources = new List<ClassResource>();
            this.Companion = new Companion();
            this.Details = new Details();
            this.Inventories = new List<Inventory>();
            this.MagicClasses = new List<MagicClass>();
            this.Personality = new Personality();
            this.Proficiency = new Proficiency();
            this.SavingThrow = new SavingThrow();
            this.Skill = new Skill();
            this.Spells = new List<Spell>();
            this.SpellSlots = new SpellSlots();
            this.Vitality = new Vitality();
            this.Wealth = new Wealth();
            this.Weapons = new List<Weapon>();
        }
    }
}
