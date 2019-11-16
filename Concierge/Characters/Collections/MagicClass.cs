using Concierge.Utility;
using System;
using System.Linq;

namespace Concierge.Characters.Collections
{
    public class MagicClass
    {
        public MagicClass()
        {
            ID = Guid.NewGuid();
        }

        public MagicClass(Guid id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public Constants.Abilities Ability { get; set; }
        public int Level { get; set; }
        public int KnownCantrips { get; set; }
        public int KnownSpells { get; set; }
        public Guid ID { get; private set; }

        public int PreparedSpells
        {
            get
            {
                return Program.Character.Spells.Where(x => x.Class?.Equals(Name) ?? false && x.Prepared).ToList().Count;
            }
        }

        public int Attack
        {
            get
            {
                int bonus = Program.Character.ProficiencyBonus;

                switch (Ability)
                {
                    default:
                    case Constants.Abilities.STR:
                        return Constants.CalculateBonus(Program.Character.Attributes.Strength) + bonus;
                    case Constants.Abilities.DEX:
                        return Constants.CalculateBonus(Program.Character.Attributes.Dexterity) + bonus;
                    case Constants.Abilities.CON:
                        return Constants.CalculateBonus(Program.Character.Attributes.Constitution) + bonus;
                    case Constants.Abilities.INT:
                        return Constants.CalculateBonus(Program.Character.Attributes.Intelligence) + bonus;
                    case Constants.Abilities.WIS:
                        return Constants.CalculateBonus(Program.Character.Attributes.Wisdom) + bonus;
                    case Constants.Abilities.CHA:
                        return Constants.CalculateBonus(Program.Character.Attributes.Charisma) + bonus;
                }
            }
        }

        public int Save
        {
            get
            {
                int bonus = Program.Character.ProficiencyBonus + Constants.BASE_DC;

                switch (Ability)
                {
                    default:
                    case Constants.Abilities.STR:
                        return Constants.CalculateBonus(Program.Character.Attributes.Strength) + bonus;
                    case Constants.Abilities.DEX:
                        return Constants.CalculateBonus(Program.Character.Attributes.Dexterity) + bonus;
                    case Constants.Abilities.CON:
                        return Constants.CalculateBonus(Program.Character.Attributes.Constitution) + bonus;
                    case Constants.Abilities.INT:
                        return Constants.CalculateBonus(Program.Character.Attributes.Intelligence) + bonus;
                    case Constants.Abilities.WIS:
                        return Constants.CalculateBonus(Program.Character.Attributes.Wisdom) + bonus;
                    case Constants.Abilities.CHA:
                        return Constants.CalculateBonus(Program.Character.Attributes.Charisma) + bonus;
                }
            }
        }
    }
}
