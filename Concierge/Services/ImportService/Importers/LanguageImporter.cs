﻿// <copyright file="LanguageImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Characteristics;
    using Concierge.Common;
    using Concierge.Persistence.ReadWriters;

    public class LanguageImporter : Importer
    {
        public LanguageImporter(ConciergeCharacter character)
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
            this.Character.Characteristic.Languages.AddRange(languages);
        }

        public override IEnumerable<IUnique> Load(ConciergeCharacter character)
        {
            return character.Characteristic.Languages;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return DefaultListReadWriter.ReadJson<Language>(fileName);
        }
    }
}