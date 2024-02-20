// <copyright file="FileAccessService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;
    using System.IO;

    using Concierge.Common.Extensions;
    using Concierge.Configuration;
    using Concierge.Persistence;
    using Concierge.Persistence.Enums;
    using Concierge.Persistence.ReadWriters;
    using Microsoft.Win32;

    public sealed class FileAccessService
    {
        private readonly CharacterReadWriter readwriter;

        private readonly OpenFileDialog openFileDialog;
        private readonly SaveFileDialog saveFileDialog;
        private readonly OpenFolderDialog openFolderDialog;

        public FileAccessService()
        {
            this.readwriter = new CharacterReadWriter(Program.ErrorService, Program.Logger);

            this.openFileDialog = new OpenFileDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.openFolderDialog = new OpenFolderDialog();
        }

        public CcsFile? OpenCcs(string file)
        {
            if (!file.IsNullOrWhiteSpace())
            {
                var ccsFile2 = this.readwriter.ReadJson<CcsFile>(file);
                ccsFile2.AbsolutePath = file;
                ccsFile2.Initialize();

                return !ccsFile2.CheckHash() || (AppSettingsManager.UserSettings.CheckVersion && !ccsFile2.CheckVersion()) ? new CcsFile() : ccsFile2;
            }

            if (ShouldUseDefaultOpen())
            {
                this.openFileDialog.InitialDirectory = AppSettingsManager.UserSettings.DefaultFolder.OpenFolder;
            }

            this.openFileDialog.Filter = FileConstants.CcsOpenFilter;
            this.openFileDialog.DefaultExt = "ccs";
            this.openFileDialog.FilterIndex = (int)CcsFiltersIndex.Ccs;

            var ccsFile = this.openFileDialog.ShowDialog() ?? false ? this.readwriter.ReadJson<CcsFile>(this.openFileDialog.FileName) : null;
            if (ccsFile is null)
            {
                return null;
            }

            ccsFile.AbsolutePath = this.openFileDialog.FileName;
            ccsFile.Initialize();

            return !ccsFile.CheckHash() || (AppSettingsManager.UserSettings.CheckVersion && !ccsFile.CheckVersion()) ? new CcsFile() : ccsFile;
        }

        public string OpenFile(int filterIndex, string filter, string defaultExtension)
        {
            if (ShouldUseDefaultOpen())
            {
                this.openFileDialog.InitialDirectory = AppSettingsManager.UserSettings.DefaultFolder.OpenFolder;
            }

            this.openFileDialog.Filter = filter;
            this.openFileDialog.DefaultExt = defaultExtension;
            this.openFileDialog.FilterIndex = filterIndex;

            return this.openFileDialog.ShowDialog() ?? false ? this.openFileDialog.FileName : string.Empty;
        }

        public string OpenFolder(string defaultPath = "")
        {
            if (!defaultPath.IsNullOrWhiteSpace())
            {
                this.openFolderDialog.DefaultDirectory = defaultPath;
            }

            return this.openFolderDialog.ShowDialog() ?? false ? this.openFolderDialog.FolderName : string.Empty;
        }

        public void Save(CcsFile ccsFile)
        {
            ccsFile.Version = Program.AssemblyVersion;
            ccsFile.LastSaveDate = DateTime.Now;
            ccsFile.Hash = ConciergeHashing.HashData(ccsFile.Character);

            var result = this.readwriter.WriteJson(ccsFile.AbsolutePath, ccsFile);
            if (result)
            {
                Program.Unmodify();
            }
            else
            {
                Program.Modify();
            }
        }

        public void Save(CcsFile ccsFile, string path)
        {
            ccsFile.Version = Program.AssemblyVersion;
            ccsFile.LastSaveDate = DateTime.Now;
            ccsFile.Hash = ConciergeHashing.HashData(ccsFile.Character);

            this.readwriter.WriteJson(path, ccsFile);
        }

        public bool SaveAs(CcsFile ccsFile)
        {
            if (ShouldUseDefaultSave())
            {
                this.saveFileDialog.InitialDirectory = AppSettingsManager.UserSettings.DefaultFolder.SaveFolder;
            }

            var name = ccsFile.Character.Disposition.Name;
            this.saveFileDialog.FileName = name.IsNullOrWhiteSpace() ? FileConstants.DefaultFileName : name;
            this.saveFileDialog.Filter = FileConstants.SaveFilter;
            this.saveFileDialog.DefaultExt = "ccs";
            this.saveFileDialog.FilterIndex = (int)CcsFiltersIndex.Ccs;

            if (this.saveFileDialog.ShowDialog() ?? false)
            {
                ccsFile.Version = Program.AssemblyVersion;
                ccsFile.LastSaveDate = DateTime.Now;
                ccsFile.Hash = ConciergeHashing.HashData(ccsFile.Character);
                ccsFile.AbsolutePath = this.saveFileDialog.FileName;

                var result = this.readwriter.WriteJson(ccsFile.AbsolutePath, ccsFile);
                if (result)
                {
                    Program.Unmodify();
                }
                else
                {
                    Program.Modify();
                }

                return result;
            }

            return false;
        }

        private static bool ShouldUseDefaultOpen()
        {
            return
                AppSettingsManager.UserSettings.DefaultFolder.UseOpenFolder &&
                !AppSettingsManager.UserSettings.DefaultFolder.OpenFolder.IsNullOrWhiteSpace() &&
                Directory.Exists(AppSettingsManager.UserSettings.DefaultFolder.OpenFolder);
        }

        private static bool ShouldUseDefaultSave()
        {
            return
                AppSettingsManager.UserSettings.DefaultFolder.UseSaveFolder &&
                !AppSettingsManager.UserSettings.DefaultFolder.SaveFolder.IsNullOrWhiteSpace() &&
                Directory.Exists(AppSettingsManager.UserSettings.DefaultFolder.SaveFolder);
        }
    }
}
