﻿// <copyright file="CharacterImporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Import
{
    using System;
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Utility;

    public sealed class CharacterImporter
    {
        private readonly ConciergeCharacter character;

        public CharacterImporter(ConciergeCharacter character)
        {
            this.character = character;
        }

        public void Import(ImportSettings importSettings)
        {
            var importFile = CharacterReadWriter.Read(importSettings.File).Character;

            this.ImportAbilities(importFile, importSettings.ImportAbilities);
            this.ImportAmmo(importFile, importSettings.ImportAmmo);
            this.ImportInventory(importFile, importSettings.ImportInventory);
            this.ImportNotes(importFile, importSettings.ImportNotes);
            this.ImportSpells(importFile, importSettings.ImportSpells);
            this.ImportWeapons(importFile, importSettings.ImportWeapons);
        }

        private static void CycleGuids(IEnumerable<IUnique> list)
        {
            foreach (var item in list)
            {
                item.Id = Guid.NewGuid();
            }
        }

        private void ImportAbilities(ConciergeCharacter importCharacter, bool importAbilities)
        {
            if (importAbilities)
            {
                Program.Logger.Info($"Import abilities.");
                CycleGuids(importCharacter.Abilities);
                this.character.Abilities.AddRange(importCharacter.Abilities);
            }
        }

        private void ImportAmmo(ConciergeCharacter importCharacter, bool importAmmo)
        {
            if (importAmmo)
            {
                Program.Logger.Info($"Import ammo.");
                CycleGuids(importCharacter.Ammunitions);
                this.character.Ammunitions.AddRange(importCharacter.Ammunitions);
            }
        }

        private void ImportInventory(ConciergeCharacter importCharacter, bool importInventory)
        {
            if (importInventory)
            {
                Program.Logger.Info($"Import inventory.");
                CycleGuids(importCharacter.Inventories);
                this.character.Inventories.AddRange(importCharacter.Inventories);
            }
        }

        private void ImportNotes(ConciergeCharacter importCharacter, bool importNotes)
        {
            if (importNotes)
            {
                Program.Logger.Info($"Import notes.");
                CycleGuids(importCharacter.Chapters);
                this.character.Chapters.AddRange(importCharacter.Chapters);
            }
        }

        private void ImportSpells(ConciergeCharacter importCharacter, bool importSpells)
        {
            if (importSpells)
            {
                Program.Logger.Info($"Import spells.");
                CycleGuids(importCharacter.Spells);
                this.character.Spells.AddRange(importCharacter.Spells);
            }
        }

        private void ImportWeapons(ConciergeCharacter importCharacter, bool importWeapons)
        {
            if (importWeapons)
            {
                Program.Logger.Info($"Import weapons.");
                CycleGuids(importCharacter.Weapons);
                this.character.Weapons.AddRange(importCharacter.Weapons);
            }
        }
    }
}