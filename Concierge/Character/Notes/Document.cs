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
            this.Name = name;
            this.Id = Guid.NewGuid();
            this.Rtf = string.Empty;
        }

        public bool IsExpanded { get; set; }

        public string Name { get; set; }

        public string Rtf { get; set; }

        public Guid Id { get; init; }

        public Document DeepCopy()
        {
            return new Document(this.Name)
            {
                IsExpanded = this.IsExpanded,
                Rtf = this.Rtf,
                Id = this.Id,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
