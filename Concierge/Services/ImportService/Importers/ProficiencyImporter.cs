// <copyright file="ProficiencyImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Characteristics;
    using Concierge.Common;
    using Concierge.Persistence.ReadWriters;

    public class ProficiencyImporter : Importer
    {
        public ProficiencyImporter(ConciergeCharacter character)
            : base(character)
        {
        }

        public override void Import(IEnumerable<IUnique> list)
        {
            if (list is not List<Proficiency> proficiencies)
            {
                return;
            }

            Program.Logger.Info($"Import proficiency.");
            CycleGuids(proficiencies);
            this.Character.Characteristic.Proficiencies.AddRange(proficiencies);
        }

        public override IEnumerable<IUnique> Load(ConciergeCharacter character)
        {
            return character.Characteristic.Proficiencies;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Proficiency>>(fileName);
        }
    }
}
