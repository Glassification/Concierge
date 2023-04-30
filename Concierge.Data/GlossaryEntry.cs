// <copyright file="GlossaryEntry.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data
{
    using System;
    using System.Collections.Generic;

    using Concierge.Common.Extensions;

    public sealed class GlossaryEntry
    {
        public GlossaryEntry()
            : this(string.Empty, string.Empty)
        {
        }

        public GlossaryEntry(string name, string markdown)
        {
            this.Name = name;
            this.Markdown = markdown;
            this.GlossaryEntries = new List<GlossaryEntry>();
        }

        public List<GlossaryEntry> GlossaryEntries { get; set; }

        public bool IsExpanded { get; set; }

        public string Markdown { get; set; }

        public string Name { get; set; }

        public bool Search(string text)
        {
            if (text.IsNullOrWhiteSpace())
            {
                return true;
            }

            var containsText =
                this.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                this.Markdown.Contains(text, StringComparison.InvariantCultureIgnoreCase);

            this.GlossaryEntries.ForEach(x => containsText |= x.Search(text));

            return containsText;
        }
    }
}
