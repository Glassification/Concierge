﻿// <copyright file="CharacterReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    using Concierge.Exceptions.Enums;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.HelperInterface;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public static class CharacterReadWriter
    {
        private static readonly ConciergeMessageWindow conciergeMessageWindow = new ();

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

                return ccsFile;
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex, Severity.Release);
                Program.Modify();

                return new CcsFile();
            }
        }

        public static void Write(CcsFile ccsFile)
        {
            try
            {
                ccsFile.Version = Constants.AssemblyVersion;
                var rawJson = JsonConvert.SerializeObject(ccsFile, Formatting.Indented);

                File.WriteAllText(ccsFile.AbsolutePath, rawJson);

                Program.Unmodify();
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex, Severity.Release);
                Program.Modify();
            }
        }

        private static bool CheckVersion(string version)
        {
            if (version.IsNullOrWhiteSpace() || version.CompareTo(Constants.AssemblyVersion) < 0)
            {
                var message = string.Format(
                    "This file was saved with version {0} of Concierge. Current version is {1}.\nContinue loading?",
                    version,
                    Constants.AssemblyVersion);

                Program.Logger.Warning(message);
                var result = conciergeMessageWindow.ShowWindow(
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
    }
}