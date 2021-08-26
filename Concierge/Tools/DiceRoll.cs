// <copyright file="DiceRoll.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using System.Collections.Generic;

    public class DiceRoll
    {
        public DiceRoll(string roll, int[] list, int total)
        {
            this.Roll = roll;
            this.DiceList = new List<int>(list);
            this.Total = total;
        }

        public string Roll { get; init; }

        public string Dice
        {
            get
            {
                var str = string.Empty;

                foreach (int die in this.DiceList)
                {
                    str += die + ", ";
                }

                return str.Trim(new char[] { ',', ' ' });
            }
        }

        public int Total { get; init; }

        private List<int> DiceList { get; init; }
    }
}
