// <copyright file="CharacterLoader.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    using Concierge.Exceptions.Enums;
    using Concierge.Presentation.Enums;
    using Concierge.Presentation.HelperUi;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public static class CharacterLoader
    {
        static CharacterLoader()
        {
            ConciergeMessageWindow = new ConciergeMessageWindow();
        }

        private static ConciergeMessageWindow ConciergeMessageWindow { get; }

        public static CcsFile LoadCharacterSheetJson(string file)
        {
            try
            {
                var rawJson = File.ReadAllText(file);
                var ccsFile = JsonConvert.DeserializeObject<CcsFile>(rawJson);

                ccsFile.AbsolutePath = file;

                if (ccsFile.CheckVersion && !CheckVersion(ccsFile.Version))
                {
                    return new CcsFile();
                }

                Program.Modified = false;

                return ccsFile;
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex, Severity.Release);
                Program.Modified = true;

                return new CcsFile();
            }
        }

        private static bool CheckVersion(string version)
        {
            if (version.IsNullOrWhiteSpace() || version.CompareTo(Constants.AssemblyVersion) > 0)
            {
                var message = string.Format(
                    "This file was saved with version {0} of Concierge. Current version is {1}.\nContinue loading?",
                    version,
                    Constants.AssemblyVersion);

                Program.Logger.Warning(message);
                var result = ConciergeMessageWindow.ShowWindow(Regex.Unescape(message), MessageWindowButtons.YesNo);

                switch (result)
                {
                    case MessageWindowResult.Yes:
                        Program.Logger.Info("Continue opening file.");
                        return true;
                    case MessageWindowResult.No:
                    default:
                        Program.Logger.Info("Cancel opening file.");
                        return false;
                }
            }

            return true;
        }
    }
}
