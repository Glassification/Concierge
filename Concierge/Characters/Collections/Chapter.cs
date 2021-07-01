// <copyright file="Chapter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Chapter
    {
        public Chapter()
        {
            this.Documents = new List<Document>();
            this.ID = Guid.NewGuid();
        }

        public Chapter(Guid id)
        {
            this.Documents = new List<Document>();
            this.ID = id;
        }

        public List<Document> Documents { get; set; }

        public string Name { get; set; }

        public Guid ID { get; private set; }

        public Document GetDocumentById(Guid id)
        {
            return this.Documents.Single(x => x.ID.Equals(id));
        }
    }
}
