// <copyright file="Language.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System;

    using Newtonsoft.Json;

    public class Language
    {
        public Language()
        {
            this.ID = Guid.NewGuid();
        }

        public Language(Guid id)
        {
            this.ID = id;
        }

        public string Name { get; set; }

        public string Script { get; set; }

        public string Speakers { get; set; }

        public Guid ID { get; }

        [JsonIgnore]
        public string Description => $"{this.Name} ({this.Script}), Spoken by: {this.Speakers}";

        public override string ToString()
        {
            return this.Name;
        }
    }
}
