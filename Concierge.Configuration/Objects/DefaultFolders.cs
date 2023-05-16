// <copyright file="DefaultFolders.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    public class DefaultFolders
    {
        public DefaultFolders()
        {
            this.OpenFolder = string.Empty;
            this.SaveFolder = string.Empty;
        }

        public string OpenFolder { get; set; }

        public string SaveFolder { get; set; }

        public bool UseOpenFolder { get; set; }

        public bool UseSaveFolder { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not DefaultFolders defaultFolders)
            {
                return false;
            }

            return
                defaultFolders.OpenFolder.Equals(this.OpenFolder) &&
                defaultFolders.SaveFolder.Equals(this.SaveFolder) &&
                defaultFolders.UseOpenFolder == this.UseOpenFolder &&
                defaultFolders.UseSaveFolder == this.UseSaveFolder;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Set(DefaultFolders defaultFolders)
        {
            this.OpenFolder = defaultFolders.OpenFolder;
            this.SaveFolder = defaultFolders.SaveFolder;
            this.UseSaveFolder = defaultFolders.UseSaveFolder;
            this.UseOpenFolder = defaultFolders.UseOpenFolder;
        }
    }
}
