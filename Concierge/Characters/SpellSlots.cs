using Concierge.Characters.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class SpellSlots
    {
        public SpellSlots()
        {
            PactTotal = 0;
            FirstTotal = 0;
            SecondTotal = 0;
            ThirdTotal = 0;
            FourthTotal = 0;
            FifthTotal = 0;
            SixthTotal = 0;
            SeventhTotal = 0;
            EighthTotal = 0;
            NinethTotal = 0;

            Reset();
        }

        public void Reset()
        {
            PactUsed = 0;
            FirstUsed = 0;
            SecondUsed = 0;
            ThirdUsed = 0;
            FourthUsed = 0;
            FifthUsed = 0;
            SixthUsed = 0;
            SeventhUsed = 0;
            EighthUsed = 0;
            NinethUsed = 0;
        }

        public int PactTotal { get; set; }
        public int FirstTotal { get; set; }
        public int SecondTotal { get; set; }
        public int ThirdTotal { get; set; }
        public int FourthTotal { get; set; }
        public int FifthTotal { get; set; }
        public int SixthTotal { get; set; }
        public int SeventhTotal { get; set; }
        public int EighthTotal { get; set; }
        public int NinethTotal { get; set; }

        public int PactUsed { get; set; }
        public int FirstUsed { get; set; }
        public int SecondUsed { get; set; }
        public int ThirdUsed { get; set; }
        public int FourthUsed { get; set; }
        public int FifthUsed { get; set; }
        public int SixthUsed { get; set; }
        public int SeventhUsed { get; set; }
        public int EighthUsed { get; set; }
        public int NinethUsed { get; set; }

        public int Level
        {
            get
            {
                int totalLevel = 0;

                foreach (MagicClass magicClass in Program.Character.MagicClasses)
                {
                    totalLevel += magicClass.Level;
                }

                return totalLevel;
            }
        }
    }
}
