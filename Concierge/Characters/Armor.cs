using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class Armor
    {
        public Armor()
        {
            Equiped = "";
            Type = Constants.ArmorType.None;
            ArmorClass = 0;
            Strength = 0;
            Weight = 0.0;
            Stealth = Constants.ArmorStealth.Normal;
            Shield = "";
            ShieldArmorClass = 0;
            ShieldWeight = 0.0;
            MiscArmorClass = 0;
            MagicArmorClass = 0;
        }

        public string Equiped { get; set; }

        public Constants.ArmorType Type { get; set; }

        public int ArmorClass { get; set; }

        public int Strength { get; set; }

        public double Weight { get; set; }

        public Constants.ArmorStealth Stealth { get; set; }

        public string Shield { get; set; }

        public int ShieldArmorClass { get; set; }

        public double ShieldWeight { get; set; }

        public int MiscArmorClass { get; set; }

        public int MagicArmorClass { get; set; }
    }
}
