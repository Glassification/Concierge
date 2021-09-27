// <copyright file="ConciergeCharacter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Concierge.Character.AbilitySavingThrows;
    using Concierge.Character.AbilitySkills;
    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Character.Notes;
    using Concierge.Character.Spellcasting;
    using Concierge.Character.Statuses;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public class ConciergeCharacter
    {
        public ConciergeCharacter()
        {
            this.Initialize();
        }

        public List<Ability> Abilities { get; set; }

        public List<Ammunition> Ammunitions { get; private set; }

        public Appearance Appearance { get; private set; }

        public Armor Armor { get; private set; }

        public Attributes Attributes { get; private set; }

        public CharacterClass Class1 { get; set; }

        public CharacterClass Class2 { get; set; }

        public CharacterClass Class3 { get; set; }

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

        public EquippedItems EquippedItems { get; private set; }

        public CharacterImage CharacterImage { get; private set; }

        public CharacterImage CharacterIcon { get; private set; }

        [JsonIgnore]
        public double CarryWeight
        {
            get
            {
                var weight = 0.0;

                foreach (var item in this.Inventories)
                {
                    if (!item.IsInBagOfHolding)
                    {
                        weight += item.Weight * item.Amount;
                    }
                }

                foreach (var weapon in this.Weapons)
                {
                    if (!weapon.IsInBagOfHolding)
                    {
                        weight += weapon.Weight;
                    }
                }

                weight += this.Armor.Weight;
                weight += this.Armor.ShieldWeight;

                if (ConciergeSettings.UseCoinWeight)
                {
                    weight += this.Wealth.TotalCoins / Constants.CoinGroup;
                }

                weight += this.EquippedItems.Weight;

                return Math.Round(weight, Constants.SignificantDigits);
            }
        }

        [JsonIgnore]
        public int ProficiencyBonus => this.Level - 1 > 0 ? Constants.Proficiencies[this.Level - 1] : Constants.Proficiencies[0];

        [JsonIgnore]
        public int PassivePerception => Constants.BasePerception + this.Skill.Perception.Bonus + this.Details.PerceptionBonus;

        [JsonIgnore]
        public int Initiative => Utilities.CalculateBonus(this.Attributes.Dexterity) + this.Details.InitiativeBonus;

        [JsonIgnore]
        public int Level => this.Class1.Level + this.Class2.Level + this.Class3.Level;

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
        public string ExperienceToLevel => this.Level - 1 > 0 ? Constants.Levels[this.Level - 1].ToString() : Constants.Levels[0].ToString();

        [JsonIgnore]
        public string GetClasses
        {
            get
            {
                var classes = new StringBuilder();

                classes.Append(this.Class1.Name.IsNullOrWhiteSpace() ? string.Empty : $"{this.Class1.Name}, ");
                classes.Append(this.Class2.Name.IsNullOrWhiteSpace() ? string.Empty : $"{this.Class2.Name}, ");
                classes.Append(this.Class3.Name.IsNullOrWhiteSpace() ? string.Empty : this.Class3.Name);

                var classString = classes.ToString().Trim(new char[] { ' ', ',' });

                return classString;
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

        public void LongRest()
        {
            this.Vitality.ResetHealth();
            this.Vitality.RegainHitDice();
            this.SpellSlots.Reset();
        }

        public bool IsWeaponProficient(Weapon weapon)
        {
            var weaponName = Utilities.FormatName(weapon.WeaponType.ToString());

            return weapon.ProficiencyOverride
                ? true
                : this.Proficiency.Weapons.ContainsValue(weaponName)
                ? true
                : weapon.WeaponType switch
            {
                // Simple Ranged Weapons
                WeaponTypes.LightCrossbow or WeaponTypes.Dart or WeaponTypes.Shortbow or WeaponTypes.Sling => this.Proficiency.Weapons.ContainsValue("Simple Ranged Weapons"),

                // Simple Melee Weapons
                WeaponTypes.Club or WeaponTypes.Dagger or WeaponTypes.Greatclub or WeaponTypes.Handaxe or WeaponTypes.Javelin or WeaponTypes.LightHammer or WeaponTypes.Mace or WeaponTypes.Quarterstaff or WeaponTypes.Sickle or WeaponTypes.Spear => this.Proficiency.Weapons.ContainsValue("Simple Melee Weapons"),

                // Martial Ranged Weapons
                WeaponTypes.Blowgun or WeaponTypes.HandCrossbow or WeaponTypes.HeavyCrossbow or WeaponTypes.Longbow or WeaponTypes.Net => this.Proficiency.Weapons.ContainsValue("Martial Ranged Weapons"),

                // Martial Melee Weapons
                WeaponTypes.Battleaxe or WeaponTypes.Flail or WeaponTypes.Glaive or WeaponTypes.Greataxe or WeaponTypes.Greatsword or WeaponTypes.Halberd or WeaponTypes.Lance or WeaponTypes.Longsword or WeaponTypes.Maul or WeaponTypes.Morningstar or WeaponTypes.Pike or WeaponTypes.Rapier or WeaponTypes.Scimitar or WeaponTypes.Shortsword or WeaponTypes.Trident or WeaponTypes.WarPick or WeaponTypes.Warhammer or WeaponTypes.Whip => this.Proficiency.Weapons.ContainsValue("Martial Melee Weapons"),
                _ => false,
            };
        }

        public Chapter GetChapterByDocumentId(Guid id)
        {
            return this.Chapters.Single(x => x.Documents.Any(y => y.Id.Equals(id)));
        }

        private void Initialize()
        {
            this.Abilities = new List<Ability>();
            this.Ammunitions = new List<Ammunition>();
            this.Appearance = new Appearance();
            this.Armor = new Armor();
            this.Attributes = new Attributes();
            this.Chapters = new List<Chapter>();
            this.Class1 = new CharacterClass();
            this.Class2 = new CharacterClass();
            this.Class3 = new CharacterClass();
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
            this.EquippedItems = new EquippedItems();
            this.CharacterImage = new CharacterImage();
            this.CharacterIcon = new CharacterImage();
        }
    }
}
