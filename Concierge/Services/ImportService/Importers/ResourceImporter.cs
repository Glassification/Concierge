﻿// <copyright file="ResourceImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Concierge.Persistence.ReadWriters;

    public class ResourceImporter : Importer
    {
        public ResourceImporter(ConciergeCharacter character)
            : base(character)
        {
        }

        public override void Import(IEnumerable<IUnique> list)
        {
            if (list is not List<ClassResource> resources)
            {
                return;
            }

            Program.Logger.Info($"Import resource.");
            CycleGuids(resources);
            this.Character.Vitality.ClassResources.AddRange(resources);
        }

        public override IEnumerable<IUnique> Load(ConciergeCharacter character)
        {
            return character.Vitality.ClassResources;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return DefaultListReadWriter.ReadJson<ClassResource>(fileName);
        }
    }
}