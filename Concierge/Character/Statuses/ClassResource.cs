// <copyright file="ClassResource.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses
{
    using System;

    using Concierge.Utility;
    using Newtonsoft.Json;

    public class ClassResource : ICopyable<ClassResource>
    {
        public ClassResource()
        {
            this.Id = Guid.NewGuid();
        }

        public string Type { get; set; }

        public int Total { get; set; }

        public int Spent { get; set; }

        [JsonIgnore]
        public string Description => $"{this.Type} - {this.Spent}/{this.Total} Used";

        public Guid Id { get; init; }

        public ClassResource DeepCopy()
        {
            return new ClassResource()
            {
                Type = this.Type,
                Total = this.Total,
                Spent = this.Spent,
                Id = this.Id,
            };
        }
    }
}
