// <copyright file="ConciergeCharacter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Character.AbilitySavingThrows;
    using Concierge.Character.AbilitySkills;
    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Character.Notes;
    using Concierge.Character.Spellcasting;
    using Concierge.Character.Statuses;
    using Concierge.Utility;
    using Concierge.Utility.Units;
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

        public List<ClassResource> ClassResources { get; private set; }

        public Companion Companion { get; private set; }

        public Senses Senses { get; private set; }

        public List<Chapter> Chapters { get; set; }

        public List<Inventory> Inventories { get; private set; }

        public List<MagicClass> MagicClasses { get; private set; }

        public Personality Personality { get; private set; }

        public List<Proficiency> Proficiency { get; set; }

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

        public List<StatusEffect> StatusEffects { get; private set; }

        public CharacterProperties Properties { get; private set; }

        public List<Language> Languages { get; set; }

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
                        weight += item.Weight.Value * item.Amount;
                    }
                }

                foreach (var weapon in this.Weapons)
                {
                    if (!weapon.IsInBagOfHolding)
                    {
                        weight += weapon.Weight.Value;
                    }
                }

                weight += this.Armor.Weight.Value;
                weight += this.Armor.ShieldWeight.Value;

                if (ConciergeSettings.UseCoinWeight)
                {
                    weight += UnitConvertion.Weight(ConciergeSettings.UnitOfMeasurement, this.Wealth.TotalCoins / Constants.CoinGroup);
                }

                weight += this.EquippedItems.Weight;

                return Math.Round(weight, Constants.SignificantDigits);
            }
        }

        [JsonIgnore]
        public int ProficiencyBonus => this.Properties.Level - 1 > 0 ? Constants.Proficiencies[this.Properties.Level - 1] : Constants.Proficiencies[0];

        [JsonIgnore]
        public int PassivePerception => Constants.BasePerception + this.Skill.Perception.Bonus + this.Senses.PerceptionBonus;

        [JsonIgnore]
        public int Initiative => Utilities.CalculateBonus(this.Attributes.Dexterity) + this.Senses.InitiativeBonus;

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
        public double LightCarryCapacity => this.Attributes.Strength * UnitConvertion.LightMultiplier;

        [JsonIgnore]
        public double MediumCarryCapacity => this.Attributes.Strength * UnitConvertion.MediumMultiplier;

        [JsonIgnore]
        public double HeavyCarryCapacity => this.Attributes.Strength * UnitConvertion.HeavyMultiplier;

        public void Reset()
        {
            this.Initialize();
        }

        public void LongRest()
        {
            this.Vitality.ResetHealth();
            this.Vitality.RegainHitDice();
            this.Vitality.ResetDeathSaves();
            this.SpellSlots.Reset();

            this.Companion.Vitality.ResetHealth();
            this.Companion.Vitality.RegainHitDice();
        }

        public bool IsWeaponProficient(Weapon weapon)
        {
            var weaponName = Utilities.FormatName(weapon.WeaponType.ToString());

            return weapon.ProficiencyOverride
                ? true
                : this.Proficiency.Any(x => x.Name.Equals(weaponName) && x.ProficiencyType == ProficiencyTypes.Weapon) ? true
                : weapon.WeaponType switch
            {
                // Simple Ranged Weapons
                WeaponTypes.LightCrossbow or WeaponTypes.Dart or WeaponTypes.Shortbow or WeaponTypes.Sling => this.Proficiency.Any(x => x.Name.Equals("Simple Ranged Weapons") && x.ProficiencyType == ProficiencyTypes.Weapon),

                // Simple Melee Weapons
                WeaponTypes.Club or WeaponTypes.Dagger or WeaponTypes.Greatclub or WeaponTypes.Handaxe or WeaponTypes.Javelin or WeaponTypes.LightHammer or WeaponTypes.Mace or WeaponTypes.Quarterstaff or WeaponTypes.Sickle or WeaponTypes.Spear => this.Proficiency.Any(x => x.Name.Equals("Simple Melee Weapons") && x.ProficiencyType == ProficiencyTypes.Weapon),

                // Martial Ranged Weapons
                WeaponTypes.Blowgun or WeaponTypes.HandCrossbow or WeaponTypes.HeavyCrossbow or WeaponTypes.Longbow or WeaponTypes.Net => this.Proficiency.Any(x => x.Name.Equals("Martial Ranged Weapons") && x.ProficiencyType == ProficiencyTypes.Weapon),

                // Martial Melee Weapons
                WeaponTypes.Battleaxe or WeaponTypes.Flail or WeaponTypes.Glaive or WeaponTypes.Greataxe or WeaponTypes.Greatsword or WeaponTypes.Halberd or WeaponTypes.Lance or WeaponTypes.Longsword or WeaponTypes.Maul or WeaponTypes.Morningstar or WeaponTypes.Pike or WeaponTypes.Rapier or WeaponTypes.Scimitar or WeaponTypes.Shortsword or WeaponTypes.Trident or WeaponTypes.WarPick or WeaponTypes.Warhammer or WeaponTypes.Whip => this.Proficiency.Any(x => x.Name.Equals("Martial Melee Weapons") && x.ProficiencyType == ProficiencyTypes.Weapon),
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
            this.ClassResources = new List<ClassResource>();
            this.Companion = new Companion();
            this.Senses = new Senses();
            this.Inventories = new List<Inventory>();
            this.MagicClasses = new List<MagicClass>();
            this.Personality = new Personality();
            this.Proficiency = new List<Proficiency>();
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
            this.StatusEffects = new List<StatusEffect>();
            this.Properties = new CharacterProperties();
            this.Languages = new List<Language>();
        }
    }
}
