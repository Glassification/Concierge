// <copyright file="Chapter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    public class Chapter
    {
        public Chapter(string name)
        {
            this.Documents = new List<Document>();
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.IsNewChapterPlaceholder = false;
        }

        public List<Document> Documents { get; set; }

        public string Name { get; set; }

        public bool IsExpanded { get; set; }

        [JsonIgnore]
        public bool IsNewChapterPlaceholder { get; set; }

        public Guid Id { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }

        public Document GetDocumentById(Guid id)
        {
            return this.Documents.Single(x => x.Id.Equals(id));
        }
    }
}
