using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Concierge.Presentation.DialogBoxes
{
    /// <summary>
    /// Interaction logic for ModifyLanguagesWindow.xaml
    /// </summary>
    public partial class ModifyLanguagesWindow : Window
    {
        public ModifyLanguagesWindow()
        {
            InitializeComponent();
            NameComboBox.ItemsSource = Constants.Languages;
        }

        public void ShowAdd()
        {
            HeaderTextBlock.Text = "Add Language";
            Editing = false;
            ApplyButton.Visibility = Visibility.Visible;
            ClearFields();

            ShowDialog();
        }

        public void ShowEdit(Language language)
        {
            HeaderTextBlock.Text = "Edit Language";
            SelectedLanguageId = language.ID;
            Editing = true;
            ApplyButton.Visibility = Visibility.Collapsed;
            FillFields(language);

            ShowDialog();
        }

        private void FillFields(Language language)
        {
            NameComboBox.Text = language.Name;
            ScriptTextBox.Text = language.Script;
            SpeakersTextBox.Text = language.Speakers;
        }

        private void ClearFields()
        {
            NameComboBox.Text = string.Empty;
            ScriptTextBox.Text = string.Empty;
            SpeakersTextBox.Text = string.Empty;
        }

        private void UpdateLanguage(Language language)
        {
            language.Name = NameComboBox.Text;
            language.Script = ScriptTextBox.Text;
            language.Speakers = SpeakersTextBox.Text;
        }

        private Language ToLanguage()
        {
            Language language = new Language()
            {
                Name = NameComboBox.Text,
                Script = ScriptTextBox.Text,
                Speakers = SpeakersTextBox.Text
            };

            return language;
        }

        private bool Editing { get; set; }
        private Guid SelectedLanguageId { get; set; }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Editing)
            {
                UpdateLanguage(Program.Character.Details.GetLanguageById(SelectedLanguageId));
            }
            else
            {
                Program.Character.Details.Languages.Add(ToLanguage());
            }

            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Details.Languages.Add(ToLanguage());
            ClearFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NameComboBox.SelectedItem != null)
            {
                FillFields(NameComboBox.SelectedItem as Language);
            }
        }
    }
}
