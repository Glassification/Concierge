// <copyright file="Language.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System;

    using Concierge.Utility;
    using Newtonsoft.Json;

    public class Language : ICopyable<Language>
    {
        public Language()
        {
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public string Script { get; set; }

        public string Speakers { get; set; }

        public Guid Id { get; init; }

        [JsonIgnore]
        public string Description => $"{this.Name} ({this.Script}), Spoken by: {this.Speakers}";

        public Language DeepCopy()
        {
            return new Language()
            {
                Name = this.Name,
                Script = this.Script,
                Speakers = this.Speakers,
                Id = this.Id,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
