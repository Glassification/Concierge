namespace Concierge.Characters
{
    using System;

    public class HitDice
    {
        private int spentD6, spentD8, spentD10, spentD12;

        public HitDice()
        {
            TotalD6 = 0;
            TotalD8 = 0;
            TotalD10 = 0;
            TotalD12 = 0;
            spentD6 = 0;
            spentD8 = 0;
            spentD10 = 0;
            spentD12 = 0;
        }

        public int TotalD6 { get; set; }

        public int TotalD8 { get; set; }

        public int TotalD10 { get; set; }

        public int TotalD12 { get; set; }

        public int SpentD6
        {
            get
            {
                return spentD6;
            }
            set
            {
                if (value <= TotalD6)
                {
                    spentD6 = value;
                }
            }
        }

        public int SpentD8
        {
            get
            {
                return spentD8;
            }
            set
            {
                if (value <= TotalD8)
                {
                    spentD8 = value;
                }
            }
        }

        public int SpentD10
        {
            get
            {
                return spentD10;
            }
            set
            {
                if (value <= TotalD10)
                {
                    spentD10 = value;
                }
            }
        }

        public int SpentD12
        {
            get
            {
                return spentD12;
            }
            set
            {
                if (value <= TotalD12)
                {
                    spentD12 = value;
                }
            }
        }
    }
}
