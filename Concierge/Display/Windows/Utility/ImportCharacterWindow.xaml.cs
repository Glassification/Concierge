// <copyright file="ImportCharacterWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System.Windows;

    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Services;
    using Concierge.Tools.Display;
    using Concierge.Tools.Import;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for ImportCharacterWindow.xaml.
    /// </summary>
    public partial class ImportCharacterWindow : ConciergeWindow
    {
        private readonly FileAccessService fileAccessService;
        private readonly CharacterImporter characterImporter;

        public ImportCharacterWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.fileAccessService = new FileAccessService();
            this.characterImporter = new CharacterImporter(Program.CcsFile.Character);
        }

        public override string HeaderText => "Import Details";

        private bool Imported { get; set; }

        public override object? ShowWindow()
        {
            this.ShowConciergeWindow();

            return this.Imported;
        }

        private ImportSettings GenerateImportSettings()
        {
            return new ImportSettings()
            {
                File = this.FileSourceTextBox.Text,
                ImportAbilities = this.AbilitiesCheckBox.IsChecked ?? false,
                ImportAmmo = this.AmmoCheckBox.IsChecked ?? false,
                ImportInventory = this.InventoryCheckBox.IsChecked ?? false,
                ImportNotes = this.NotesCheckBox.IsChecked ?? false,
                ImportSpells = this.SpellsCheckBox.IsChecked ?? false,
                ImportWeapons = this.WeaponsCheckBox.IsChecked ?? false,
            };
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var file = this.fileAccessService.OpenCcsName();

            if (!file.IsNullOrWhiteSpace())
            {
                this.FileSourceTextBox.Text = file;
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.FileSourceTextBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            var result = ConciergeMessageBox.Show(
                "Importing values from a character sheet cannot be undone.\n\nDo you wish to continue?",
                "Warning",
                ConciergeWindowButtons.YesNo,
                ConciergeWindowIcons.Warning);

            if (result != ConciergeWindowResult.Yes)
            {
                return;
            }

            var settings = this.GenerateImportSettings();
            var isSuccess = this.characterImporter.Import(settings);
            if (!isSuccess)
            {
                return;
            }

            this.Imported = true;
            this.ReturnAndClose();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;

            this.CloseConciergeWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;

            this.CloseConciergeWindow();
        }
    }
}
