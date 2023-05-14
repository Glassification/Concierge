// <copyright file="CharacterImportService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService
{
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Persistence.ReadWriters;
    using Concierge.Services.Enums;
    using Concierge.Services.ImportService.Importers;

    public sealed class CharacterImportService
    {
        public CharacterImportService()
        {
            this.Importers = new List<Importer>();
        }

        private List<Importer> Importers { get; set; }

        public bool Import(ImportTypes importType, string filename)
        {
            return importType switch
            {
                ImportTypes.Character => this.ImportCharacter(filename),
                ImportTypes.Object => this.ImportSingle(filename),
                _ => false,
            };
        }

        public void AddImporter(Importer importer)
        {
            this.Importers.Add(importer);
        }

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
            var importFile = CharacterReadWriter.Read(filename);
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
