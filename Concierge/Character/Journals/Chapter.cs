// <copyright file="Chapter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Journals
{
    using System.Collections.Generic;

    using Concierge.Common;
    using Concierge.Common.Extensions;

    public sealed class Chapter : Entry, ICopyable<Chapter>
    {
        public Chapter(string name)
            : base(name)
        {
            this.Documents = [];
        }

        public List<Document> Documents { get; set; }

        public Chapter DeepCopy()
        {
            return new Chapter(this.Name)
            {
                Documents = [.. this.Documents.DeepCopy()],
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
