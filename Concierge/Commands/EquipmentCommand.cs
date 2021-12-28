// <copyright file="EquipmentCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System.Collections.Generic;

    using Concierge.Character.Items;
    using Concierge.Interfaces.Enums;

    public class EquipmentCommand : Command
    {
        private readonly EquippedItems oldEquippedItems;
        private readonly EquippedItems newEquippedItems;
        private readonly List<Inventory> oldList;
        private readonly List<Inventory> newList;

        public EquipmentCommand(EquippedItems oldEquippedItems, List<Inventory> oldList, EquippedItems newEquippedItems, List<Inventory> newList)
        {
            this.ConciergePage = ConciergePage.EquippedItems;
            this.oldEquippedItems = oldEquippedItems;
            this.newEquippedItems = newEquippedItems;
            this.oldList = oldList;
            this.newList = newList;
        }

        public override void Redo()
        {
            Program.CcsFile.Character.EquippedItems = this.newEquippedItems;
            Program.CcsFile.Character.Inventories = this.newList;
        }

        public override void Undo()
        {
            Program.CcsFile.Character.EquippedItems = this.oldEquippedItems;
            Program.CcsFile.Character.Inventories = this.oldList;
        }
    }
}
