// <copyright file="CharacterImportService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService
{
    using System;
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Common;
    using Concierge.Persistence.ReadWriters;

    public sealed class CharacterImportService
    {
        private readonly ConciergeCharacter character;

        public CharacterImportService(ConciergeCharacter character)
        {
            this.character = character;
        }

        public bool Import(ImportSettings importSettings)
        {
            var importFile = CharacterReadWriter.Read(importSettings.File);
            if (importFile.IsEmpty)
            {
                return false;
            }

            var character = importFile.Character;
            this.ImportAbilities(character, importSettings.ImportAbilities);
            this.ImportAmmo(character, importSettings.ImportAmmo);
            this.ImportInventory(character, importSettings.ImportInventory);
            this.ImportJournal(character, importSettings.ImportJournal);
            this.ImportLanguage(character, importSettings.ImportLanguage);
            this.ImportProficiency(character, importSettings.ImportProficiency);
            this.ImportSpells(character, importSettings.ImportSpells);
            this.ImportWeapons(character, importSettings.ImportWeapons);

            return true;
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
                CycleGuids(importCharacter.Characteristic.Abilities);
                this.character.Characteristic.Abilities.AddRange(importCharacter.Characteristic.Abilities);
            }
        }

        private void ImportAmmo(ConciergeCharacter importCharacter, bool importAmmo)
        {
            if (importAmmo)
            {
                Program.Logger.Info($"Import ammo.");
                CycleGuids(importCharacter.Equipment.Ammunition);
                this.character.Equipment.Ammunition.AddRange(importCharacter.Equipment.Ammunition);
            }
        }

        private void ImportInventory(ConciergeCharacter importCharacter, bool importInventory)
        {
            if (importInventory)
            {
                Program.Logger.Info($"Import inventory.");
                CycleGuids(importCharacter.Equipment.Inventory);
                this.character.Equipment.Inventory.AddRange(importCharacter.Equipment.Inventory);
            }
        }

        private void ImportJournal(ConciergeCharacter importCharacter, bool importJournal)
        {
            if (importJournal)
            {
                Program.Logger.Info($"Import journal.");
                CycleGuids(importCharacter.Journal.Chapters);
                this.character.Journal.Chapters.AddRange(importCharacter.Journal.Chapters);
            }
        }

        private void ImportLanguage(ConciergeCharacter importCharacter, bool importLanguage)
        {
            if (importLanguage)
            {
                Program.Logger.Info($"Import language.");
                CycleGuids(importCharacter.Characteristic.Languages);
                this.character.Characteristic.Languages.AddRange(importCharacter.Characteristic.Languages);
            }
        }

        private void ImportProficiency(ConciergeCharacter importCharacter, bool importProficiency)
        {
            if (importProficiency)
            {
                Program.Logger.Info($"Import proficiency.");
                CycleGuids(importCharacter.Characteristic.Proficiencies);
                this.character.Characteristic.Proficiencies.AddRange(importCharacter.Characteristic.Proficiencies);
            }
        }

        private void ImportSpells(ConciergeCharacter importCharacter, bool importSpells)
        {
            if (importSpells)
            {
                Program.Logger.Info($"Import spells.");
                CycleGuids(importCharacter.Magic.Spells);
                this.character.Magic.Spells.AddRange(importCharacter.Magic.Spells);
            }
        }

        private void ImportWeapons(ConciergeCharacter importCharacter, bool importWeapons)
        {
            if (importWeapons)
            {
                Program.Logger.Info($"Import weapons.");
                CycleGuids(importCharacter.Equipment.Weapons);
                this.character.Equipment.Weapons.AddRange(importCharacter.Equipment.Weapons);
            }
        }
    }
}
