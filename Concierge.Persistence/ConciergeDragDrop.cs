// <copyright file="ConciergeDragDrop.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Navigation;

    /// <summary>
    /// Utility class for capturing file paths from IDataObject in drag-and-drop operations.
    /// </summary>
    public static class ConciergeDragDrop
    {
        /// <summary>
        /// Gets a value indicating whether the drag-and-drop operation has been handled.
        /// </summary>
        public static bool IsHandled { get; private set; }

        /// <summary>
        /// Retrieves the first file path from IDataObject if it matches the specified file extension.
        /// </summary>
        /// <param name="data">The IDataObject containing dragged data.</param>
        /// <param name="extension">The file extension (e.g., ".txt") to match.</param>
        /// <returns>The first file path found that matches the extension, or an empty string if no valid file found.</returns>
        public static DropFile Capture(IDataObject data, string extension)
        {
            if (!data.GetDataPresent(DataFormats.FileDrop))
            {
                return DropFile.Empty;
            }

            var file = ((string[])data.GetData(DataFormats.FileDrop)).FirstOrDefault(string.Empty);
            if (!Path.GetExtension(file).Equals(extension, StringComparison.InvariantCultureIgnoreCase))
            {
                return new DropFile(file, false);
            }

            return new DropFile(file, true);
        }

        /// <summary>
        /// Marks the drag-and-drop operation as handled.
        /// </summary>
        public static void Handle()
        {
            IsHandled = true;
        }

        /// <summary>
        /// Resets the handled status of the drag-and-drop operation.
        /// </summary>
        public static void Reset()
        {
            IsHandled = false;
        }
    }
}
