// <copyright file="DequipItemCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System;
    using System.Linq;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Interfaces.Enums;

    public class DequipItemCommand : Command
    {
        private readonly EquipmentSlot equipmentSlot;
        private readonly Guid equippedId;
        private readonly Guid id;
        private readonly int index;

        public DequipItemCommand(Inventory item, int index, Guid equippedId, EquipmentSlot equipmentSlot)
        {
            this.ConciergePage = ConciergePage.EquippedItems;
            this.equipmentSlot = equipmentSlot;
            this.index = index;
            this.equippedId = equippedId;
            this.id = item.Id;
        }

        public override void Redo()
        {
            Program.CcsFile.Character.EquippedItems.Dequip(this.id, this.equippedId, this.equipmentSlot);
        }

        public override void Undo()
        {
            var item = Program.CcsFile.Character.Inventories.Where(x => x.Id.Equals(this.id)).First();
            item = Program.CcsFile.Character.EquippedItems.Equip(item, this.equipmentSlot);
            item.Index = this.index;
            item.EquppedId = this.equippedId;
        }
    }
}
