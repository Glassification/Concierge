// <copyright file="Inventory.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System;

    using Newtonsoft.Json;

    public class Inventory
    {
        public Inventory()
        {
            this.ID = Guid.NewGuid();
        }

        public Inventory(Guid id)
        {
            this.ID = id;
        }

        public string Name { get; set; }

        public int Amount { get; set; }

        public double Weight { get; set; }

        public bool IsInBagOfHolding { get; set; }

        [JsonIgnore]
        public string InBagOfHoldingText => this.IsInBagOfHolding ? "Yes" : "No";

        public string Note { get; set; }

        public bool Attuned { get; set; }

        [JsonIgnore]
        public string AttunedText => this.Attuned ? "Yes" : "No";

        public Guid EquppedId { get; set; }

        public Guid ID { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }

        public Inventory Copy()
        {
            return new Inventory()
            {
                Name = this.Name,
                Amount = 1,
                Weight = this.Weight,
                IsInBagOfHolding = false,
                Note = this.Note,
                EquppedId = Guid.NewGuid(),
                ID = this.ID,
            };
        }
    }
}
