// <copyright file="LocalLogger.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Logging
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;

    using Concierge.Utility.Extensions;

    public class LocalLogger : Logger
    {
        private const string DefaultLoggingFolder = "Logging";

        public LocalLogger()
        : base()
        {
            this.LogLocation = this.FormatLogFilePath(string.Empty);
            this.LogFileName = this.FormatLogFileName();

            this.MaxLogFileSize = 5000000;
            this.MaxLogFiles = 5;
            this.MaxLogArchives = 5;
            this.MaxLogRetentionDays = 90;
        }

        public LocalLogger(string logPath)
        : base()
        {
            this.LogLocation = this.FormatLogFilePath(logPath);
            this.LogFileName = this.FormatLogFileName();

            this.MaxLogFileSize = 5000000;
            this.MaxLogFiles = 5;
            this.MaxLogArchives = 5;
            this.MaxLogRetentionDays = 90;
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

        protected override void CreateLog(string message)
        {
            if (!Directory.Exists(this.LogLocation))
            {
                Directory.CreateDirectory(this.LogLocation);
            }

            var logFilePath = Path.Combine(this.LogLocation, this.LogFileName);
            this.Rotate(logFilePath);

            using (var streamWriter = File.AppendText(logFilePath))
            {
                streamWriter.WriteLine(message);
            }
        }

        private string FormatLogFileName()
        {
            return $"Concierge.log";
        }

        private string FormatLogFilePath(string filePath)
        {
            return filePath.IsNullOrWhiteSpace() ?
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), DefaultLoggingFolder) :
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

            var fileTime = DateTime.Now.ToString("dd_MM_yy_h_m_s");
            var rotatedPath = filePath.Replace(".log", $".{fileTime}");
            File.Move(filePath, rotatedPath);

            var folderPath = Path.GetDirectoryName(rotatedPath);
            var logFolderContent = new DirectoryInfo(folderPath).GetFileSystemInfos();

            var chunks = logFolderContent.Where(x => !x.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase));
            if (chunks.Count() <= this.MaxLogFiles)
            {
                return;
            }

            var archiveFolderInfo = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(rotatedPath), $"{Path.GetFileNameWithoutExtension(this.LogFileName)}_{fileTime}"));
            foreach (var chunk in chunks)
            {
                Directory.Move(chunk.FullName, Path.Combine(archiveFolderInfo.FullName, chunk.Name));
            }

            ZipFile.CreateFromDirectory(archiveFolderInfo.FullName, Path.Combine(folderPath, $"{Path.GetFileNameWithoutExtension(this.LogFileName)}_{fileTime}.zip"));
            Directory.Delete(archiveFolderInfo.FullName, true);

            var archives = logFolderContent.Where(x => x.Extension.Equals(".zip", StringComparison.OrdinalIgnoreCase)).ToArray();
            if (archives.Length <= this.MaxLogArchives)
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
