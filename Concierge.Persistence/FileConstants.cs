// <copyright file="FileConstants.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    /// <summary>
    /// Provides constants related to file operations and filters.
    /// </summary>
    public static class FileConstants
    {
        /// <summary>
        /// The file filter used for opening CCS (Concierge Character Sheet) files.
        /// </summary>
        public const string CcsOpenFilter = "*CCS (*.ccs)|*.ccs|JSON (*.json)|*.json|All files (*.*)|*.*";

        /// <summary>
        /// The file filter used for opening image files.
        /// </summary>
        public const string ImageOpenFilter = "*BMP (*.bmp)|*.bmp|JPEG (*.jpeg;*.jpg)|*.jpeg;*.jpg|PNG (*.png)|*.png|TIFF (*.tiff)|*.tiff|All files (*.*)|*.*";

        /// <summary>
        /// The file filter used for saving CCS (Concierge Character Sheet) files.
        /// </summary>
        public const string SaveFilter = "CCS (*.ccs)|*.ccs";

        /// <summary>
        /// The file filter used for saving ZIP files.
        /// </summary>
        public const string ZipFilter = "ZIP (*.zip)|*.zip";

        /// <summary>
        /// The default file name used when creating a new CCS (Concierge Character Sheet) file.
        /// </summary>
        public const string DefaultFileName = "New Character.ccs";

        /// <summary>
        /// The default file name used when creating a new AppData ZIP file.
        /// </summary>
        public const string DefaultAppDataFileName = "AppData.zip";
    }
}
