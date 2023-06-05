// <copyright file="AbilityImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Characteristics;
    using Concierge.Common;

    public class AbilityImporter : Importer
    {
        public AbilityImporter(ConciergeCharacter character)
            : base(character)
        {
        }

        public override void Import(IEnumerable<IUnique> list)
        {
            if (list is not List<Ability> abilities)
            {
                return;
            }

            Program.Logger.Info($"Import abilities.");
            CycleGuids(abilities);
            this.Character.Characteristic.Abilities.AddRange(abilities);
        }

        public override IEnumerable<IUnique> Load(ConciergeCharacter character)
        {
            return character.Characteristic.Abilities;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Ability>>(fileName);
        }
    }
}
