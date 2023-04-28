// <copyright file="CharacterReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    using Concierge.Character;
    using Concierge.Common.Exceptions;
    using Concierge.Common.Extensions;
    using Concierge.Configuration;
    using Concierge.Display.Enums;
    using Concierge.Tools.Display;
    using Newtonsoft.Json;

    public static class CharacterReadWriter
    {
        private const string IsJsonSearchText = "\"Character\"";

        public static CcsFile Read(string fileName)
        {
            try
            {
                CcsFile? ccsFile;
                var rawJson = File.ReadAllText(fileName);
                if (rawJson.Contains(IsJsonSearchText))
                {
                    Program.Logger.Info("No Decompressing needed.");
                    ccsFile = JsonConvert.DeserializeObject<CcsFile>(rawJson);
                }
                else
                {
                    Program.Logger.Info("Decompressing file.");
                    var compressedJson = File.ReadAllBytes(fileName);
                    ccsFile = JsonConvert.DeserializeObject<CcsFile>(CcsCompression.Unzip(compressedJson));
                }

                if (ccsFile is null)
                {
                    throw new NullValueException(nameof(ccsFile));
                }

                ccsFile.AbsolutePath = fileName;
                if (!CheckHash(ccsFile) || (AppSettingsManager.UserSettings.CheckVersion && !CheckVersion(ccsFile.Version)))
                {
                    return new CcsFile();
                }

                AppSettingsManager.RefreshUnits();
                Program.Logger.Info($"Successfully loaded {fileName}");

                Initialize(ccsFile.Character);

                return ccsFile;
            }
            catch (Exception ex)
            {
                ex = ex.TryConvertToReadWriterException(fileName);

                Program.ErrorService.LogError(ex);
                Program.Modify();

                return new CcsFile(true);
            }
        }

        public static bool Write(CcsFile ccsFile)
        {
            try
            {
                ccsFile.Version = Program.AssemblyVersion;
                ccsFile.LastSaveDate = DateTime.Now;
                ccsFile.Hash = CcsHashing.HashCharacter(ccsFile.Character);
                var rawJson = JsonConvert.SerializeObject(ccsFile, Formatting.Indented);

                if (AppSettingsManager.StartUp.CompressCharacterSheet || !Program.IsDebug)
                {
                    Program.Logger.Info("Compressing file.");
                    File.WriteAllBytes(ccsFile.AbsolutePath, CcsCompression.Zip(rawJson));
                }
                else
                {
                    Program.Logger.Info("No Compressing needed.");
                    File.WriteAllText(ccsFile.AbsolutePath, rawJson);
                }

                Program.Unmodify();
                Program.Logger.Info($"Successfully saved to {ccsFile.AbsolutePath}");

                return true;
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
                Program.Modify();

                return false;
            }
        }

        private static bool CheckVersion(string version)
        {
            if (version.IsNullOrWhiteSpace() || CompareMajorMinorVersion(version))
            {
                var message = string.Format(
                    "This file was saved with version {0} of Concierge. Current version is {1}.\nContinue loading?",
                    version,
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

        private static bool CheckHash(CcsFile ccsFile)
        {
            if (CcsHashing.CheckHash(ccsFile))
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

        private static bool CompareMajorMinorVersion(string fileVersion)
        {
            var fileVersions = fileVersion.Split('.');
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

        private static void Initialize(ConciergeCharacter character)
        {
            foreach (var weapon in character.Weapons)
            {
                weapon.Initialize(character);
            }

            foreach (var weapon in character.Companion.Attacks)
            {
                weapon.Initialize(character.Companion);
            }
        }
    }
}
