// <copyright file="History.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a history manager that stores and navigates through a list of strings.
    /// </summary>
    public sealed class History
    {
        private readonly List<string> history;

        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class with a default text.
        /// </summary>
        /// <param name="defaultText">The default text to use when the history is empty.</param>
        public History(string defaultText)
            : this(new List<string>(), defaultText)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class with an existing history list and default text.
        /// </summary>
        /// <param name="history">The list of strings representing the history.</param>
        /// <param name="defaultText">The default text to use when the history is empty.</param>
        public History(List<string> history, string defaultText)
        {
            this.history = history;
            this.Index = -1;
            this.DefaultText = defaultText;
        }

        /// <summary>
        /// Gets the current index in the history.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Gets the total count of items in the history.
        /// </summary>
        public int Count => this.history.Count;

        /// <summary>
        /// Gets the default text used when the history is empty.
        /// </summary>
        public string DefaultText { get; private set; }

        /// <summary>
        /// Moves forward in the history and returns the corresponding item.
        /// </summary>
        /// <returns>The item at the new index after moving forward in the history.</returns>
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

        /// <summary>
        /// Moves backward in the history and returns the corresponding item.
        /// </summary>
        /// <returns>The item at the new index after moving backward in the history.</returns>
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

        /// <summary>
        /// Adds a new item to the history.
        /// </summary>
        /// <param name="item">The item to be added to the history.</param>
        public void Add(string item)
        {
            this.history.Insert(0, item);
            this.Index = -1;
        }
    }
}
