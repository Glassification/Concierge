// <copyright file="DefaultFolders.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    /// <summary>
    /// Represents default folder settings, including open and save folders, and options to use them.
    /// </summary>
    public class DefaultFolders
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFolders"/> class with default settings.
        /// </summary>
        public DefaultFolders()
        {
            this.OpenFolder = string.Empty;
            this.SaveFolder = string.Empty;
        }

        /// <summary>
        /// Gets or sets the default open folder path.
        /// </summary>
        public string OpenFolder { get; set; }

        /// <summary>
        /// Gets or sets the default save folder path.
        /// </summary>
        public string SaveFolder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the default open folder.
        /// </summary>
        public bool UseOpenFolder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the default save folder.
        /// </summary>
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

        /// <summary>
        /// Sets the properties of the current <see cref="DefaultFolders"/> instance with the values of another <see cref="DefaultFolders"/> instance.
        /// </summary>
        /// <param name="defaultFolders">The <see cref="DefaultFolders"/> object from which to copy the properties.</param>
        public void Set(DefaultFolders defaultFolders)
        {
            this.OpenFolder = defaultFolders.OpenFolder;
            this.SaveFolder = defaultFolders.SaveFolder;
            this.UseSaveFolder = defaultFolders.UseSaveFolder;
            this.UseOpenFolder = defaultFolders.UseOpenFolder;
        }
    }
}
