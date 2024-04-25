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

    /// <summary>
    /// Provides methods for handling file access operations, such as opening, saving, and browsing files and folders.
    /// </summary>
    public sealed class FileAccessService
    {
        private readonly CharacterReadWriter readwriter;

        private readonly OpenFileDialog openFileDialog;
        private readonly SaveFileDialog saveFileDialog;
        private readonly OpenFolderDialog openFolderDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAccessService"/> class.
        /// </summary>
        public FileAccessService()
        {
            this.readwriter = new CharacterReadWriter(Program.ErrorService, Program.Logger);

            this.openFileDialog = new OpenFileDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.openFolderDialog = new OpenFolderDialog();
        }

        /// <summary>
        /// Opens a Concierge Character Sheet (CCS) file and returns its content.
        /// </summary>
        /// <param name="file">The path of the CCS file to open.</param>
        /// <returns>The <see cref="CcsFile"/> object representing the opened CCS file, or null if the operation fails.</returns>
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

        /// <summary>
        /// Opens a file dialog window for selecting a file to open.
        /// </summary>
        /// <param name="filterIndex">The index of the file dialog filter.</param>
        /// <param name="filter">The file dialog filter string.</param>
        /// <param name="defaultExtension">The default extension for the file dialog.</param>
        /// <returns>The path of the selected file, or an empty string if no file is selected.</returns>
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

        /// <summary>
        /// Opens a folder dialog window for selecting a folder.
        /// </summary>
        /// <param name="defaultPath">The default path for the folder dialog.</param>
        /// <returns>The path of the selected folder, or an empty string if no folder is selected.</returns>
        public string OpenFolder(string defaultPath = "")
        {
            if (!defaultPath.IsNullOrWhiteSpace())
            {
                this.openFolderDialog.DefaultDirectory = defaultPath;
            }

            return this.openFolderDialog.ShowDialog() ?? false ? this.openFolderDialog.FolderName : string.Empty;
        }

        /// <summary>
        /// Saves a Concierge Character Sheet (CCS) file.
        /// </summary>
        /// <param name="ccsFile">The CCS file to save.</param>
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

        /// <summary>
        /// Saves a Concierge Character Sheet (CCS) file with the specified path.
        /// </summary>
        /// <param name="ccsFile">The CCS file to save.</param>
        /// <param name="path">The path where the file will be saved.</param>
        public void Save(CcsFile ccsFile, string path)
        {
            ccsFile.Version = Program.AssemblyVersion;
            ccsFile.LastSaveDate = DateTime.Now;
            ccsFile.Hash = ConciergeHashing.HashData(ccsFile.Character);

            this.readwriter.WriteJson(path, ccsFile);
        }

        /// <summary>
        /// Saves a Concierge Character Sheet (CCS) file with a new file name or path.
        /// </summary>
        /// <param name="ccsFile">The CCS file to save.</param>
        /// <returns>True if the file is saved successfully; otherwise, false.</returns>
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
