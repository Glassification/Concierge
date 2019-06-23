using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class HitDice
    {
        public HitDice()
        {
            TotalD6 = 0;
            TotalD8 = 0;
            TotalD10 = 0;
            TotalD12 = 0;
            SpentD6 = 0;
            SpentD8 = 0;
            SpentD10 = 0;
            SpentD12 = 0;
        }

        public int TotalD6 { get; set; }

        public int TotalD8 { get; set; }

        public int TotalD10 { get; set; }

        public int TotalD12 { get; set; }

        public int SpentD6 { get; set; }

        public int SpentD8 { get; set; }

        public int SpentD10 { get; set; }

        public int SpentD12 { get; set; }
    }
}
