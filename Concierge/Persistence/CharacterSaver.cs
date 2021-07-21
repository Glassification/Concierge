// <copyright file="CharacterSaver.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.IO;

    using Concierge.Exceptions.Enums;
    using Concierge.Utility;
    using Newtonsoft.Json;

    public static class CharacterSaver
    {
        public static void SaveCharacterSheetJson(CcsFile ccsFile)
        {
            try
            {
                ccsFile.Version = Constants.AssemblyVersion;
                var rawJson = JsonConvert.SerializeObject(ccsFile, Formatting.Indented);

                File.WriteAllText(ccsFile.AbsolutePath, rawJson);

                Program.Modified = false;
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex, Severity.Release);
                Program.Modified = true;
            }
        }
    }
}
