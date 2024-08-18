// <copyright file="AbilityImporter.cs" company="Thomas Beckett">
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
    /// Represents an importer for ability data.
    /// </summary>
    public sealed class AbilityImporter : Importer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityImporter"/> class with the specified character.
        /// </summary>
        /// <param name="character">The character sheet to import ability data into.</param>
        public AbilityImporter(CharacterSheet character)
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
            this.Character.Detail.Abilities.AddRange(abilities);
        }

        public override IEnumerable<IUnique> Load(CharacterSheet character)
        {
            return character.Detail.Abilities;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Ability>>(fileName);
        }
    }
}
