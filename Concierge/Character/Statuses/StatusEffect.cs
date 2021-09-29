// <copyright file="StatusEffect.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses
{
    using System;

    using Concierge.Character.Enums;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public class StatusEffect
    {
        public StatusEffect()
        {
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public StatusEffectTypes Type { get; set; }

        public string Description { get; set; }

        public Guid Id { get; init; }

        [JsonIgnore]
        public string Display => $"{this.Name}{(this.Type == StatusEffectTypes.None ? string.Empty : $" {this.Type}")}{(this.Description.IsNullOrWhiteSpace() ? string.Empty : " - ")}{this.Description}";

        public override string ToString()
        {
            return $"{this.Name} {this.Type}";
        }
    }
}
