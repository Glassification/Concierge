// <copyright file="AppDataExporter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.Exporters
{
    using System;
    using System.IO;
    using System.IO.Compression;

    using Concierge.Common;
    using Concierge.Logging;
    using Microsoft.Win32;

    /// <summary>
    /// Provides functionality to export application data to a compressed ZIP file.
    /// </summary>
    public sealed class AppDataExporter : IExporter
    {
        private readonly Logger logger;
        private readonly SaveFileDialog saveFileDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDataExporter"/> class.
        /// </summary>
        /// <param name="logger">The logger for recording log messages.</param>
        public AppDataExporter(Logger logger)
        {
            this.logger = logger;
            this.saveFileDialog = new SaveFileDialog
            {
                FileName = FileConstants.DefaultAppDataFileName,
                Filter = FileConstants.ZipFilter,
                DefaultExt = "zip",
                FilterIndex = 0,
            };
        }

        /// <summary>
        /// Exports application data to a compressed ZIP file.
        /// </summary>
        /// <returns><c>true</c> if the export operation is successful; otherwise, <c>false</c>.</returns>
        public bool Export()
        {
            try
            {
                if (this.saveFileDialog.ShowDialog() ?? false)
                {
                    var file = this.saveFileDialog.FileName;
                    this.logger.Info($"Exporting AppData to {file}");

                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }

                    ZipFile.CreateFromDirectory(ConciergeFiles.AppDataDirectory, file);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
                return false;
            }
        }
    }
}
