// <copyright file="Importer.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System;
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Common;
    using Concierge.Persistence.ReadWriters;

    public abstract class Importer
    {
        public Importer(ConciergeCharacter character)
        {
            this.ReadWriter = new DefaultListReadWriter(Program.ErrorService, Program.Logger);
            this.Character = character;
        }

        protected IReadWriters ReadWriter { get; private set; }

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
