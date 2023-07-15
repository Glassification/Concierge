// <copyright file="InventoryImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Equipable;
    using Concierge.Common;

    public class InventoryImporter : Importer
    {
        public InventoryImporter(ConciergeCharacter character)
            : base(character)
        {
        }

        public override void Import(IEnumerable<IUnique> list)
        {
            if (list is not List<Inventory> items)
            {
                return;
            }

            Program.Logger.Info($"Import inventory.");
            CycleGuids(items);
            this.Character.Equipment.Inventory.AddRange(items);
        }

        public override IEnumerable<IUnique> Load(ConciergeCharacter character)
        {
            return character.Equipment.Inventory;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Inventory>>(fileName);
        }
    }
}
