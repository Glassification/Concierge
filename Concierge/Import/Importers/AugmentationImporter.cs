// <copyright file="AugmentationImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Import.Importers
{
    using System.Collections.Generic;

    using Concierge;
    using Concierge.Character;
    using Concierge.Character.Equipable;
    using Concierge.Common;

    /// <summary>
    /// Represents an importer for ammunition data.
    /// </summary>
    public sealed class AugmentationImporter : Importer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AugmentationImporter"/> class with the specified character.
        /// </summary>
        /// <param name="character">The character sheet to import ammunition data into.</param>
        public AugmentationImporter(CharacterSheet character)
            : base(character)
        {
        }

        public override void Import(IEnumerable<IUnique> list)
        {
            if (list is not List<Augment> ammo)
            {
                return;
            }

            Program.Logger.Info($"Import ammunition.");
            CycleGuids(ammo);
            this.Character.Equipment.Augmentation.AddRange(ammo);
        }

        public override IEnumerable<IUnique> Load(CharacterSheet character)
        {
            return character.Equipment.Augmentation;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Augment>>(fileName);
        }
    }
}
