using System;
using System.Collections.Generic;

namespace Concierge.Utility
{
    public class Die
    {
        public Die(string roll, int[] list, int total)
        {
            Roll = roll;
            DiceList = new List<int>(list);
            Total = total;
        }

        public string Roll { get; set; }
        public List<int> DiceList { get; set; }
        public string Dice
        {
            get
            {
                string str = "";

                foreach (int die in DiceList)
                {
                    str += die + ", ";
                }

                if (str.Length >= 2)
                {
                    str = str.Remove(str.Length - 2, 2);
                }

                return str;
            }
        }
        public int Total { get; set; }
    }
}
