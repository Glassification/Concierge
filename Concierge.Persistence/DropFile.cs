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
        /// Initializes a new instance of the <see cref="DropFile"/> class with the specified file path.
        /// The file is considered valid and no error message is associated with it.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        public DropFile(string filePath)
        {
            this.ErrorMessage = string.Empty;
            this.FilePath = filePath;
            this.IsValid = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropFile"/> class with the specified file path and error message.
        /// The file is considered invalid.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <param name="errorMessage">The error message associated with the file.</param>
        public DropFile(string filePath, string errorMessage)
        {
            this.ErrorMessage = errorMessage;
            this.FilePath = filePath;
            this.IsValid = false;
        }

        /// <summary>
        /// Gets or sets the error message associated with the file.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the path of the file.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets a value indicating whether the file is valid.
        /// </summary>
        public bool IsValid { get; private set; }
    }
}
