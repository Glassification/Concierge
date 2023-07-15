// <copyright file="StatusEffectImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Vitals;
    using Concierge.Common;

    public class StatusEffectImporter : Importer
    {
        public StatusEffectImporter(ConciergeCharacter character)
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
            this.Character.Vitality.StatusEffects.AddRange(effects);
        }

        public override IEnumerable<IUnique> Load(ConciergeCharacter character)
        {
            return character.Vitality.StatusEffects;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<StatusEffect>>(fileName);
        }
    }
}
