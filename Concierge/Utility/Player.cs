using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Utility
{
    public class Player
    {
        #region Constants

        public const int CURRENCIES = 5;
        public enum Currency { Platinum, Gold, Electrum, Silver, Copper };

        #endregion

        #region Members

        public int[] currency = new int[CURRENCIES];

        #endregion

        #region Constructors

        /// =========================================
        /// Player()
        /// =========================================
        public Player(string name)
        {
            Name = name;
        }

        /// =========================================
        /// Player()
        /// =========================================
        public Player(int cp, int sp, int ep, int gp, int pp)
        {
            currency[(int)Currency.Copper] = cp;
            currency[(int)Currency.Silver] = sp;
            currency[(int)Currency.Electrum] = ep;
            currency[(int)Currency.Gold] = gp;
            currency[(int)Currency.Platinum] = pp;
        }

        #endregion

        #region Accessors

        public string Name { get; set; }

        public double Total
        {
            get
            {
                return (Copper / 100.0) + (Silver / 10.0) + (Electrum / 5.0) + Gold + (Platinum * 10.0);
            }
        }

        public int Copper
        {
            get
            {
                return currency[(int)Currency.Copper];
            }
            set
            {
                currency[(int)Currency.Copper] = value;
            }
        }

        public int Silver
        {
            get
            {
                return currency[(int)Currency.Silver];
            }
            set
            {
                currency[(int)Currency.Silver] = value;
            }
        }

        public int Electrum
        {
            get
            {
                return currency[(int)Currency.Electrum];
            }
            set
            {
                currency[(int)Currency.Electrum] = value;
            }
        }

        public int Gold
        {
            get
            {
                return currency[(int)Currency.Gold];
            }
            set
            {
                currency[(int)Currency.Gold] = value;
            }
        }

        public int Platinum
        {
            get
            {
                return currency[(int)Currency.Platinum];
            }
            set
            {
                currency[(int)Currency.Platinum] = value;
            }
        }

        #endregion
    }
}
