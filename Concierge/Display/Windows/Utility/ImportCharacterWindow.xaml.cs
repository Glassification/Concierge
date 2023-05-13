// <copyright file="ImportCharacterWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System.Windows;
    using Concierge.Character;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Services;
    using Concierge.Services.Enums;
    using Concierge.Services.ImportService;

    /// <summary>
    /// Interaction logic for ImportCharacterWindow.xaml.
    /// </summary>
    public partial class ImportCharacterWindow : ConciergeWindow
    {
        private readonly FileAccessService fileAccessService;
        private readonly CharacterImportService characterImporter;

        public ImportCharacterWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.fileAccessService = new FileAccessService();
            this.characterImporter = new CharacterImportService();
        }

        public override string HeaderText => "Import Details";

        public override string WindowName => nameof(ImportCharacterWindow);

        private bool Imported { get; set; }

        public override object? ShowWindow()
        {
            this.ShowConciergeWindow();

            return this.Imported;
        }

        private void UncheckAllExcept(string name)
        {
            this.AbilitiesCheckBox.UncheckUnlessNameMatches(name);
            this.AmmoCheckBox.UncheckUnlessNameMatches(name);
            this.InventoryCheckBox.UncheckUnlessNameMatches(name);
            this.NotesCheckBox.UncheckUnlessNameMatches(name);
            this.LanguageCheckBox.UncheckUnlessNameMatches(name);
            this.ProficiencyCheckBox.UncheckUnlessNameMatches(name);
            this.SpellsCheckBox.UncheckUnlessNameMatches(name);
            this.WeaponsCheckBox.UncheckUnlessNameMatches(name);
        }

        private void LoadImporters()
        {
            if (this.AbilitiesCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new AbilityImporter(Program.CcsFile.Character));
            }

            if (this.AmmoCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new AmmunitionImporter(Program.CcsFile.Character));
            }

            if (this.InventoryCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new InventoryImporter(Program.CcsFile.Character));
            }

            if (this.NotesCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new JournalImporter(Program.CcsFile.Character));
            }

            if (this.LanguageCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new LanguageImporter(Program.CcsFile.Character));
            }

            if (this.ProficiencyCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new ProficiencyImporter(Program.CcsFile.Character));
            }

            if (this.SpellsCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new SpellImporter(Program.CcsFile.Character));
            }

            if (this.WeaponsCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new WeaponImporter(Program.CcsFile.Character));
            }
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
            if (sender is not ConciergeTextButton button)
            {
                return;
            }

            if (this.FileSourceTextBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            var oldItem = Program.CcsFile.Character.DeepCopy();
            this.LoadImporters();
            var isSuccess = this.characterImporter.Import(
                this.ImportCharacterButton.IsChecked ?? false ? ImportTypes.Character : ImportTypes.Object,
                this.FileSourceTextBox.Text);

            if (!isSuccess)
            {
                ConciergeMessageBox.Show(
                    "Concierge was unable to find any data to import.",
                    "Error",
                    ConciergeWindowButtons.Ok,
                    ConciergeWindowIcons.Error);
                return;
            }

            Program.Modify();
            this.Imported = true;
            Program.UndoRedoService.AddCommand(new EditCommand<ConciergeCharacter>(Program.CcsFile.Character, oldItem, this.ConciergePage));

            if (button.Name.Contains("Close"))
            {
                this.ReturnAndClose();
            }
            else
            {
                this.InvokeApplyChanges();
            }
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

        private void ImportObjectButton_Checked(object sender, RoutedEventArgs e)
        {
            this.UncheckAllExcept(string.Empty);
        }

        private void ImportCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is not ConciergeCheckBox checkBox)
            {
                return;
            }

            if (this.ImportObjectButton.IsChecked ?? false)
            {
                this.UncheckAllExcept(checkBox.Name);
            }
        }
    }
}
