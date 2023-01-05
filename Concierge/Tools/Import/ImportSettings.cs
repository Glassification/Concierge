// <copyright file="ImportSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Import
{
    public sealed class ImportSettings
    {
        public ImportSettings()
        {
            this.File = string.Empty;
        }

        public string File { get; set; }

        public bool ImportAbilities { get; set; }

        public bool ImportAmmo { get; set; }

        public bool ImportInventory { get; set; }

        public bool ImportNotes { get; set; }

        public bool ImportSpells { get; set; }

        public bool ImportWeapons { get; set; }
    }
}
