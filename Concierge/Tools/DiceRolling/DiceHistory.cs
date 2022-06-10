// <copyright file="DiceHistory.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.DiceRolling
{
    using System;
    using System.Collections.Generic;

    public class DiceHistory
    {
        private readonly List<string> history;

        public DiceHistory()
        {
            this.history = new List<string>();
            this.Index = -1;
        }

        public int Index { get; private set; }

        public int Count => this.history.Count;

        public string Forward()
        {
            if (this.Count == 0)
            {
                return string.Empty;
            }

            this.Index--;
            this.Index = Math.Max(this.Index, -1);
            if (this.Index == -1)
            {
                return string.Empty;
            }

            return this.history[this.Index];
        }

        public string Backward()
        {
            if (this.Count == 0)
            {
                return string.Empty;
            }

            this.Index++;
            this.Index = Math.Min(this.Index, this.Count - 1);

            return this.history[this.Index];
        }

        public void Add(string item)
        {
            this.history.Insert(0, item);
            this.Index = -1;
        }
    }
}
