// <copyright file="LocalLogger.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Logging
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    using Concierge.Persistence;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public sealed class LocalLogger : Logger
    {
        public const string DefaultLogFileName = @"Concierge.log";

        public LocalLogger(bool isDebug = false)
        : this(string.Empty, string.Empty, isDebug)
        {
        }

        public LocalLogger(string logPath, string logName, bool isDebug = false)
        : base(isDebug)
        {
            this.LogLocation = FormatLogFilePath(logPath);
            this.LogFileName = FormatLogFileName(logName);

            this.MaxLogFileSize = 5000000;
            this.MaxLogFiles = 5;
            this.MaxLogArchives = 5;
            this.MaxLogRetentionDays = 90;

            Directory.CreateDirectory(this.LogLocation);
            this.Rotate(Path.Combine(this.LogLocation, this.LogFileName));
        }

        public int MaxLogFileSize { get; set; }

        public int MaxLogFiles { get; set; }

        public int MaxLogArchives { get; set; }

        public int MaxLogRetentionDays { get; set; }

        public string LogLocation { get; private set; }

        public string LogFileName { get; private set; }

        public override string ToString()
        {
            return $"{base.ToString()}, Chunk Size: {this.MaxLogFileSize}, Max chunk count: {this.MaxLogFiles}, Max log archive count: {this.MaxLogArchives}, Cleanup period: {this.MaxLogRetentionDays} days]";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "Improve readability.")]
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

        private static string FormatLogFilePath(string filePath)
        {
            return filePath.IsNullOrWhiteSpace() ?
                ConciergeFiles.LoggingDirectory :
                filePath;
        }

        private void Rotate(string filePath)
        {
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
    }
}
