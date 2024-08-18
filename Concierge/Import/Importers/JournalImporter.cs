// <copyright file="JournalImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Import.Importers
{
    using System.Collections.Generic;

    using Concierge;
    using Concierge.Character;
    using Concierge.Character.Journals;
    using Concierge.Common;

    /// <summary>
    /// Represents an importer for journal data.
    /// </summary>
    public sealed class JournalImporter : Importer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JournalImporter"/> class with the specified character.
        /// </summary>
        /// <param name="character">The character sheet to import journal data into.</param>
        public JournalImporter(CharacterSheet character)
            : base(character)
        {
        }

        public override void Import(IEnumerable<IUnique> list)
        {
            if (list is not List<Chapter> chapters)
            {
                return;
            }

            Program.Logger.Info($"Import journal.");
            CycleGuids(chapters);
            this.Character.Journal.Chapters.AddRange(chapters);
        }

        public override IEnumerable<IUnique> Load(CharacterSheet character)
        {
            return character.Journal.Chapters;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Chapter>>(fileName);
        }
    }
}
