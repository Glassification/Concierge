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
            ProficiencyOverride = false;
        }

        public Weapon(Guid id)
        {
            ID = id;
            ProficiencyOverride = false;
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
                int bonus = 0;

                if (Program.Character.IsWeaponProficient(this))
                    bonus = Program.Character.ProficiencyBonus;

                switch (Ability)
                {
                    default:
                    case Constants.Abilities.STR:
                        return Utilities.CalculateBonus(Program.Character.Attributes.Strength) + bonus;
                    case Constants.Abilities.DEX:
                        return Utilities.CalculateBonus(Program.Character.Attributes.Dexterity) + bonus;
                    case Constants.Abilities.CON:
                        return Utilities.CalculateBonus(Program.Character.Attributes.Constitution) + bonus;
                    case Constants.Abilities.INT:
                        return Utilities.CalculateBonus(Program.Character.Attributes.Intelligence) + bonus;
                    case Constants.Abilities.WIS:
                        return Utilities.CalculateBonus(Program.Character.Attributes.Wisdom) + bonus;
                    case Constants.Abilities.CHA:
                        return Utilities.CalculateBonus(Program.Character.Attributes.Charisma) + bonus;
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
        public Constants.WeaponTypes WeaponType { get; set; }
        public bool ProficiencyOverride { get; set; }
        public Guid ID { get; private set; }
    }
}
