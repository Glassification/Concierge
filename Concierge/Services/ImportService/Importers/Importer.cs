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

    /// <summary>
    /// An abstract class representing an importer for character sheet data.
    /// </summary>
    public abstract class Importer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Importer"/> class with the specified character.
        /// </summary>
        /// <param name="character">The character sheet to import data into.</param>
        public Importer(CharacterSheet character)
        {
            this.ReadWriter = new DefaultListReadWriter(Program.ErrorService, Program.Logger);
            this.Character = character;
        }

        /// <summary>
        /// Gets the read-writer instance used for importing data.
        /// </summary>
        protected IReadWriters ReadWriter { get; private set; }

        /// <summary>
        /// Gets or sets the character sheet instance.
        /// </summary>
        protected CharacterSheet Character { get; set; }

        /// <summary>
        /// Imports data into the character sheet from the provided list.
        /// </summary>
        /// <param name="list">The list of items to import.</param>
        public abstract void Import(IEnumerable<IUnique> list);

        /// <summary>
        /// Loads data into the character sheet based on its properties.
        /// </summary>
        /// <param name="character">The character sheet to load data into.</param>
        /// <returns>The list of items loaded.</returns>
        public abstract IEnumerable<IUnique> Load(CharacterSheet character);

        /// <summary>
        /// Loads data into the character sheet from the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to load data from.</param>
        /// <returns>The list of items loaded.</returns>
        public abstract IEnumerable<IUnique> Load(string fileName);

        /// <summary>
        /// Generates new GUIDs for each item in the provided list.
        /// </summary>
        /// <param name="list">The list of items to cycle GUIDs for.</param>
        protected static void CycleGuids(IEnumerable<IUnique> list)
        {
            foreach (var item in list)
            {
                item.Id = Guid.NewGuid();
            }
        }
    }
}
