// <copyright file="Chapter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Notes
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Chapter : ICopyable
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

        public Guid Id { get; init; }

        public ICopyable DeepCopy()
        {
            return new Chapter(this.Name)
            {
                Documents = new List<Document>(this.Documents),
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
