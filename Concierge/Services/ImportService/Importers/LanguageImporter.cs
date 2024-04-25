// <copyright file="LanguageImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Details;
    using Concierge.Common;

    /// <summary>
    /// Represents an importer for language data.
    /// </summary>
    public sealed class LanguageImporter : Importer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageImporter"/> class with the specified character.
        /// </summary>
        /// <param name="character">The character sheet to import language data into.</param>
        public LanguageImporter(CharacterSheet character)
            : base(character)
        {
        }

        public override void Import(IEnumerable<IUnique> list)
        {
            if (list is not List<Language> languages)
            {
                return;
            }

            Program.Logger.Info($"Import language.");
            CycleGuids(languages);
            this.Character.Detail.Languages.AddRange(languages);
        }

        public override IEnumerable<IUnique> Load(CharacterSheet character)
        {
            return character.Detail.Languages;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Language>>(fileName);
        }
    }
}
