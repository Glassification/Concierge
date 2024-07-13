// <copyright file="LocalLogger.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Logging
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    using Concierge.Common;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents a local file logger that extends the base <see cref="Logger"/> class.
    /// This class is responsible for logging messages to a local file.
    /// </summary>
    public sealed class LocalLogger : Logger
    {
        /// <summary>
        /// The default log file name.
        /// </summary>
        public const string DefaultLogFileName = @"Concierge.log";

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalLogger"/> class with the provided log path and debug mode flag.
        /// </summary>
        /// <param name="logPath">The path to the log files directory.</param>
        /// <param name="isDebug">Indicates whether the logger is running in debug mode.</param>
        public LocalLogger(string logPath, bool isDebug = false)
        : this(logPath, string.Empty, isDebug)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalLogger"/> class with the provided log path, log name, and debug mode flag.
        /// </summary>
        /// <param name="logPath">The path to the log files directory.</param>
        /// <param name="logName">The name of the log file.</param>
        /// <param name="isDebug">Indicates whether the logger is running in debug mode.</param>
        public LocalLogger(string logPath, string logName, bool isDebug = false)
        : base(isDebug)
        {
            this.LogLocation = logPath;
            this.LogFileName = FormatLogFileName(logName);

            this.MaxLogFileSize = 5000000;
            this.MaxLogFiles = 5;
            this.MaxLogArchives = 5;
            this.MaxLogRetentionDays = 90;

            Directory.CreateDirectory(this.LogLocation);
            this.Rotate(Path.Combine(this.LogLocation, this.LogFileName));
        }

        /// <summary>
        /// Gets or sets the maximum log file size in bytes.
        /// </summary>
        public int MaxLogFileSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of log files to keep.
        /// </summary>
        public int MaxLogFiles { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of log archives to keep.
        /// </summary>
        public int MaxLogArchives { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of days to retain log files.
        /// </summary>
        public int MaxLogRetentionDays { get; set; }

        /// <summary>
        /// Gets the log files directory location.
        /// </summary>
        public string LogLocation { get; private set; }

        /// <summary>
        /// Gets the log file name.
        /// </summary>
        public string LogFileName { get; private set; }

        public override string ToString()
        {
            return $"{base.ToString()}, Chunk Size: {this.MaxLogFileSize}, Max chunk count: {this.MaxLogFiles}, Max log archive count: {this.MaxLogArchives}, Cleanup period: {this.MaxLogRetentionDays} days]";
        }

        /// <summary>
        /// Rotates the log file if it exceeds the maximum log file size and performs log file cleanup.
        /// </summary>
        /// <param name="filePath">The path of the log file to rotate.</param>
        public void Rotate(string filePath)
        {
            // Rotate log file if it exceeds the maximum file size
            if (!File.Exists(filePath))
            {
                return;
            }

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length < this.MaxLogFileSize)
            {
                return;
            }

            var fileTime = ConciergeDateTime.RotateLog;
            var rotatedPath = filePath.Replace(".log", $".{fileTime}.log");
            File.Move(filePath, rotatedPath);

            // Archive old log chunks if there are too many
            var folderPath = Path.GetDirectoryName(rotatedPath) ?? string.Empty;
            var logFolderContent = new DirectoryInfo(folderPath).GetFileSystemInfos();

            var chunks = logFolderContent.Where(x => !x.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase));
            if (chunks.Count() <= this.MaxLogFiles)
            {
                return;
            }

            var archiveFolderInfo = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(rotatedPath) ?? string.Empty, $"{Path.GetFileNameWithoutExtension(this.LogFileName)}.{fileTime}"));
            foreach (var chunk in chunks)
            {
                Directory.Move(chunk.FullName, Path.Combine(archiveFolderInfo.FullName, chunk.Name));
            }

            ZipFile.CreateFromDirectory(archiveFolderInfo.FullName, Path.Combine(folderPath, $"{Path.GetFileNameWithoutExtension(this.LogFileName)}_{fileTime}.zip"));
            Directory.Delete(archiveFolderInfo.FullName, true);

            // Cleanup old log archives if there are too many
            var archives = logFolderContent.Where(x => x.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase)).ToArray();
            if (archives.Length + 1 <= this.MaxLogArchives)
            {
                return;
            }

            var oldestArchive = archives.OrderBy(x => x.CreationTime).First();
            var cleanupDate = oldestArchive.CreationTime.AddDays(this.MaxLogRetentionDays);
            if (DateTime.Compare(cleanupDate, DateTime.Now) <= 0)
            {
                foreach (var file in logFolderContent)
                {
                    file.Delete();
                }
            }
            else
            {
                File.Delete(oldestArchive.FullName);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "Simple using is retarded.")]
        protected override void CreateLog(string message)
        {
            var logFilePath = Path.Combine(this.LogLocation, this.LogFileName);
            using (var streamWriter = File.AppendText(logFilePath))
            {
                streamWriter.WriteLine(message);
            }
        }

        private static string FormatLogFileName(string logName)
        {
            return logName.IsNullOrWhiteSpace() ?
                DefaultLogFileName :
                logName;
        }
    }
}
