// <copyright file="Language.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Characteristics
{
    using System;

    using Newtonsoft.Json;

    public class Language
    {
        public Language()
        {
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public string Script { get; set; }

        public string Speakers { get; set; }

        public Guid Id { get; }

        [JsonIgnore]
        public string Description => $"{this.Name} ({this.Script}), Spoken by: {this.Speakers}";

        public override string ToString()
        {
            return this.Name;
        }
    }
}
