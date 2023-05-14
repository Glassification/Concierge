// <copyright file="StatusEffect.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using System;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Dtos;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    public sealed class StatusEffect : ICopyable<StatusEffect>, IUnique
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

        public Guid Id { get; set; }

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

        public CategoryDto GetCategory()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{this.Name} {this.Type}";
        }
    }
}
