// <copyright file="ProficiencyImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Details;
    using Concierge.Common;

    public sealed class ProficiencyImporter : Importer
    {
        public ProficiencyImporter(CharacterSheet character)
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
            this.Character.Detail.Proficiencies.AddRange(proficiencies);
        }

        public override IEnumerable<IUnique> Load(CharacterSheet character)
        {
            return character.Detail.Proficiencies;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Proficiency>>(fileName);
        }
    }
}
