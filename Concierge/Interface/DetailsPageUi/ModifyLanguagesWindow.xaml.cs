// <copyright file="ModifyLanguagesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.DetailsPageUi
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Characteristics;
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

        private Language SelectedLanguage { get; set; }

        private List<Language> Languages { get; set; }

        public void ShowAdd(List<Language> languages)
        {
            this.HeaderTextBlock.Text = "Add Language";
            this.Languages = languages;
            this.Editing = false;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.ClearFields();

            this.ShowDialog();
        }

        public void ShowEdit(Language language)
        {
            this.HeaderTextBlock.Text = "Edit Language";
            this.SelectedLanguage = language;
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
            Program.Modify();

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

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }
    }
}
