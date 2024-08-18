// <copyright file="ProficiencyImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Import.Importers
{
    using System.Collections.Generic;

    using Concierge;
    using Concierge.Character;
    using Concierge.Character.Details;
    using Concierge.Common;

    /// <summary>
    /// Represents an importer for proficiency data.
    /// </summary>
    public sealed class ProficiencyImporter : Importer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProficiencyImporter"/> class with the specified character.
        /// </summary>
        /// <param name="character">The character sheet to import proficiency data into.</param>
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
