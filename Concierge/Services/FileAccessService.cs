// <copyright file="FileAccessService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using Concierge.Persistence;
    using Concierge.Persistence.ReadWriters;
    using Microsoft.Win32;

    public sealed class FileAccessService
    {
        private const string DefaultCcsOpenExtension = "ccs";
        private const string DefaultImageOpenExtension = "jpg";
        private const string DefaultSaveExtension = "ccs";
        private const string CcsOpenFilter = "*CCS (*.ccs)|*.ccs|JSON (*.json)|*.json|All files (*.*)|*.*";
        private const string ImageOpenFilter = "*BMP (*.bmp)|*.bmp|JPEG (*.jpeg;*.jpg)|*.jpeg;*.jpg|PNG (*.png)|*.png|TIFF (*.tiff)|*.tiff|All files (*.*)|*.*";
        private const string SaveFilter = "CCS (*.ccs)|*.ccs";
        private const string DefaultFileName = "New Character.ccs";

        private readonly OpenFileDialog ccsOpenFileDialog;
        private readonly OpenFileDialog imageOpenFileDialog;
        private readonly SaveFileDialog saveFileDialog;

        public FileAccessService()
        {
            this.ccsOpenFileDialog = new OpenFileDialog()
            {
                DefaultExt = DefaultCcsOpenExtension,
                Filter = CcsOpenFilter,
            };

            this.imageOpenFileDialog = new OpenFileDialog()
            {
                DefaultExt = DefaultImageOpenExtension,
                Filter = ImageOpenFilter,
                FilterIndex = 2,
            };

            this.saveFileDialog = new SaveFileDialog()
            {
                DefaultExt = DefaultSaveExtension,
                Filter = SaveFilter,
                FileName = DefaultFileName,
            };
        }

        public CcsFile? OpenCcs()
        {
            return this.ccsOpenFileDialog.ShowDialog() ?? false ? CharacterReadWriter.Read(this.ccsOpenFileDialog.FileName) : null;
        }

        public string OpenImage()
        {
            return this.imageOpenFileDialog.ShowDialog() ?? false ? this.imageOpenFileDialog.FileName : string.Empty;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Keep consistent with Save As.")]
        public void Save(CcsFile ccsFile)
        {
            CharacterReadWriter.Write(ccsFile);
        }

        public bool SaveAs(CcsFile ccsFile)
        {
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
