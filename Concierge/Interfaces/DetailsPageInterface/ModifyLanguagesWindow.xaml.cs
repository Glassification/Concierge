// <copyright file="ModifyLanguagesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Characteristics;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyLanguagesWindow.xaml.
    /// </summary>
    public partial class ModifyLanguagesWindow : Window, IConciergeModifyWindow
    {
        public ModifyLanguagesWindow()
        {
            this.InitializeComponent();
            this.NameComboBox.ItemsSource = Constants.Languages;
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private bool Editing { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Language";

        private Language SelectedLanguage { get; set; }

        private List<Language> Languages { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Languages = Program.CcsFile.Character.Details.Languages;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;

            this.ClearFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowAdd(List<Language> languages)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Languages = languages;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;

            this.ClearFields();
            this.ShowDialog();
        }

        public void ShowEdit(Language language)
        {
            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedLanguage = language;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;

            this.FillFields(language);
            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void FillFields(Language language)
        {
            this.NameComboBox.Text = language.Name;
            this.ScriptTextBox.Text = language.Script;
            this.SpeakersTextBox.Text = language.Speakers;
        }

        private void ClearFields()
        {
            this.NameComboBox.Text = string.Empty;
            this.ScriptTextBox.Text = string.Empty;
            this.SpeakersTextBox.Text = string.Empty;
        }

        private void UpdateLanguage(Language language)
        {
            language.Name = this.NameComboBox.Text;
            language.Script = this.ScriptTextBox.Text;
            language.Speakers = this.SpeakersTextBox.Text;
        }

        private Language ToLanguage()
        {
            return new Language()
            {
                Name = this.NameComboBox.Text,
                Script = this.ScriptTextBox.Text,
                Speakers = this.SpeakersTextBox.Text,
            };
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = ConciergeWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateLanguage(this.SelectedLanguage);
            }
            else
            {
                this.Languages.Add(this.ToLanguage());
            }

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.Languages.Add(this.ToLanguage());
            this.ClearFields();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NameComboBox.SelectedItem != null)
            {
                this.FillFields(this.NameComboBox.SelectedItem as Language);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
