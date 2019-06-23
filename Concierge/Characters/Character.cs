using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Collections.Generic;

namespace Concierge.Characters
{
    public class Character
    {
        private static int[] Levels = { 300, 900, 2700, 6500, 14000, 23000, 34000, 48000, 64000, 85000, 100000,
                                        120000, 140000, 165000, 195000, 225000, 265000, 305000, 355000 , 0};

        private static int[] Proficiencies = { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6 };

        public Character()
        {
            Initialize();
        }

        public void Reset()
        {
            Initialize();
        }

        public bool ValidateClassLevel(int level, Guid id)
        {
            int totalLevel = 0;

            foreach (Class @class in Classess)
            {
                if (!@class.ID.Equals(id))
                {
                    totalLevel += @class.Level;
                }
            }

            return totalLevel <= Constants.MAX_LEVEL && totalLevel >= 0;
        }

        public void LongRest()
        {
            Vitality.ResetHealth();
            Vitality.RegainHitDice();
            SpellSlots.Reset();
        }

        private void Initialize()
        {
            Abilities = new List<Ability>();
            Ammunitions = new List<Ammunition>();
            Appearance = new Appearance();
            Armor = new Armor();
            Attributes = new Attributes();
            Classess = new List<Class>();
            ClassResources = new List<ClassResource>();
            Companion = new Companion();
            Details = new Details();
            Documents = new List<Document>();
            Inventories = new List<Inventory>();
            MagicClasses = new List<MagicClass>();
            Personality = new Personality();
            Proficiency = new Proficiency();
            SavingThrow = new SavingThrow();
            Skill = new Skill();
            Spells = new List<Spell>();
            SpellSlots = new SpellSlots();
            Vitality = new Vitality();
            Wealth = new Wealth();
            Weapons = new List<Weapon>();
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
        public List<Document> Documents { get; set; }
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

        public double CarryWeight
        {
            get
            {
                double weight = 0.0;

                foreach (Inventory item in Inventories)
                {
                    weight += item.Weight * item.Amount;
                }

                foreach (Weapon weapon in Weapons)
                {
                    weight += weapon.Weight;
                }

                weight += Armor.Weight;
                weight += Armor.ShieldWeight;

                if (Settings.UseCoinWeight)
                {
                    weight += (Wealth.TotalCoins / Constants.COIN_GROUP);
                }

                return weight;
            }
        }

        public int ProficiencyBonus
        {
            get
            {
                if (Level - 1 > 0)
                {
                    return Proficiencies[Level - 1];
                }

                return Proficiencies[0];
            }
        }

        public int PassivePerception
        {
            get
            {
                return Constants.BASE_PERCEPTION + Skill.Perception.Bonus + Details.PerceptionBonus;
            }
        }

        public int Initiative
        {
            get
            {
                return Constants.CalculateBonus(Attributes.Dexterity) + Details.InitiativeBonus;
            }
        }

        public int Level
        {
            get
            {
                int totalLevel = 0;

                foreach (Class @class in Classess)
                {
                    totalLevel += @class.Level;
                }

                return totalLevel;
            }
        }

        public string ExperienceToLevel
        {
            get
            {
                if (Level - 1 > 0)
                {
                    return Levels[Level - 1].ToString();
                }

                return Levels[0].ToString();
            }
        }

        public string GetClasses
        {
            get
            {
                string classes = "";

                foreach (Class @class in Classess)
                {
                    classes += @class.Name + ", ";
                }

                classes = !string.IsNullOrEmpty(classes) ? classes.Remove(classes.Length - 2) : "";

                return classes;
            }
        }

        public int LightCarryCapacity
        {
            get
            {
                return Attributes.Strength * 5;
            }
        }

        public int MediumCarryCapacity
        {
            get
            {
                return Attributes.Strength * 10;
            }
        }

        public int HeavyCarryCapacity
        {
            get
            {
                return Attributes.Strength * 15;
            }
        }
    }
}
