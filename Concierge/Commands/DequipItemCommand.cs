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
        private readonly Inventory item;
        private readonly EquipmentSlot equipmentSlot;
        private readonly Guid equippedId;
        private readonly int index;

        public DequipItemCommand(Inventory item, EquipmentSlot equipmentSlot)
        {
            this.ConciergePage = ConciergePage.EquippedItems;
            this.item = item;
            this.equipmentSlot = equipmentSlot;
            this.index = item.Index;
            this.equippedId = item.EquppedId;
        }

        public override void Redo()
        {
            Program.CcsFile.Character.EquippedItems.Dequip(this.item, this.equipmentSlot);
        }

        public override void Undo()
        {
            Program.CcsFile.Character.EquippedItems.Equip(this.item, this.equipmentSlot);
            this.item.Index = this.index;
            this.item.EquppedId = this.equippedId;
        }
    }
}
