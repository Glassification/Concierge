// <copyright file="SpellImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Spellcasting;
    using Concierge.Common;

    public sealed class SpellImporter : Importer
    {
        public SpellImporter(ConciergeCharacter character)
            : base(character)
        {
        }

        public override void Import(IEnumerable<IUnique> list)
        {
            if (list is not List<Spell> spells)
            {
                return;
            }

            Program.Logger.Info($"Import spells.");
            CycleGuids(spells);
            this.Character.Magic.Spells.AddRange(spells);
        }

        public override IEnumerable<IUnique> Load(ConciergeCharacter character)
        {
            return character.Magic.Spells;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Spell>>(fileName);
        }
    }
}
