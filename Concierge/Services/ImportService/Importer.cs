// <copyright file="Importer.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService
{
    using System;
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Common;

    public abstract class Importer
    {
        public Importer(ConciergeCharacter character)
        {
            this.Character = character;
        }

        protected ConciergeCharacter Character { get; set; }

        public abstract void Import(IEnumerable<IUnique> list);

        public abstract IEnumerable<IUnique> Load(ConciergeCharacter character);

        public abstract IEnumerable<IUnique> Load(string fileName);

        protected static void CycleGuids(IEnumerable<IUnique> list)
        {
            foreach (var item in list)
            {
                item.Id = Guid.NewGuid();
            }
        }
    }
}
