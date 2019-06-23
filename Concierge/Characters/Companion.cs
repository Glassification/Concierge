using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class Companion
    {
        public Companion()
        {
            Name = "";
            ArmorClass = "";
            HitDice = "";
            Health = "";
            CurrentHealth = "";
            Speed = "";
            Strength = 0;
            Dexterity = 0;
            Constitution = 0;
            Intelligence = 0;
            Wisdom = 0;
            Charisma = 0;
            Perception = "";
            Senses = "";
            Attack = "";
            Type = "";
            AttackBonus = "";
            Damage = "";
            DamageType = "";
            Reach = "";
            Notes = "";
        }

        public string Name { get; set; }

        public string ArmorClass { get; set; }

        public string HitDice { get; set; }

        public string Health { get; set; }

        public string CurrentHealth { get; set; }

        public string Speed { get; set; }

        public int Strength { get; set; }

        public int Dexterity { get; set; }

        public int Constitution { get; set; }

        public int Intelligence { get; set; }

        public int Wisdom { get; set; }

        public int Charisma { get; set; }


        public string Perception { get; set; }

        public string Senses { get; set; }

        public string Attack { get; set; }

        public string Type { get; set; }

        public string AttackBonus { get; set; }

        public string Damage { get; set; }

        public string DamageType { get; set; }

        public string Reach { get; set; }

        public string Notes { get; set; }
    }
}
