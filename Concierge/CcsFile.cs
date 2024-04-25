// <copyright file="CcsFile.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Concierge.Character;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Display;
    using Concierge.Display.Enums;
    using Concierge.Persistence;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a Concierge character file, managing character data, file paths, and version information.
    /// </summary>
    public sealed class CcsFile
    {
        private bool isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="CcsFile"/> class.
        /// </summary>
        public CcsFile()
        {
            this.Character = new CharacterSheet();
            this.CharacterService = new CharacterService(this.Character);
            this.OriginalCreationDate = ConciergeDateTime.OriginalCreationNow;
            this.Version = new ConciergeVersion();
            this.AbsolutePath = string.Empty;
            this.Hash = string.Empty;
        }

        /// <summary>
        /// Gets or sets the absolute path of the character file.
        /// </summary>
        [JsonIgnore]
        public string AbsolutePath { get; set; }

        /// <summary>
        /// Gets or sets the character sheet associated with the file.
        /// </summary>
        public CharacterSheet Character { get; set; }

        /// <summary>
        /// Gets the character service instance associated with the file.
        /// </summary>
        public CharacterService CharacterService { get; private set; }

        /// <summary>
        /// Gets the file name extracted from the absolute path.
        /// </summary>
        [JsonIgnore]
        public string FileName => Path.GetFileName(this.AbsolutePath) ?? string.Empty;

        /// <summary>
        /// Gets the directory path extracted from the absolute path.
        /// </summary>
        [JsonIgnore]
        public string FilePath => Path.GetDirectoryName(this.AbsolutePath) ?? string.Empty;

        /// <summary>
        /// Gets or sets the hash value of the character data.
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Gets the original creation date of the character file.
        /// </summary>
        public string OriginalCreationDate { get; init; }

        /// <summary>
        /// Gets or sets the date and time when the file was last saved.
        /// </summary>
        public DateTime LastSaveDate { get; set; }

        /// <summary>
        /// Gets or sets the version information of the character file.
        /// </summary>
        public ConciergeVersion Version { get; set; }

        /// <summary>
        /// Gets a value indicating whether the character file is empty.
        /// </summary>
        public bool IsEmpty { get; }

        /// <summary>
        /// Checks if the character file is saved.
        /// </summary>
        /// <param name="autosaveChecked">A nullable boolean indicating if autosave is checked.</param>
        /// <returns><c>true</c> if the character file is saved; otherwise, <c>false</c>.</returns>
        public bool IsFileSaved(bool? autosaveChecked)
        {
            return this.AbsolutePath.IsNullOrWhiteSpace() && (autosaveChecked ?? false);
        }

        /// <summary>
        /// Checks if the version of the character file is compatible with the current program version.
        /// </summary>
        /// <returns><c>true</c> if the version is compatible; otherwise, <c>false</c>.</returns>
        public bool CheckVersion()
        {
            if (this.Version.IsEmpty || this.CompareMajorMinorVersion())
            {
                var message = string.Format(
                    "This file was saved with version {0} of Concierge. Current version is {1}.\nContinue loading?",
                    this.Version,
                    Program.AssemblyVersion);

                Program.Logger.Warning(message);
                var result = ConciergeMessageBox.Show(
                    Regex.Unescape(message),
                    "Warning",
                    ConciergeButtons.Yes | ConciergeButtons.No,
                    ConciergeIcons.Alert);

                Program.Logger.Info($"{(result == ConciergeResult.OK ? "Continue" : "Cancel")} opening file.");
                return result switch
                {
                    ConciergeResult.Yes => true,
                    _ => false,
                };
            }

            return true;
        }

        /// <summary>
        /// Checks the integrity of the character file's data using its hash value.
        /// </summary>
        /// <returns><c>true</c> if the file integrity is verified; otherwise, <c>false</c>.</returns>
        public bool CheckHash()
        {
            if (ConciergeHashing.CheckHash(this.Character, this.Hash))
            {
                return true;
            }

            var message = "The integrity of this file cannot be verified.\nContinue loading?";

            Program.Logger.Warning(message);
            var result = ConciergeMessageBox.Show(
                Regex.Unescape(message),
                "Warning",
                ConciergeButtons.Yes | ConciergeButtons.No,
                ConciergeIcons.Alert);

            Program.Logger.Info($"{(result == ConciergeResult.OK ? "Continue" : "Cancel")} opening file.");
            return result switch
            {
                ConciergeResult.Yes => true,
                _ => false,
            };
        }

        /// <summary>
        /// Initializes the character file, ensuring that all necessary components are set up.
        /// </summary>
        public void Initialize()
        {
            if (this.isInitialized)
            {
                return;
            }

            this.CharacterService = new CharacterService(this.Character);
            foreach (var weapon in this.Character.Equipment.Weapons)
            {
                weapon.Initialize(this.CharacterService);
            }

            foreach (var weapon in this.Character.Companion.Weapons)
            {
                weapon.Initialize(this.CharacterService);
            }

            foreach (var magicalClass in this.Character.SpellCasting.MagicalClasses)
            {
                magicalClass.Initialize(this.CharacterService);
            }

            var concentratedSpells = this.Character.SpellCasting.Spells.Where(x => x.CurrentConcentration).ToList();
            if (concentratedSpells.Count > 1)
            {
                foreach (var spell in concentratedSpells)
                {
                    spell.CurrentConcentration = false;
                }
            }

            this.isInitialized = true;
        }

        private bool CompareMajorMinorVersion()
        {
            return this.Version.Major != Program.AssemblyVersion.Major || this.Version.Minor != Program.AssemblyVersion.Minor;
        }
    }
}
