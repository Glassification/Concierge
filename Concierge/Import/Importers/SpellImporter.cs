// <copyright file="SpellImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Import.Importers
{
    using System.Collections.Generic;

    using Concierge;
    using Concierge.Character;
    using Concierge.Character.Magic;
    using Concierge.Common;

    /// <summary>
    /// Represents an importer for spell data.
    /// </summary>
    public sealed class SpellImporter : Importer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpellImporter"/> class with the specified character.
        /// </summary>
        /// <param name="character">The character sheet to import spell data into.</param>
        public SpellImporter(CharacterSheet character)
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
            this.Character.SpellCasting.Spells.AddRange(spells);
        }

        public override IEnumerable<IUnique> Load(CharacterSheet character)
        {
            return character.SpellCasting.Spells;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<Spell>>(fileName);
        }
    }
}
