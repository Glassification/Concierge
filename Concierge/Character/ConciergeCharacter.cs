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
    using Concierge.Commands;
    using Concierge.Configuration;
    using Concierge.Leveling;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Units;
    using Concierge.Utility.Utilities;
    using Newtonsoft.Json;

    public sealed class ConciergeCharacter : ICopyable<ConciergeCharacter>
    {
        private readonly ConciergeLeveler conciergeLeveler;

        public ConciergeCharacter()
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
            this.Proficiencies = new List<Proficiency>();
            this.SavingThrow = new SavingThrow();
            this.Skill = new Skill();
            this.Spells = new List<Spell>();
            this.SpellSlots = new SpellSlots();
            this.Vitality = new Vitality();
            this.Wealth = new Wealth();
            this.Weapons = new List<Weapon>();
            this.EquippedItems = new EquippedItems();
            this.CharacterImage = new CharacterImage();
            this.StatusEffects = new List<StatusEffect>();
            this.Properties = new CharacterProperties();
            this.Languages = new List<Language>();

            this.conciergeLeveler = new ConciergeLeveler(this);
        }

        public List<Ability> Abilities { get; set; }

        public List<Ammunition> Ammunitions { get; set; }

        public Appearance Appearance { get; set; }

        public Armor Armor { get; set; }

        public Attributes Attributes { get; set; }

        public List<Chapter> Chapters { get; set; }

        public CharacterImage CharacterImage { get; set; }

        public List<ClassResource> ClassResources { get; set; }

        public Companion Companion { get; set; }

        public EquippedItems EquippedItems { get; set; }

        public List<Inventory> Inventories { get; set; }

        public List<Language> Languages { get; set; }

        public List<MagicClass> MagicClasses { get; set; }

        public Personality Personality { get; set; }

        public List<Proficiency> Proficiencies { get; set; }

        public CharacterProperties Properties { get; set; }

        public SavingThrow SavingThrow { get; set; }

        public Senses Senses { get; set; }

        public Skill Skill { get; set; }

        public List<Spell> Spells { get; set; }

        public SpellSlots SpellSlots { get; set; }

        public List<StatusEffect> StatusEffects { get; set; }

        public Vitality Vitality { get; set; }

        public Wealth Wealth { get; set; }

        public List<Weapon> Weapons { get; set; }

        [JsonIgnore]
        public double CarryWeight
        {
            get
            {
                var weight = 0.0;

                foreach (var item in this.Inventories)
                {
                    if (!item.IgnoreWeight)
                    {
                        weight += item.Weight.Value * item.Amount;
                    }
                }

                foreach (var weapon in this.Weapons)
                {
                    if (!weapon.IgnoreWeight)
                    {
                        weight += weapon.Weight.Value;
                    }
                }

                weight += this.Armor.Weight.Value;
                weight += this.Armor.ShieldWeight.Value;

                if (AppSettingsManager.UserSettings.UseCoinWeight)
                {
                    weight += UnitConvertion.Weight(AppSettingsManager.UserSettings.UnitOfMeasurement, this.Wealth.TotalCoins / Constants.CoinGroup);
                }

                weight += this.EquippedItems.Weight;

                return weight;
            }
        }

        [JsonIgnore]
        public int ProficiencyBonus => this.Properties.Level - 1 > 0 ? Constants.ProficiencyLevels[this.Properties.Level - 1] : Constants.ProficiencyLevels[0];

        [JsonIgnore]
        public int PassivePerception => Constants.BasePerception + this.Skill.Perception.Bonus + this.Senses.PerceptionBonus;

        [JsonIgnore]
        public int Initiative => CharacterUtility.CalculateBonus(this.Attributes.Dexterity) + this.Senses.InitiativeBonus;

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

        public override bool Equals(object? obj)
        {
            if (obj is not ConciergeCharacter)
            {
                return false;
            }

            var character1 = JsonConvert.SerializeObject(obj as ConciergeCharacter, Formatting.Indented);
            var character2 = JsonConvert.SerializeObject(this, Formatting.Indented);

            return character1.Equals(character2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void LongRest()
        {
            var oldVitality = this.Vitality.DeepCopy();
            var oldSpellSlots = this.SpellSlots.DeepCopy();
            var oldCompanionVitality = this.Companion.Vitality.DeepCopy();

            this.Vitality.ResetHealth();
            this.Vitality.RegainHitDice();
            this.Vitality.ResetDeathSaves();
            this.SpellSlots.Reset();

            this.Companion.Vitality.ResetHealth();
            this.Companion.Vitality.RegainHitDice();

            Program.UndoRedoService.AddCommand(
                new LongRestCommand(
                    oldVitality,
                    oldCompanionVitality,
                    oldSpellSlots,
                    this.Vitality.DeepCopy(),
                    this.Companion.Vitality.DeepCopy(),
                    this.SpellSlots.DeepCopy()));
        }

        public void LevelUp(HitDie hitDie, int classNumber, int bonusHp)
        {
            this.conciergeLeveler.LevelUp(hitDie, classNumber, bonusHp);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0075:Simplify conditional expression", Justification = "Increase readability.")]
        public bool IsWeaponProficient(Weapon weapon)
        {
            var weaponName = StringUtility.FormatName(weapon.Type.ToString());

            return weapon.ProficiencyOverride
                ? true
                : this.Proficiencies.Any(x => x.Name.Equals(weaponName) && x.ProficiencyType == ProficiencyTypes.Weapon) ? true
                : weapon.Type switch
            {
                // Simple Ranged Weapons
                WeaponTypes.LightCrossbow or WeaponTypes.Dart or WeaponTypes.Shortbow or WeaponTypes.Sling => this.Proficiencies.Any(x => x.Name.Equals(Proficiency.SimpleRanged) && x.ProficiencyType == ProficiencyTypes.Weapon),

                // Simple Melee Weapons
                WeaponTypes.Club or WeaponTypes.Dagger or WeaponTypes.Greatclub or WeaponTypes.Handaxe or WeaponTypes.Javelin or WeaponTypes.LightHammer or WeaponTypes.Mace or WeaponTypes.Quarterstaff or WeaponTypes.Sickle or WeaponTypes.Spear => this.Proficiencies.Any(x => x.Name.Equals(Proficiency.SimpleMelee) && x.ProficiencyType == ProficiencyTypes.Weapon),

                // Martial Ranged Weapons
                WeaponTypes.Blowgun or WeaponTypes.HandCrossbow or WeaponTypes.HeavyCrossbow or WeaponTypes.Longbow or WeaponTypes.Net => this.Proficiencies.Any(x => x.Name.Equals(Proficiency.MartialRanged) && x.ProficiencyType == ProficiencyTypes.Weapon),

                // Martial Melee Weapons
                WeaponTypes.Battleaxe or WeaponTypes.Flail or WeaponTypes.Glaive or WeaponTypes.Greataxe or WeaponTypes.Greatsword or WeaponTypes.Halberd or WeaponTypes.Lance or WeaponTypes.Longsword or WeaponTypes.Maul or WeaponTypes.Morningstar or WeaponTypes.Pike or WeaponTypes.Rapier or WeaponTypes.Scimitar or WeaponTypes.Shortsword or WeaponTypes.Trident or WeaponTypes.WarPick or WeaponTypes.Warhammer or WeaponTypes.Whip => this.Proficiencies.Any(x => x.Name.Equals(Proficiency.MartialMelee) && x.ProficiencyType == ProficiencyTypes.Weapon),
                _ => false,
            };
        }

        public Chapter GetChapter(Guid documentId)
        {
            return this.Chapters.Single(x => x.Documents.Any(y => y.Id.Equals(documentId)));
        }

        public ConciergeCharacter DeepCopy()
        {
            return new ConciergeCharacter()
            {
                Abilities = this.Abilities.DeepCopy().ToList(),
                Ammunitions = this.Ammunitions.DeepCopy().ToList(),
                Appearance = this.Appearance.DeepCopy(),
                Armor = this.Armor.DeepCopy(),
                Attributes = this.Attributes.DeepCopy(),
                Chapters = this.Chapters.DeepCopy().ToList(),
                ClassResources = this.ClassResources.DeepCopy().ToList(),
                Companion = this.Companion.DeepCopy(),
                Senses = this.Senses.DeepCopy(),
                Inventories = this.Inventories.DeepCopy().ToList(),
                MagicClasses = this.MagicClasses.DeepCopy().ToList(),
                Personality = this.Personality.DeepCopy(),
                Proficiencies = this.Proficiencies.DeepCopy().ToList(),
                SavingThrow = this.SavingThrow.DeepCopy(),
                Skill = this.Skill.DeepCopy(),
                Spells = this.Spells.DeepCopy().ToList(),
                SpellSlots = this.SpellSlots.DeepCopy(),
                Vitality = this.Vitality.DeepCopy(),
                Wealth = this.Wealth.DeepCopy(),
                Weapons = this.Weapons.DeepCopy().ToList(),
                EquippedItems = this.EquippedItems.DeepCopy(),
                CharacterImage = this.CharacterImage.DeepCopy(),
                StatusEffects = this.StatusEffects.DeepCopy().ToList(),
                Properties = this.Properties.DeepCopy(),
                Languages = this.Languages.DeepCopy().ToList(),
            };
        }
    }
}
