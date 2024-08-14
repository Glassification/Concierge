// <copyright file="LanguagesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Details;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for LanguagesWindow.xaml.
    /// </summary>
    public partial class LanguagesWindow : ConciergeWindow
    {
        private bool editing;
        private Language selectedLanguage = new ();
        private List<Language> languages = [];

        public LanguagesWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.NameComboBox.ItemsSource = DefaultItems;
            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.NameComboBox);
            this.SetMouseOverEvents(this.ScriptTextBox, this.ScriptTextBackground);
            this.SetMouseOverEvents(this.SpeakersTextBox, this.SpeakersTextBackground);
        }

        public override string HeaderText => $"{(this.editing ? "Edit" : "Add")} Language";

        public override string WindowName => nameof(LanguagesWindow);

        public bool ItemsAdded { get; private set; }

        private static List<ComboBoxItemControl> DefaultItems => ComboBoxGenerator.SelectorComboBox(Defaults.Languages, Program.CustomItemService.GetItems<Language>());

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.languages = Program.CcsFile.Character.Detail.Languages;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T languages)
        {
            if (languages is not List<Language> castItem)
            {
                return false;
            }

            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.languages = castItem;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        public override void ShowEdit<T>(T language)
        {
            if (language is not Language castItem)
            {
                return;
            }

            this.editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.selectedLanguage = castItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            if (this.editing)
            {
                this.UpdateLanguage(this.selectedLanguage);
            }
            else
            {
                this.languages.Add(this.ToLanguage());
            }

            this.CloseConciergeWindow();
        }

        private void FillFields(Language language)
        {
            Program.Drawing();

            this.NameComboBox.Text = language.Name;
            this.ScriptTextBox.Text = language.Script;
            this.SpeakersTextBox.Text = language.Speakers;

            Program.NotDrawing();
        }

        private void ClearFields(string name = "")
        {
            Program.Drawing();

            this.NameComboBox.Text = name;
            this.ScriptTextBox.Text = string.Empty;
            this.SpeakersTextBox.Text = string.Empty;

            Program.NotDrawing();
        }

        private void UpdateLanguage(Language language)
        {
            var oldItem = language.DeepCopy();

            language.Name = this.NameComboBox.Text;
            language.Script = this.ScriptTextBox.Text;
            language.Speakers = this.SpeakersTextBox.Text;

            if (!language.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<Language>(language, oldItem, this.ConciergePage));
            }
        }

        private Language Create()
        {
            return new Language()
            {
                Name = this.NameComboBox.Text,
                Script = this.ScriptTextBox.Text,
                Speakers = this.SpeakersTextBox.Text,
            };
        }

        private Language ToLanguage()
        {
            this.ItemsAdded = true;
            var language = this.Create();

            Program.UndoRedoService.AddCommand(new AddCommand<Language>(this.languages, language, this.ConciergePage));

            return language;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.languages.Add(this.ToLanguage());
            this.ClearFields();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isLocked = this.LockButton.IsChecked ?? false;
            if (this.NameComboBox.SelectedItem is ComboBoxItemControl item && item.Item is Language language && !isLocked)
            {
                this.FillFields(language);
            }
            else if (!isLocked)
            {
                this.ClearFields(this.NameComboBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.ShowWarning("Could not save the Language.\nA name is required before saving a custom item.");
                return;
            }

            Program.CustomItemService.AddItem(this.Create());
            this.ClearFields();
            this.NameComboBox.ItemsSource = DefaultItems;
        }

        private void LockButton_Checked(object sender, RoutedEventArgs e)
        {
            this.LockIcon.Kind = PackIconKind.Lock;
        }

        private void LockButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.LockIcon.Kind = PackIconKind.LockOpenVariant;
        }
    }
}
