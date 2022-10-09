// <copyright file="EquipItemCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System;
    using System.Linq;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Interfaces.Enums;

    public sealed class EquipItemCommand : Command
    {
        private readonly EquipmentSlot equipmentSlot;
        private readonly Guid equippedId;
        private readonly Guid id;
        private readonly int index;

        public EquipItemCommand(Inventory item, EquipmentSlot equipmentSlot)
        {
            this.ConciergePage = ConciergePage.EquippedItems;
            this.equipmentSlot = equipmentSlot;
            this.index = item.Index;
            this.equippedId = item.EquppedId;
            this.id = item.Id;
        }

        public override void Redo()
        {
            var item = Program.CcsFile.Character.Inventories.Where(x => x.Id.Equals(this.id)).First();
            item = Program.CcsFile.Character.EquippedItems.Equip(item, this.equipmentSlot);
            item.Index = this.index;
            item.EquppedId = this.equippedId;
        }

        public override void Undo()
        {
            Program.CcsFile.Character.EquippedItems.Dequip(this.id, this.equippedId, this.equipmentSlot);
        }
    }
}
