// <copyright file="Document.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Notes
{
    using System;

    using Concierge.Utility;

    public class Document : ICopyable<Document>
    {
        public Document(string name)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
        }

        public bool IsExpanded { get; set; }

        public string Name { get; set; }

        public string RTF { get; set; }

        public Guid Id { get; init; }

        public Document DeepCopy()
        {
            return new Document(this.Name)
            {
                IsExpanded = this.IsExpanded,
                RTF = this.RTF,
                Id = this.Id,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
