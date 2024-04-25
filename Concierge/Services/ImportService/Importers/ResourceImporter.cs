// <copyright file="ResourceImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService.Importers
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Vitals;
    using Concierge.Common;

    /// <summary>
    /// Represents an importer for class resource data.
    /// </summary>
    public sealed class ResourceImporter : Importer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceImporter"/> class with the specified character.
        /// </summary>
        /// <param name="character">The character sheet to import class resource data into.</param>
        public ResourceImporter(CharacterSheet character)
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

        public override IEnumerable<IUnique> Load(CharacterSheet character)
        {
            return character.Vitality.ClassResources;
        }

        public override IEnumerable<IUnique> Load(string fileName)
        {
            return this.ReadWriter.ReadJson<List<ClassResource>>(fileName);
        }
    }
}
