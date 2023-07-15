// <copyright file="AmmunitionImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Equipable;
    using Concierge.Common;

    public class AmmunitionImporter : Importer
    {
        public AmmunitionImporter(ConciergeCharacter character)
            : base(character)
        {
        }

        public override void Import(IEnumerable<IUnique> list)
        {
            if (list is not List<Ammunition> ammo)
            {
                return;
            }

            Program.Logger.Info($"Import ammunition.");
            CycleGuids(ammo);
            this.Character.Equipment.Ammunition.AddRange(ammo);
        }

        public override IEnumerable<IUnique> Load(ConciergeCharacter character)
        {
            return character.Equipment.Ammunition;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Ammunition>>(fileName);
        }
    }
}
