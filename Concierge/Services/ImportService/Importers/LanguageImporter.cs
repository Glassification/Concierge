// <copyright file="LanguageImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Details;
    using Concierge.Common;

    public sealed class LanguageImporter : Importer
    {
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
