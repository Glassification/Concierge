// <copyright file="Inventory.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Items
{
    using System;

    using Concierge.Primitives;
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

        public ConciergeDouble Weight { get; set; }

        public bool IsInBagOfHolding { get; set; }

        [JsonIgnore]
        public string InBagOfHoldingText => this.IsInBagOfHolding ? "Yes" : "No";

        public string Note { get; set; }

        public bool Attuned { get; set; }

        [JsonIgnore]
        public string AttunedText => this.Attuned ? "Yes" : "No";

        public int Index { get; set; }

        public Guid EquppedId { get; set; }

        public Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not Inventory)
            {
                return false;
            }

            var item = obj as Inventory;
            return item.Name.Equals(this.Name) &&
                item.Weight == this.Weight &&
                item.Note.Equals(this.Note);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public Inventory DeepCopy()
        {
            return new Inventory()
            {
                Name = this.Name,
                Amount = this.Amount,
                Weight = this.Weight.DeepCopy(),
                IsInBagOfHolding = this.IsInBagOfHolding,
                Attuned = this.Attuned,
                Note = this.Note,
                Index = this.Index,
                EquppedId = this.EquppedId,
                Id = this.Id,
            };
        }
    }
}
