// <copyright file="StatusEffectImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Vitals;
    using Concierge.Common;

    /// <summary>
    /// Represents an importer for status effect data.
    /// </summary>
    public sealed class StatusEffectImporter : Importer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectImporter"/> class with the specified character.
        /// </summary>
        /// <param name="character">The character sheet to import status effect data into.</param>
        public StatusEffectImporter(CharacterSheet character)
            : base(character)
        {
        }

        public override void Import(IEnumerable<IUnique> list)
        {
            if (list is not List<StatusEffect> effects)
            {
                return;
            }

            Program.Logger.Info($"Import status effect.");
            CycleGuids(effects);
            this.Character.Vitality.Status.StatusEffects.AddRange(effects);
        }

        public override IEnumerable<IUnique> Load(CharacterSheet character)
        {
            return character.Vitality.Status.StatusEffects;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<StatusEffect>>(fileName);
        }
    }
}
