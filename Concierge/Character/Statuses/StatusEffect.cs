// <copyright file="StatusEffect.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses
{
    using System;

    using Concierge.Character.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public class StatusEffect : ICopyable<StatusEffect>
    {
        public StatusEffect()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public StatusEffectTypes Type { get; set; }

        public string Description { get; set; }

        public Guid Id { get; init; }

        [JsonIgnore]
        public string Display => $"{this.Name}{(this.Type == StatusEffectTypes.None ? string.Empty : $" {this.Type}")}{(this.Description.IsNullOrWhiteSpace() ? string.Empty : " - ")}{this.Description}";

        public StatusEffect DeepCopy()
        {
            return new StatusEffect()
            {
                Name = this.Name,
                Type = this.Type,
                Description = this.Description,
                Id = this.Id,
            };
        }

        public override string ToString()
        {
            return $"{this.Name} {this.Type}";
        }
    }
}
