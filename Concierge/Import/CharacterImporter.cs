// <copyright file="CharacterImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Import
{
    using System.Collections.Generic;
    using System.Linq;

    using Concierge;
    using Concierge.Import.Enums;
    using Concierge.Import.Importers;
    using Concierge.Persistence.ReadWriters;

    /// <summary>
    /// Service for importing characters and other data.
    /// </summary>
    public sealed class CharacterImporter
    {
        private readonly CharacterReadWriter readwriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterImporter"/> class.
        /// </summary>
        public CharacterImporter()
        {
            this.readwriter = new CharacterReadWriter(Program.ErrorService, Program.Logger);
            this.Importers = [];
        }

        private List<Importer> Importers { get; set; }

        /// <summary>
        /// Imports data based on the specified import type and filename.
        /// </summary>
        /// <param name="importType">The type of import.</param>
        /// <param name="filename">The filename or path of the file to import.</param>
        /// <returns>True if the import was successful; otherwise, false.</returns>
        public bool Import(ImportTypes importType, string filename)
        {
            return importType switch
            {
                ImportTypes.Character => this.ImportCharacter(filename),
                ImportTypes.Single => this.ImportSingle(filename),
                _ => false,
            };
        }

        /// <summary>
        /// Adds an importer to the list of importers.
        /// </summary>
        /// <param name="importer">The importer to add.</param>
        public void AddImporter(Importer importer)
        {
            this.Importers.Add(importer);
        }

        /// <summary>
        /// Clears all importers from the list.
        /// </summary>
        public void ClearImporters()
        {
            this.Importers.Clear();
        }

        private bool ImportSingle(string filename)
        {
            var importer = this.Importers.FirstOrDefault();
            if (importer is null)
            {
                return false;
            }

            var list = importer.Load(filename);
            if (!list.Any())
            {
                return false;
            }

            importer.Import(list);
            this.ClearImporters();
            return true;
        }

        private bool ImportCharacter(string filename)
        {
            var importFile = this.readwriter.ReadJson<CcsFile>(filename);
            if (importFile.IsEmpty)
            {
                return false;
            }

            foreach (var importer in this.Importers)
            {
                var list = importer.Load(importFile.Character);
                if (!list.Any())
                {
                    continue;
                }

                importer.Import(list);
            }

            this.ClearImporters();
            return true;
        }
    }
}
