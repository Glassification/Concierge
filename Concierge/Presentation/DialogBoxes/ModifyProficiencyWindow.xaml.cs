using Concierge.Utility;
using System;
using System.Windows;
using System.Windows.Input;

namespace Concierge.Presentation.DialogBoxes
{
    /// <summary>
    /// Interaction logic for ModifyProficiencyWindow.xaml
    /// </summary>
    public partial class ModifyProficiencyWindow : Window
    {
        public ModifyProficiencyWindow()
        {
            InitializeComponent();
            PopupButton = Constants.PopupButtons.Cancel;
        }

        public void ShowAdd(Constants.PopupButtons popupButton)
        {
            PopupButton = popupButton;
            Editing = false;
            HeaderTextBlock.Text = HeaderText;
            ClearFields();
            ApplyButton.Visibility = Visibility.Visible;

            ShowDialog();
        }

        public void ShowEdit(Constants.PopupButtons popupButton, Guid id)
        {
            PopupButton = popupButton;
            Editing = true;
            SelectedProficiencyId = id;
            HeaderTextBlock.Text = HeaderText;
            ProficiencyTextBox.Text = Program.Character.Proficiency.GetProficiencyById(id);
            ApplyButton.Visibility = Visibility.Collapsed;

            ShowDialog();
        }

        private void ClearFields()
        {
            ProficiencyTextBox.Text = string.Empty;
        }

        private Constants.PopupButtons PopupButton { get; set; }
        private bool Editing { get; set; }
        private Guid SelectedProficiencyId { get; set; }

        private string HeaderText
        {
            get
            {
                return $"{(Editing ? "Edit" : "Add")} " +
                    $"{PopupButton.ToString().Replace("Proficiency", "")} " +
                    $"Proficiency";
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
                Program.Character.Proficiency.SetProficiencyById(SelectedProficiencyId, ProficiencyTextBox.Text);
            }
            else
            {
                Program.Character.Proficiency.AddProficiencyByPopupButton(PopupButton, ProficiencyTextBox.Text);
            }

            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Proficiency.AddProficiencyByPopupButton(PopupButton, ProficiencyTextBox.Text);
            ClearFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Hide();
                    break;
            }
        }
    }
}
