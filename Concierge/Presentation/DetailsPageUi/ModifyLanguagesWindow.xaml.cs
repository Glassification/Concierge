// <copyright file="ModifyLanguagesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.DetailsPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Collections;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyLanguagesWindow.xaml.
    /// </summary>
    public partial class ModifyLanguagesWindow : Window
    {
        public ModifyLanguagesWindow()
        {
            this.InitializeComponent();
            this.NameComboBox.ItemsSource = Constants.Languages;
        }

        private bool Editing { get; set; }

        private Guid SelectedLanguageId { get; set; }

        public void ShowAdd()
        {
            this.HeaderTextBlock.Text = "Add Language";
            this.Editing = false;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.ClearFields();

            this.ShowDialog();
        }

        public void ShowEdit(Language language)
        {
            this.HeaderTextBlock.Text = "Edit Language";
            this.SelectedLanguageId = language.ID;
            this.Editing = true;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.FillFields(language);

            this.ShowDialog();
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

            Program.Modified = true;
        }

        private Language ToLanguage()
        {
            Language language = new Language()
            {
                Name = this.NameComboBox.Text,
                Script = this.ScriptTextBox.Text,
                Speakers = this.SpeakersTextBox.Text,
            };

            return language;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Editing)
            {
                this.UpdateLanguage(Program.Character.Details.GetLanguageById(this.SelectedLanguageId));
            }
            else
            {
                Program.Character.Details.Languages.Add(this.ToLanguage());
                Program.Modified = true;
            }

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Details.Languages.Add(this.ToLanguage());
            Program.Modified = true;
            this.ClearFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NameComboBox.SelectedItem != null)
            {
                this.FillFields(this.NameComboBox.SelectedItem as Language);
            }
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.Black;
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.White;
        }
    }
}
