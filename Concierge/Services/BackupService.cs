// <copyright file="BackupService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Threading;

    using Concierge.Common.Extensions;
    using Concierge.Configuration;

    public sealed class BackupService
    {
        public const int Interval = 10;

        private readonly string backupFolder;
        private readonly FileAccessService fileAccessService;
        private readonly DispatcherTimer dispatcherTimer;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("GeneratedRegex", "SYSLIB1045:Convert to 'GeneratedRegexAttribute'.", Justification = "<The way she goes.>")]
        private readonly Regex extractNumber = new (@"\d+", RegexOptions.Compiled);

        public BackupService(FileAccessService fileAccessService, string backupFolder)
        {
            this.backupFolder = backupFolder;
            this.fileAccessService = fileAccessService;
            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Tick += this.Save;

            Program.Logger.Info($"Initialized {nameof(BackupService)}.");
        }

        public void Start()
        {
            Program.Logger.Info($"Start backup timer.");

            this.dispatcherTimer.Interval = TimeSpan.FromMinutes(Interval);
            this.dispatcherTimer.Start();
        }

        public void Stop()
        {
            Program.Logger.Info($"Stop backup timer.");

            this.dispatcherTimer.Stop();
        }

        private static string GenerateSaveName(CcsFile ccsFile)
        {
            var name = ccsFile.Character.Properties.Name.Strip(" ");
            name = name.IsNullOrWhiteSpace() ? "Character" : name;

            return $"{name}_Backup";
        }

        private int GetIndex(CcsFile ccsFile)
        {
            var directoryInfo = new DirectoryInfo(this.backupFolder);
            var files = directoryInfo.GetFiles("*.ccs");

            var matchedFiles = files.Where(x => x.Name.Contains(GenerateSaveName(ccsFile))).ToList();
            var oldestFile = matchedFiles.OrderBy(y => y.CreationTime).FirstOrDefault();
            if (oldestFile is null)
            {
                return 1;
            }

            if (matchedFiles.Count < AppSettingsManager.StartUp.MaxBackups)
            {
                return matchedFiles.Count + 1;
            }

            if (int.TryParse(this.extractNumber.Match(oldestFile.Name).Value, out int index))
            {
                return index;
            }

            return 1;
        }

        private void Save(object? sender, EventArgs e)
        {
            if (AppSettingsManager.StartUp.MaxBackups == 0)
            {
                Program.Logger.Info($"Backups disabled.");
                return;
            }

            Program.Logger.Info($"Creating backup...");

            var ccsFile = Program.CcsFile;
            var fileName = GenerateSaveName(ccsFile);
            var path = Path.Combine(this.backupFolder, $"{fileName}{this.GetIndex(ccsFile)}.ccs");

            this.fileAccessService.Save(ccsFile, path);
        }
    }
}
