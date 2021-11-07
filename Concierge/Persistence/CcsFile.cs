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
            this.Default();
        }

        public ConciergeCharacter Character { get; set; }

        public string Version { get; set; }

        [JsonIgnore]
        public string FileName => Path.GetFileName(this.AbsolutePath);

        [JsonIgnore]
        public string FilePath => Path.GetDirectoryName(this.AbsolutePath);

        [JsonIgnore]
        public string AbsolutePath { get; set; }

        public void Default()
        {
            this.Character = new ConciergeCharacter();
            this.Version = Program.AssemblyVersion;
        }
    }
}
