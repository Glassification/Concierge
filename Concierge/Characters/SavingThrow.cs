using Concierge.SavingThrowsNamespace;
using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class SavingThrow
    {

        public SavingThrow()
        {
            Strength = new Strength();
            Dexterity = new Dexterity();
            Constitution = new Constitution();
            Intelligence = new Intelligence();
            Wisdom = new Wisdom();
            Charisma = new Charisma();

            SetSavingThrowCheck(Constants.Checks.Normal);
        }

        public void SetSavingThrowCheck(Constants.Checks checks)
        {
            Strength.Checks = checks;
            Dexterity.Checks = checks;
            Constitution.Checks = checks;
            Intelligence.Checks = checks;
            Wisdom.Checks = checks;
            Charisma.Checks = checks;
        }

        public Strength Strength { get; set; }
        public Dexterity Dexterity { get; set; }
        public Constitution Constitution { get; set; }
        public Intelligence Intelligence { get; set; }
        public Wisdom Wisdom { get; set; }
        public Charisma Charisma { get; set; }
    }
}
