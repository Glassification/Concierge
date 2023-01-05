// <copyright file="Language.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System;

    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public sealed class Language : ICopyable<Language>, IUnique
    {
        public Language()
        {
            this.Name = string.Empty;
            this.Script = string.Empty;
            this.Speakers = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public string Script { get; set; }

        public string Speakers { get; set; }

        public Guid Id { get; set; }

        [JsonIgnore]
        public string Description => $"{this.Name}{(IsValid(this.Script) ? $" ({this.Script})" : string.Empty)}, Spoken by: {this.Speakers}";

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

        private static bool IsValid(string value)
        {
            return !value.IsNullOrWhiteSpace() && !value.Equals("-") && !value.Equals("--");
        }
    }
}
