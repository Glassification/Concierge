// <copyright file="History.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using System;
    using System.Collections.Generic;

    public sealed class History
    {
        private readonly List<string> history;

        public History(string defaultText)
            : this(new List<string>(), defaultText)
        {
        }

        public History(List<string> history, string defaultText)
        {
            this.history = history;
            this.Index = -1;
            this.DefaultText = defaultText;
        }

        public int Index { get; private set; }

        public int Count => this.history.Count;

        public string DefaultText { get; private set; }

        public string Forward()
        {
            if (this.Count == 0)
            {
                return this.DefaultText;
            }

            this.Index--;
            this.Index = Math.Max(this.Index, -1);
            if (this.Index == -1)
            {
                return this.DefaultText;
            }

            return this.history[this.Index];
        }

        public string Backward()
        {
            if (this.Count == 0)
            {
                return this.DefaultText;
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
