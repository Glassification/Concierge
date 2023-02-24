// <copyright file="Entry.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Glossary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Concierge.Utility.Extensions;

    public class Entry
    {
        public Entry()
        {
            this.Name = string.Empty;
            this.RichText = string.Empty;
        }

        public string Name { get; set; }

        public string RichText { get; set; }

        public string Text => this.RichText.StripRichTextFormat();
    }
}
