// <copyright file="GlossaryEntry.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data
{
    using System.Collections.Generic;

    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents an entry in a glossary.
    /// </summary>
    public sealed class GlossaryEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlossaryEntry"/> class with empty values for name and markdown.
        /// </summary>
        public GlossaryEntry()
            : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlossaryEntry"/> class with the specified name and markdown.
        /// </summary>
        /// <param name="name">The name of the entry.</param>
        /// <param name="markdown">The markdown content of the entry.</param>
        public GlossaryEntry(string name, string markdown)
        {
            this.Name = name;
            this.Markdown = markdown;
            this.GlossaryEntries = [];
        }

        /// <summary>
        /// Gets or sets the list of nested glossary entries.
        /// </summary>
        public List<GlossaryEntry> GlossaryEntries { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the glossary entry is expanded.
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Gets or sets the markdown content of the entry.
        /// </summary>
        public string Markdown { get; set; }

        /// <summary>
        /// Gets or sets the name of the entry.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Searches the glossary entry and its nested entries for the specified text.
        /// </summary>
        /// <param name="text">The text to search for.</param>
        /// <returns><see langword="true"/> if the text is found in the glossary entry or its nested entries; otherwise, <see langword="false"/>.</returns>
        public bool Search(string text)
        {
            if (text.IsNullOrWhiteSpace())
            {
                return true;
            }

            var containsText =
                this.Name.ContainsIgnoreCase(text) ||
                this.Markdown.ContainsIgnoreCase(text);

            this.GlossaryEntries.ForEach(x => containsText |= x.Search(text));

            return containsText;
        }
    }
}
