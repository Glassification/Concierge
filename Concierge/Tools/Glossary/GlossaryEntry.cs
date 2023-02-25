// <copyright file="GlossaryEntry.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Glossary
{
    using System.Collections.Generic;

    public sealed class GlossaryEntry
    {
        public GlossaryEntry()
        {
            this.Name = string.Empty;
            this.Markdown = string.Empty;
            this.GlossaryEntries = new List<GlossaryEntry>();
        }

        public List<GlossaryEntry> GlossaryEntries { get; set; }

        public bool IsExpanded { get; set; }

        public string Markdown { get; set; }

        public string Name { get; set; }
    }
}
