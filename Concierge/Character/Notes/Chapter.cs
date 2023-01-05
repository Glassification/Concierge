// <copyright file="Chapter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Notes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public sealed class Chapter : ICopyable<Chapter>, IUnique
    {
        public Chapter(string name)
        {
            this.Documents = new List<Document>();
            this.Id = Guid.NewGuid();
            this.Name = name;
        }

        public List<Document> Documents { get; set; }

        public string Name { get; set; }

        public bool IsExpanded { get; set; }

        public Guid Id { get; set; }

        public Chapter DeepCopy()
        {
            return new Chapter(this.Name)
            {
                Documents = this.Documents.DeepCopy().ToList(),
                IsExpanded = this.IsExpanded,
                Id = this.Id,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
