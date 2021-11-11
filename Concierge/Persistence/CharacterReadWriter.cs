// <copyright file="CharacterReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    using Concierge.Interfaces.Enums;
    using Concierge.Tools.Interface;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public static class CharacterReadWriter
    {
        public static CcsFile Read(string file)
        {
            try
            {
                var rawJson = File.ReadAllText(file);
                var ccsFile = JsonConvert.DeserializeObject<CcsFile>(rawJson);

                ccsFile.AbsolutePath = file;

                if (ConciergeSettings.CheckVersion && !CheckVersion(ccsFile.Version))
                {
                    return new CcsFile();
                }

                Program.Unmodify();
                Program.Logger.Info($"Successfully loaded {file}");

                ConciergeSettings.RefreshUnits();

                return ccsFile;
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
                Program.Modify();

                return new CcsFile();
            }
        }

        public static bool Write(CcsFile ccsFile)
        {
            try
            {
                ccsFile.Version = Program.AssemblyVersion;
                var rawJson = JsonConvert.SerializeObject(ccsFile, Formatting.Indented);

                File.WriteAllText(ccsFile.AbsolutePath, rawJson);

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

                switch (result)
                {
                    case ConciergeWindowResult.Yes:
                        Program.Logger.Info("Continue opening file.");
                        return true;
                    case ConciergeWindowResult.No:
                    default:
                        Program.Logger.Info("Cancel opening file.");
                        return false;
                }
            }

            return true;
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
    }
}
