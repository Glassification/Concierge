// <copyright file="InventoryImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Equipable;
    using Concierge.Common;

    /// <summary>
    /// Represents an importer for inventory data.
    /// </summary>
    public sealed class InventoryImporter : Importer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryImporter"/> class with the specified character.
        /// </summary>
        /// <param name="character">The character sheet to import inventory data into.</param>
        public InventoryImporter(CharacterSheet character)
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

        public override IEnumerable<IUnique> Load(CharacterSheet character)
        {
            return character.Equipment.Inventory;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Inventory>>(fileName);
        }
    }
}
