// <copyright file="WeaponImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Equipable;
    using Concierge.Common;

    /// <summary>
    /// Represents an importer for weapon data.
    /// </summary>
    public sealed class WeaponImporter : Importer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeaponImporter"/> class with the specified character.
        /// </summary>
        /// <param name="character">The character sheet to import weapon data into.</param>
        public WeaponImporter(CharacterSheet character)
            : base(character)
        {
        }

        public override void Import(IEnumerable<IUnique> list)
        {
            if (list is not List<Weapon> weapons)
            {
                return;
            }

            Program.Logger.Info($"Import weapons.");
            CycleGuids(weapons);
            this.Character.Equipment.Weapons.AddRange(weapons);
        }

        public override IEnumerable<IUnique> Load(CharacterSheet character)
        {
            return character.Equipment.Weapons;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Weapon>>(fileName);
        }
    }
}
