// <copyright file="Document.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System;

    public class Document
    {
        public Document(string name)
        {
            this.ID = Guid.NewGuid();
            this.Name = name;
        }

        public bool IsExpanded { get; set; }

        public string Name { get; set; }

        public string RTF { get; set; }

        public Guid ID { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
