// <copyright file="Chapter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Journal
{
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public sealed class Chapter : Entry, ICopyable<Chapter>
    {
        public Chapter(string name)
            : base(name)
        {
            this.Documents = new List<Document>();
        }

        public List<Document> Documents { get; set; }

        public Chapter DeepCopy()
        {
            return new Chapter(this.Name)
            {
                Documents = this.Documents.DeepCopy().ToList(),
                IsExpanded = this.IsExpanded,
                Id = this.Id,
                Created = this.Created,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
