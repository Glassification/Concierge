// <copyright file="LanguagesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Characteristics;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for LanguagesWindow.xaml.
    /// </summary>
    public partial class LanguagesWindow : ConciergeWindow
    {
        public LanguagesWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.NameComboBox.ItemsSource = Constants.Languages;
            this.ConciergePage = ConciergePage.None;
            this.SelectedLanguage = new Language();
            this.Languages = new List<Language>();
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Language";

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private Language SelectedLanguage { get; set; }

        private List<Language> Languages { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Languages = Program.CcsFile.Character.Languages;
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

            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Languages = castItem;
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

            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedLanguage = castItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateLanguage(this.SelectedLanguage);
            }
            else
            {
                this.Languages.Add(this.ToLanguage());
            }

            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void FillFields(Language language)
        {
            this.NameComboBox.Text = language.Name;
            this.ScriptTextBox.Text = language.Script;
            this.SpeakersTextBox.Text = language.Speakers;
        }

        private void ClearFields(string name = "")
        {
            this.NameComboBox.Text = name;
            this.ScriptTextBox.Text = string.Empty;
            this.SpeakersTextBox.Text = string.Empty;
        }

        private void UpdateLanguage(Language language)
        {
            var oldItem = language.DeepCopy();

            language.Name = this.NameComboBox.Text;
            language.Script = this.ScriptTextBox.Text;
            language.Speakers = this.SpeakersTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Language>(language, oldItem, this.ConciergePage));
        }

        private Language ToLanguage()
        {
            this.ItemsAdded = true;

            var language = new Language()
            {
                Name = this.NameComboBox.Text,
                Script = this.ScriptTextBox.Text,
                Speakers = this.SpeakersTextBox.Text,
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Language>(this.Languages, language, this.ConciergePage));

            return language;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Languages.Add(this.ToLanguage());
            this.ClearFields();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NameComboBox.SelectedItem is Language language)
            {
                this.FillFields(language);
            }
            else
            {
                this.ClearFields(this.NameComboBox.Text);
            }
        }
    }
}
