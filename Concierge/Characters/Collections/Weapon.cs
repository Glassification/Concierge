using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters.Collections
{
    public class Weapon
    {
        public Weapon()
        {
            ID = Guid.NewGuid();
        }

        public Weapon(Guid id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public int Attack
        {
            get
            {
                switch (Ability)
                {
                    default:
                    case Constants.Abilities.STR:
                        return Constants.CalculateBonus(Program.Character.Attributes.Strength) + Program.Character.ProficiencyBonus;
                    case Constants.Abilities.DEX:
                        return Constants.CalculateBonus(Program.Character.Attributes.Dexterity) + Program.Character.ProficiencyBonus;
                    case Constants.Abilities.CON:
                        return Constants.CalculateBonus(Program.Character.Attributes.Constitution) + Program.Character.ProficiencyBonus;
                    case Constants.Abilities.INT:
                        return Constants.CalculateBonus(Program.Character.Attributes.Intelligence) + Program.Character.ProficiencyBonus;
                    case Constants.Abilities.WIS:
                        return Constants.CalculateBonus(Program.Character.Attributes.Wisdom) + Program.Character.ProficiencyBonus;
                    case Constants.Abilities.CHA:
                        return Constants.CalculateBonus(Program.Character.Attributes.Charisma) + Program.Character.ProficiencyBonus;
                }
            }
        }
        public Constants.Abilities Ability { get; set; }
        public string Damage { get; set; }
        public string Misc { get; set; }
        public Constants.DamageTypes DamageType { get; set; }
        public string Range { get; set; }
        public string Note { get; set; }
        public double Weight { get; set; }
        public Guid ID { get; private set; }
    }
}
