// <copyright file="ConciergeCharacter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System.Linq;

    using Concierge.Character.AbilitySaves;
    using Concierge.Character.AbilitySkills;
    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Character.Journals;
    using Concierge.Character.Spellcasting;
    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Common.Utilities;
    using Concierge.Data.Units;
    using Concierge.Leveling;
    using Newtonsoft.Json;

    public sealed class ConciergeCharacter : ICopyable<ConciergeCharacter>, ICreature
    {
        private readonly ConciergeLeveler conciergeLeveler;

        public ConciergeCharacter()
        {
            this.Journal = new Journal();
            this.Companion = new Companion();
            this.SavingThrows = new SavingThrows();
            this.Skills = new Skills();
            this.Vitality = new Vitality();
            this.Wealth = new Wealth();
            this.CharacterImage = new CharacterImage();
            this.Properties = new CharacterProperties();
            this.Magic = new Magic();
            this.Characteristic = new Characteristic();
            this.Equipment = new Equipment();

            this.conciergeLeveler = new ConciergeLeveler(this);
        }

        public Characteristic Characteristic { get; set; }

        public Companion Companion { get; set; }

        public Journal Journal { get; set; }

        public Magic Magic { get; set; }

        public CharacterProperties Properties { get; set; }

        public CharacterImage CharacterImage { get; set; }

        public SavingThrows SavingThrows { get; set; }

        public Skills Skills { get; set; }

        public Vitality Vitality { get; set; }

        public Wealth Wealth { get; set; }

        public Equipment Equipment { get; set; }

        [JsonIgnore]
        public int ProficiencyBonus => this.Properties.Level - 1 > 0 ? Defaults.ProficiencyLevels[this.Properties.Level - 1] : Defaults.ProficiencyLevels[0];

        [JsonIgnore]
        public int PassivePerception => Constants.BasePerception + this.Skills.Perception.Bonus + this.Characteristic.Senses.PerceptionBonus;

        [JsonIgnore]
        public int Initiative => Constants.Bonus(this.Characteristic.Attributes.Dexterity) + this.Characteristic.Senses.InitiativeBonus;

        [JsonIgnore]
        public double LightCarryCapacity => this.Characteristic.Attributes.Strength * UnitConversion.LightMultiplier;

        [JsonIgnore]
        public double MediumCarryCapacity => this.Characteristic.Attributes.Strength * UnitConversion.MediumMultiplier;

        [JsonIgnore]
        public double HeavyCarryCapacity => this.Characteristic.Attributes.Strength * UnitConversion.HeavyMultiplier;

        [JsonIgnore]
        public CreatureType CreatureType => CreatureType.Character;

        public override bool Equals(object? obj)
        {
            if (obj is not ConciergeCharacter)
            {
                return false;
            }

            var character1 = JsonConvert.SerializeObject(obj as ConciergeCharacter);
            var character2 = JsonConvert.SerializeObject(this);

            return character1.Equals(character2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CalculateBonusFromAbility(Abilities ability)
        {
            return ability switch
            {
                Abilities.STR => Constants.Bonus(this.Characteristic.Attributes.Strength) + this.ProficiencyBonus,
                Abilities.DEX => Constants.Bonus(this.Characteristic.Attributes.Dexterity) + this.ProficiencyBonus,
                Abilities.CON => Constants.Bonus(this.Characteristic.Attributes.Constitution) + this.ProficiencyBonus,
                Abilities.INT => Constants.Bonus(this.Characteristic.Attributes.Intelligence) + this.ProficiencyBonus,
                Abilities.WIS => Constants.Bonus(this.Characteristic.Attributes.Wisdom) + this.ProficiencyBonus,
                Abilities.CHA => Constants.Bonus(this.Characteristic.Attributes.Charisma) + this.ProficiencyBonus,
                Abilities.NONE => this.ProficiencyBonus,
                _ => this.ProficiencyBonus,
            };
        }

        public bool ValidateClassLevel(int number)
        {
            var totalLevel =
                (this.Properties.Class1.ClassNumber == number ? 0 : this.Properties.Class1.Level) +
                (this.Properties.Class2.ClassNumber == number ? 0 : this.Properties.Class2.Level) +
                (this.Properties.Class3.ClassNumber == number ? 0 : this.Properties.Class3.Level);

            return totalLevel is <= Constants.MaxLevel and >= 0;
        }

        public void LongRest()
        {
            var oldVitality = this.Vitality.DeepCopy();
            var oldSpellSlots = this.Magic.SpellSlots.DeepCopy();
            var oldCompanionVitality = this.Companion.Vitality.DeepCopy();

            this.Vitality.ResetHealth();
            this.Vitality.RegainHitDice();
            this.Vitality.ResetDeathSaves();
            this.Magic.SpellSlots.Reset();

            this.Companion.Vitality.ResetHealth();
            this.Companion.Vitality.RegainHitDice();

            Program.UndoRedoService.AddCommand(
                new LongRestCommand(
                    oldVitality,
                    oldCompanionVitality,
                    oldSpellSlots,
                    this.Vitality.DeepCopy(),
                    this.Companion.Vitality.DeepCopy(),
                    this.Magic.SpellSlots.DeepCopy()));
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
                : this.Characteristic.Proficiencies.Any(x => x.Name.Equals(weaponName) && x.ProficiencyType == ProficiencyTypes.Weapon) ? true
                : weapon.Type switch
            {
                // Simple Ranged Weapons
                WeaponTypes.LightCrossbow or WeaponTypes.Dart or WeaponTypes.Shortbow or WeaponTypes.Sling => this.Characteristic.Proficiencies.Any(x => x.Name.Equals(Proficiency.SimpleRanged) && x.ProficiencyType == ProficiencyTypes.Weapon),

                // Simple Melee Weapons
                WeaponTypes.Club or WeaponTypes.Dagger or WeaponTypes.Greatclub or WeaponTypes.Handaxe or WeaponTypes.Javelin or WeaponTypes.LightHammer or WeaponTypes.Mace or WeaponTypes.Quarterstaff or WeaponTypes.Sickle or WeaponTypes.Spear => this.Characteristic.Proficiencies.Any(x => x.Name.Equals(Proficiency.SimpleMelee) && x.ProficiencyType == ProficiencyTypes.Weapon),

                // Martial Ranged Weapons
                WeaponTypes.Blowgun or WeaponTypes.HandCrossbow or WeaponTypes.HeavyCrossbow or WeaponTypes.Longbow or WeaponTypes.Net => this.Characteristic.Proficiencies.Any(x => x.Name.Equals(Proficiency.MartialRanged) && x.ProficiencyType == ProficiencyTypes.Weapon),

                // Martial Melee Weapons
                WeaponTypes.Battleaxe or WeaponTypes.Flail or WeaponTypes.Glaive or WeaponTypes.Greataxe or WeaponTypes.Greatsword or WeaponTypes.Halberd or WeaponTypes.Lance or WeaponTypes.Longsword or WeaponTypes.Maul or WeaponTypes.Morningstar or WeaponTypes.Pike or WeaponTypes.Rapier or WeaponTypes.Scimitar or WeaponTypes.Shortsword or WeaponTypes.Trident or WeaponTypes.WarPick or WeaponTypes.Warhammer or WeaponTypes.Whip => this.Characteristic.Proficiencies.Any(x => x.Name.Equals(Proficiency.MartialMelee) && x.ProficiencyType == ProficiencyTypes.Weapon),
                _ => false,
            };
        }

        public ConciergeCharacter DeepCopy()
        {
            return new ConciergeCharacter()
            {
                Journal = this.Journal.DeepCopy(),
                Companion = this.Companion.DeepCopy(),
                SavingThrows = this.SavingThrows.DeepCopy(),
                Skills = this.Skills.DeepCopy(),
                Vitality = this.Vitality.DeepCopy(),
                Wealth = this.Wealth.DeepCopy(),
                CharacterImage = this.CharacterImage.DeepCopy(),
                Properties = this.Properties.DeepCopy(),
                Magic = this.Magic.DeepCopy(),
                Characteristic = this.Characteristic.DeepCopy(),
                Equipment = this.Equipment.DeepCopy(),
            };
        }
    }
}
