// <copyright file="DequipItemCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Interfaces.Enums;

    public class DequipItemCommand : Command
    {
        private readonly EquippedItems equippedItems;
        private readonly EquipmentSlot slot;
        private readonly Guid equippedId;

        public DequipItemCommand(EquippedItems equippedItems, Inventory item, EquipmentSlot slot)
        {
            this.ConciergePage = ConciergePage.EquippedItems;
            this.equippedItems = equippedItems;
            this.Item = item;
            this.slot = slot;
            this.equippedId = item.EquppedId;
        }

        private Inventory Item { get; set; }

        public override void Redo()
        {
            this.equippedItems.Dequip(this.Item, this.slot);
        }

        public override void Undo()
        {
            this.Item = this.equippedItems.Equip(this.Item, this.slot);
            this.Item.EquppedId = this.equippedId;
        }
    }
}
