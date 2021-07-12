// <copyright file="CcsFile.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System.IO;

    using Concierge.Characters;
    using Newtonsoft.Json;

    public class CcsFile
    {
        public CcsFile()
        {
            this.Character = new Character();
            this.Default();
        }

        public Character Character { get; set; }

        public bool AutosaveEnable { get; set; }

        public int AutosaveInterval { get; set; }

        public bool UseCoinWeight { get; set; }

        public bool UseEncumbrance { get; set; }

        [JsonIgnore]
        public string FileName => Path.GetFileName(this.AbsolutePath);

        [JsonIgnore]
        public string FilePath => Path.GetDirectoryName(this.AbsolutePath);

        [JsonIgnore]
        public string AbsolutePath { get; set; }

        public void Default()
        {
            this.AutosaveEnable = false;
            this.AutosaveInterval = 1;
            this.UseCoinWeight = false;
            this.UseEncumbrance = false;
        }
    }
}
