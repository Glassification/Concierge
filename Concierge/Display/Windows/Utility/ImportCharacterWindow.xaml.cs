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
    using Concierge.Import;
    using Concierge.Import.Enums;
    using Concierge.Import.Importers;
    using Concierge.Persistence;
    using Concierge.Persistence.Enums;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for ImportCharacterWindow.xaml.
    /// </summary>
    public partial class ImportCharacterWindow : ConciergeWindow
    {
        private readonly FileAccessService fileAccessService = new ();
        private readonly CharacterImporter characterImporter = new ();

        private bool imported;

        public ImportCharacterWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
        }

        public override string HeaderText => "Import Details";

        public override string WindowName => nameof(ImportCharacterWindow);

        public override object? ShowWindow()
        {
            this.ShowConciergeWindow();

            return this.imported;
        }

        private void UncheckAllExcept(string name)
        {
            this.AbilitiesCheckBox.UncheckUnlessNameMatches(name);
            this.AugmentCheckBox.UncheckUnlessNameMatches(name);
            this.InventoryCheckBox.UncheckUnlessNameMatches(name);
            this.NotesCheckBox.UncheckUnlessNameMatches(name);
            this.LanguageCheckBox.UncheckUnlessNameMatches(name);
            this.ProficiencyCheckBox.UncheckUnlessNameMatches(name);
            this.ResourcesCheckBox.UncheckUnlessNameMatches(name);
            this.SpellsCheckBox.UncheckUnlessNameMatches(name);
            this.StatusEffectsCheckBox.UncheckUnlessNameMatches(name);
            this.WeaponsCheckBox.UncheckUnlessNameMatches(name);
        }

        private void Clear()
        {
            this.FileSourceTextBox.Text = string.Empty;
            this.UncheckAllExcept(string.Empty);
        }

        private void LoadImporters()
        {
            if (this.AbilitiesCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new AbilityImporter(Program.CcsFile.Character));
            }

            if (this.AugmentCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new AugmentationImporter(Program.CcsFile.Character));
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

            if (this.ResourcesCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new ResourceImporter(Program.CcsFile.Character));
            }

            if (this.SpellsCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new SpellImporter(Program.CcsFile.Character));
            }

            if (this.StatusEffectsCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new StatusEffectImporter(Program.CcsFile.Character));
            }

            if (this.WeaponsCheckBox.IsChecked ?? false)
            {
                this.characterImporter.AddImporter(new WeaponImporter(Program.CcsFile.Character));
            }
        }

        private void AttemptAutoSelect(string file)
        {
            if (this.ImportCharacterButton.IsChecked ?? false)
            {
                return;
            }

            this.AbilitiesCheckBox.IsChecked = AutoSelection.FuzzySearch(file, SelectionType.Ability);
            this.AugmentCheckBox.IsChecked = AutoSelection.FuzzySearch(file, SelectionType.Augmentation);
            this.InventoryCheckBox.IsChecked = AutoSelection.FuzzySearch(file, SelectionType.Inventory);
            this.NotesCheckBox.IsChecked = AutoSelection.FuzzySearch(file, SelectionType.Journal);
            this.LanguageCheckBox.IsChecked = AutoSelection.FuzzySearch(file, SelectionType.Language);
            this.ProficiencyCheckBox.IsChecked = AutoSelection.FuzzySearch(file, SelectionType.Proficiency);
            this.ResourcesCheckBox.IsChecked = AutoSelection.FuzzySearch(file, SelectionType.Resource);
            this.SpellsCheckBox.IsChecked = AutoSelection.FuzzySearch(file, SelectionType.Spell);
            this.StatusEffectsCheckBox.IsChecked = AutoSelection.FuzzySearch(file, SelectionType.StatusEffect);
            this.WeaponsCheckBox.IsChecked = AutoSelection.FuzzySearch(file, SelectionType.Weapon);
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var filterIndex = this.ImportCharacterButton.IsChecked ?? false ? CcsFiltersIndex.Ccs : CcsFiltersIndex.Json;
            var file = this.fileAccessService.OpenFile(
                (int)filterIndex,
                FileConstants.CcsOpenFilter,
                filterIndex.ToString().ToLower(),
                string.Empty);

            if (!file.IsNullOrWhiteSpace())
            {
                this.FileSourceTextBox.Text = file;
                this.AttemptAutoSelect(file);
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
                this.ImportCharacterButton.IsChecked ?? false ? ImportTypes.Character : ImportTypes.Single,
                this.FileSourceTextBox.Text);

            if (!isSuccess)
            {
                ConciergeMessageBox.ShowError("Concierge was unable to find any data to import.");
                return;
            }

            this.imported = true;
            Program.UndoRedoService.AddCommand(new EditCommand<CharacterSheet>(Program.CcsFile.Character, oldItem, this.ConciergePage));
            if (button.Name.Contains("Close"))
            {
                this.ReturnAndClose();
            }
            else
            {
                this.Clear();
                this.InvokeApplyChanges();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;

            this.CloseConciergeWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;

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
