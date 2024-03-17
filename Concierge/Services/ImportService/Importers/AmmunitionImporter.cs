// <copyright file="AmmunitionImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Equipable;
    using Concierge.Common;

    public sealed class AmmunitionImporter : Importer
    {
        public AmmunitionImporter(CharacterSheet character)
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
