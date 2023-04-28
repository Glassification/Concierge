// <copyright file="Document.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Journal
{
    using Concierge.Common;

    public sealed class Document : Entry, ICopyable<Document>
    {
        public Document(string name)
            : base(name)
        {
            this.Rtf = string.Empty;
        }

        public string Rtf { get; set; }

        public Document DeepCopy()
        {
            return new Document(this.Name)
            {
                IsExpanded = this.IsExpanded,
                Rtf = this.Rtf,
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
