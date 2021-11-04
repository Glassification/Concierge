// <copyright file="DequipItemCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Interfaces.Enums;

    public class DequipItemCommand : Command
    {
        private readonly EquippedItems equippedItems;
        private readonly EquipmentSlot slot;

        public DequipItemCommand(EquippedItems equippedItems, Inventory item, EquipmentSlot slot, ConciergePage conciergePage)
        {
            this.ConciergePage = conciergePage;
            this.equippedItems = equippedItems;
            this.Item = item;
            this.slot = slot;
        }

        private Inventory Item { get; set; }

        public override void Redo()
        {
            this.equippedItems.Dequip(this.Item, this.slot);
        }

        public override void Undo()
        {
            this.Item = this.equippedItems.Equip(this.Item, this.slot);
        }
    }
}
