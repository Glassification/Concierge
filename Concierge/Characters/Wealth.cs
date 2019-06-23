using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class Wealth
    {
        public Wealth()
        {
            Copper = 0;
            Silver = 0;
            Electrum = 0;
            Gold = 0;
            Platinum = 0;
        }

        public int Copper { get; set; }

        public int Silver { get; set; }

        public int Electrum { get; set; }

        public int Gold { get; set; }

        public int Platinum { get; set; }

        public double TotalValue
        {
            get
            {
                return (Copper / 100.0) + (Silver / 10.0) + (Electrum / 2.0) + Gold + (Platinum * 10.0);
            }
        }

        public int TotalCoins
        {
            get
            {
                return Copper + Silver + Electrum + Gold + Platinum;
            }
        }
    }
}
