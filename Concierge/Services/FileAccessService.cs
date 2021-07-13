// <copyright file="FileAccessService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using Concierge.Persistence;
    using Microsoft.Win32;

    public class FileAccessService
    {
        private const string DefaultOpenExtension = "ccs";
        private const string DefaultSaveExtension = "ccs";
        private const string OpenFilter = "CCS files (*.ccs)|*.ccs|JSON files (*.json)|*.json|All files (*.*)|*.*";
        private const string SaveFilter = "CCS files (*.ccs)|*.ccs";
        private const string DefaultFileName = "New Character.ccs";

        private readonly OpenFileDialog openFileDialog;
        private readonly SaveFileDialog saveFileDialog;

        public FileAccessService()
        {
            this.openFileDialog = new OpenFileDialog()
            {
                DefaultExt = DefaultOpenExtension,
                Filter = OpenFilter,
            };

            this.saveFileDialog = new SaveFileDialog()
            {
                DefaultExt = DefaultSaveExtension,
                Filter = SaveFilter,
                FileName = DefaultFileName,
            };
        }

        public CcsFile Open()
        {
            if (this.openFileDialog.ShowDialog() ?? false)
            {
                return CharacterLoader.LoadCharacterSheetJson(this.openFileDialog.FileName);
            }
            else
            {
                return null;
            }
        }

        public void Save(CcsFile ccsFile)
        {
            if (this.saveFileDialog.ShowDialog() ?? false)
            {
                ccsFile.AbsolutePath = this.saveFileDialog.FileName;

                CharacterSaver.SaveCharacterSheetJson(ccsFile);
            }
        }
    }
}
