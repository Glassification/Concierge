﻿// <copyright file="CcsFile.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    using Concierge.Character;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Display;
    using Concierge.Display.Enums;
    using Concierge.Persistence;
    using Newtonsoft.Json;

    public sealed class CcsFile
    {
        public CcsFile()
        {
            this.Character = new ConciergeCharacter();
            this.OriginalCreationDate = ConciergeDateTime.OriginalCreationNow;
            this.Version = Program.AssemblyVersion;
            this.AbsolutePath = string.Empty;
            this.Hash = string.Empty;
        }

        public CcsFile(bool empty)
            : this()
        {
            this.IsEmpty = empty;
        }

        [JsonIgnore]
        public string AbsolutePath { get; set; }

        public ConciergeCharacter Character { get; set; }

        [JsonIgnore]
        public string FileName => Path.GetFileName(this.AbsolutePath) ?? string.Empty;

        [JsonIgnore]
        public string FilePath => Path.GetDirectoryName(this.AbsolutePath) ?? string.Empty;

        public string Hash { get; set; }

        public string OriginalCreationDate { get; init; }

        public DateTime LastSaveDate { get; set; }

        public string Version { get; set; }

        public bool IsEmpty { get; }

        public bool IsFileSaved(bool? autosaveChecked)
        {
            return this.AbsolutePath.IsNullOrWhiteSpace() && (autosaveChecked ?? false);
        }

        public bool CheckVersion()
        {
            if (this.Version.IsNullOrWhiteSpace() || this.CompareMajorMinorVersion())
            {
                var message = string.Format(
                    "This file was saved with version {0} of Concierge. Current version is {1}.\nContinue loading?",
                    this.Version,
                    Program.AssemblyVersion);

                Program.Logger.Warning(message);
                var result = ConciergeMessageBox.Show(
                    Regex.Unescape(message),
                    "Warning",
                    ConciergeWindowButtons.YesNo,
                    ConciergeWindowIcons.Alert);

                Program.Logger.Info($"{(result == ConciergeWindowResult.OK ? "Continue" : "Cancel")} opening file.");
                return result switch
                {
                    ConciergeWindowResult.Yes => true,
                    _ => false,
                };
            }

            return true;
        }

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
                ConciergeWindowButtons.YesNo,
                ConciergeWindowIcons.Alert);

            Program.Logger.Info($"{(result == ConciergeWindowResult.OK ? "Continue" : "Cancel")} opening file.");
            return result switch
            {
                ConciergeWindowResult.Yes => true,
                _ => false,
            };
        }

        public void Initialize()
        {
            foreach (var weapon in this.Character.Equipment.Weapons)
            {
                weapon.Initialize(this.Character);
            }

            foreach (var weapon in this.Character.Companion.Equipment.Weapons)
            {
                weapon.Initialize(this.Character.Companion);
            }
        }

        private bool CompareMajorMinorVersion()
        {
            var fileVersions = this.Version.Split('.');
            var programVersions = Program.AssemblyVersion.Split('.');

            if (fileVersions.Length != 3 || programVersions.Length != 3)
            {
                return true;
            }

            if (!fileVersions[0].Equals(programVersions[0]) || !fileVersions[1].Equals(programVersions[1]))
            {
                return true;
            }

            return false;
        }
    }
}