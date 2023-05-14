// <copyright file="FileAccessService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using Concierge.Common.Extensions;
    using Concierge.Persistence;
    using Concierge.Persistence.Enums;
    using Concierge.Persistence.ReadWriters;
    using Microsoft.Win32;

    public sealed class FileAccessService
    {
        private readonly OpenFileDialog openFileDialog;
        private readonly SaveFileDialog saveFileDialog;

        public FileAccessService()
        {
            this.openFileDialog = new OpenFileDialog();
            this.saveFileDialog = new SaveFileDialog();
        }

        public CcsFile? OpenCcs(string file)
        {
            if (!file.IsNullOrWhiteSpace())
            {
                return CharacterReadWriter.Read(file);
            }

            this.openFileDialog.Filter = FileConstants.CcsOpenFilter;
            this.openFileDialog.DefaultExt = "ccs";
            this.openFileDialog.FilterIndex = (int)CcsFiltersIndex.Ccs;

            return this.openFileDialog.ShowDialog() ?? false ? CharacterReadWriter.Read(this.openFileDialog.FileName) : null;
        }

        public string Open(int filterIndex, string filter, string defaultExtension)
        {
            this.openFileDialog.Filter = filter;
            this.openFileDialog.DefaultExt = defaultExtension;
            this.openFileDialog.FilterIndex = filterIndex;

            return this.openFileDialog.ShowDialog() ?? false ? this.openFileDialog.FileName : string.Empty;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Keep consistent with Save As.")]
        public void Save(CcsFile ccsFile)
        {
            CharacterReadWriter.Write(ccsFile);
        }

        public bool SaveAs(CcsFile ccsFile)
        {
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
