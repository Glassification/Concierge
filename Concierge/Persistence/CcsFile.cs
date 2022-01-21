// <copyright file="CcsFile.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System.IO;

    using Concierge.Character;
    using Newtonsoft.Json;

    public class CcsFile
    {
        public CcsFile()
        {
            this.Character = new ConciergeCharacter();
            this.Version = Program.AssemblyVersion;
            this.AbsolutePath = string.Empty;
        }

        public ConciergeCharacter Character { get; set; }

        public string Version { get; set; }

        [JsonIgnore]
        public string FileName => Path.GetFileName(this.AbsolutePath);

        [JsonIgnore]
        public string FilePath => Path.GetDirectoryName(this.AbsolutePath) ?? string.Empty;

        [JsonIgnore]
        public string AbsolutePath { get; set; }
    }
}
