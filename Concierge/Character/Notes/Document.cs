// <copyright file="Document.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Notes
{
    using System;

    public class Document
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

        public override string ToString()
        {
            return this.Name;
        }
    }
}
