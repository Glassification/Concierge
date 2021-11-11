// <copyright file="Inventory.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Items
{
    using System;

    using Concierge.Primatives;
    using Concierge.Utility;
    using Newtonsoft.Json;

    public class Inventory : ICopyable<Inventory>
    {
        public Inventory()
        {
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public int Amount { get; set; }

        public ConvertableDouble Weight { get; set; }

        public bool IsInBagOfHolding { get; set; }

        [JsonIgnore]
        public string InBagOfHoldingText => this.IsInBagOfHolding ? "Yes" : "No";

        public string Note { get; set; }

        public bool Attuned { get; set; }

        [JsonIgnore]
        public string AttunedText => this.Attuned ? "Yes" : "No";

        public Guid EquppedId { get; set; }

        public Guid Id { get; init; }

        public override string ToString()
        {
            return this.Name;
        }

        public Inventory DeepCopy()
        {
            return new Inventory()
            {
                Name = this.Name,
                Amount = 1,
                Weight = this.Weight.DeepCopy(),
                IsInBagOfHolding = false,
                Attuned = this.Attuned,
                Note = this.Note,
                EquppedId = Guid.NewGuid(),
                Id = this.Id,
            };
        }
    }
}
