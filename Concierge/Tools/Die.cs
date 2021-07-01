// <copyright file="Die.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using System.Collections.Generic;

    public class Die
    {
        public Die(string roll, int[] list, int total)
        {
            this.Roll = roll;
            this.DiceList = new List<int>(list);
            this.Total = total;
        }

        public string Roll { get; set; }

        public List<int> DiceList { get; set; }

        public string Dice
        {
            get
            {
                var str = string.Empty;

                foreach (int die in this.DiceList)
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
