// <copyright file="CcsFile.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System.IO;

    using Concierge.Character;
    using Concierge.Utility;
    using Newtonsoft.Json;

    public class CcsFile
    {
        public CcsFile()
        {
            this.Character = new ConciergeCharacter();
            this.OriginalCreationDate = ConciergeDateTime.OriginalCreationNow;
            this.Version = Program.AssemblyVersion;
            this.AbsolutePath = string.Empty;
            this.Hash = string.Empty;
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

        public string Version { get; set; }
    }
}
