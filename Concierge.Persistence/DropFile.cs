// <copyright file="DropFile.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    /// <summary>
    /// Represents a file object used for handling dropped files.
    /// </summary>
    public sealed class DropFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropFile"/> class.
        /// </summary>
        /// <param name="filePath">The path of the dropped file.</param>
        /// <param name="isValid">Indicates whether the dropped file is valid.</param>
        public DropFile(string filePath, bool isValid)
        {
            this.FilePath = filePath;
            this.IsValid = isValid;
        }

        /// <summary>
        /// Gets an empty <see cref="DropFile"/> instance.
        /// </summary>
        public static DropFile Empty => new (string.Empty, false);

        /// <summary>
        /// Gets or sets the full path of the dropped file.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dropped file is valid.
        /// </summary>
        public bool IsValid { get; set; }
    }
}
