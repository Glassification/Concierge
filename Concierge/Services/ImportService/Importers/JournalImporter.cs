// <copyright file="JournalImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Journals;
    using Concierge.Common;

    public class JournalImporter : Importer
    {
        public JournalImporter(ConciergeCharacter character)
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

        public override IEnumerable<IUnique> Load(ConciergeCharacter character)
        {
            return character.Journal.Chapters;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Chapter>>(fileName);
        }
    }
}
