// <copyright file="ModifyProficiencyWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.DetailsPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Characters.Enums;

    /// <summary>
    /// Interaction logic for ModifyProficiencyWindow.xaml.
    /// </summary>
    public partial class ModifyProficiencyWindow : Window
    {
        public ModifyProficiencyWindow()
        {
            this.InitializeComponent();
            this.PopupButton = PopupButtons.Cancel;
        }

        private PopupButtons PopupButton { get; set; }

        private bool Editing { get; set; }

        private Guid SelectedProficiencyId { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} {this.PopupButton.ToString().Replace("Proficiency", string.Empty)} Proficiency";

        public void ShowAdd(PopupButtons popupButton)
        {
            this.PopupButton = popupButton;
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ClearFields();
            this.ApplyButton.Visibility = Visibility.Visible;

            this.ShowDialog();
        }

        public void ShowEdit(PopupButtons popupButton, Guid id)
        {
            this.PopupButton = popupButton;
            this.Editing = true;
            this.SelectedProficiencyId = id;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ProficiencyTextBox.Text = Program.Character.Proficiency.GetProficiencyById(id);
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.ShowDialog();
        }

        private void ClearFields()
        {
            this.ProficiencyTextBox.Text = string.Empty;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Editing)
            {
                Program.Character.Proficiency.SetProficiencyById(this.SelectedProficiencyId, this.ProficiencyTextBox.Text);
                Program.Modified = true;
            }
            else
            {
                Program.Character.Proficiency.AddProficiencyByPopupButton(this.PopupButton, this.ProficiencyTextBox.Text);
                Program.Modified = true;
            }

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Proficiency.AddProficiencyByPopupButton(this.PopupButton, this.ProficiencyTextBox.Text);
            Program.Modified = true;
            this.ClearFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
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
    }
}
