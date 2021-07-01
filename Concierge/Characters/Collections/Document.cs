// <copyright file="Document.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System;

    public class Document
    {
        public Document()
        {
            this.ID = Guid.NewGuid();
        }

        public Document(Guid id)
        {
            this.ID = id;
        }

        public string Name { get; set; }

        public string RTF { get; set; }

        public Guid ID { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
