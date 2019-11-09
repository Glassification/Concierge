using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class Details
    {
        public Details()
        {
            Name = "";
            Race = "";
            Background = "";
            Alignment = "";
            Experience = "";
            Languages = new Dictionary<Guid, string>();
            InitiativeBonus = 0;
            PerceptionBonus = 0;
            BaseMovement = 0;
            Vision = "";
        }

        public string Name { get; set; }

        public string Race { get; set; }

        public string Background { get; set; }

        public string Alignment { get; set; }

        public string Experience { get; set; }

        public Dictionary<Guid, string> Languages { get; set; }

        public int InitiativeBonus { get; set; }

        public int PerceptionBonus { get; set; }

        public int BaseMovement { get; set; }

        public string Vision { get; set; }

        public int Movement
        {
            get
            {
                int newMovement = BaseMovement;

                if (Program.Character.Vitality.Conditions.Fatigued.Equals("Five") ||
                    Program.Character.Vitality.Conditions.Grappled.Equals("Grappled") ||
                    Program.Character.Vitality.Conditions.Restrained.Equals("Restrained"))
                {
                    return 0;
                }
                else
                {
                    if (Program.Character.Vitality.Conditions.Encumbrance.Equals("Encumbered"))
                    {
                        newMovement -= 10;
                    }
                    else if (Program.Character.Vitality.Conditions.Encumbrance.Equals("Heavily Encumbered"))
                    {
                        newMovement -= 20;
                    }

                    if (Program.Character.Vitality.Conditions.Fatigued.Equals("Two") ||
                        Program.Character.Vitality.Conditions.Fatigued.Equals("Three") ||
                        Program.Character.Vitality.Conditions.Fatigued.Equals("Four"))
                    {
                        newMovement /= 2;
                    }

                    return newMovement;
                }
            }
        }
    }
}
