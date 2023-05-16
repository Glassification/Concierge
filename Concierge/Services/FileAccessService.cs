// <copyright file="FileAccessService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using Concierge.Common.Extensions;
    using Concierge.Configuration;
    using Concierge.Persistence;
    using Concierge.Persistence.Dialogs;
    using Concierge.Persistence.Enums;
    using Concierge.Persistence.ReadWriters;
    using Microsoft.Win32;

    public sealed class FileAccessService
    {
        private readonly OpenFileDialog openFileDialog;
        private readonly SaveFileDialog saveFileDialog;
        private readonly FolderPicker folderPickerDialog;

        public FileAccessService()
        {
            this.openFileDialog = new OpenFileDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.folderPickerDialog = new FolderPicker();
        }

        public CcsFile? OpenCcs(string file)
        {
            if (!file.IsNullOrWhiteSpace())
            {
                return CharacterReadWriter.Read(file);
            }

            if (AppSettingsManager.UserSettings.DefaultFolder.UseOpenFolder)
            {
                this.openFileDialog.InitialDirectory = AppSettingsManager.UserSettings.DefaultFolder.OpenFolder;
            }

            this.openFileDialog.Filter = FileConstants.CcsOpenFilter;
            this.openFileDialog.DefaultExt = "ccs";
            this.openFileDialog.FilterIndex = (int)CcsFiltersIndex.Ccs;

            return this.openFileDialog.ShowDialog() ?? false ? CharacterReadWriter.Read(this.openFileDialog.FileName) : null;
        }

        public string OpenFile(int filterIndex, string filter, string defaultExtension)
        {
            if (AppSettingsManager.UserSettings.DefaultFolder.UseOpenFolder)
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
                this.folderPickerDialog.InputPath = defaultPath;
            }

            return this.folderPickerDialog.ShowDialog() ?? false ? this.folderPickerDialog.ResultPath : string.Empty;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Keep consistent with Save As.")]
        public void Save(CcsFile ccsFile)
        {
            CharacterReadWriter.Write(ccsFile);
        }

        public bool SaveAs(CcsFile ccsFile)
        {
            if (AppSettingsManager.UserSettings.DefaultFolder.UseSaveFolder)
            {
                this.saveFileDialog.InitialDirectory = AppSettingsManager.UserSettings.DefaultFolder.SaveFolder;
            }

            this.saveFileDialog.FileName = FileConstants.DefaultFileName;
            this.saveFileDialog.Filter = FileConstants.SaveFilter;
            this.saveFileDialog.DefaultExt = "ccs";
            this.saveFileDialog.FilterIndex = (int)CcsFiltersIndex.Ccs;

            if (this.saveFileDialog.ShowDialog() ?? false)
            {
                ccsFile.AbsolutePath = this.saveFileDialog.FileName;
                CharacterReadWriter.Write(ccsFile);
                return true;
            }

            return false;
        }
    }
}
