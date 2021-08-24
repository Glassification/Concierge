// <copyright file="ModifyProficiencyWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyProficiencyWindow.xaml.
    /// </summary>
    public partial class ModifyProficiencyWindow : Window, IConciergeWindow
    {
        public ModifyProficiencyWindow()
        {
            this.InitializeComponent();
            this.ProficiencyComboBox.ItemsSource = Enum.GetValues(typeof(ProficiencyTypes)).Cast<ProficiencyTypes>();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private bool Editing { get; set; }

        private Guid SelectedProficiencyId { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Proficiency";

        private MessageWindowResult Result { get; set; }

        public MessageWindowResult ShowWizardSetup()
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.ProficiencyComboBox.IsEnabled = true;

            this.ClearFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowAdd()
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ClearFields();
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;
            this.ProficiencyComboBox.IsEnabled = true;

            this.ShowDialog();
        }

        public void ShowEdit(Guid id)
        {
            this.Editing = true;
            this.SelectedProficiencyId = id;
            this.HeaderTextBlock.Text = this.HeaderText;

            var proficiency = Program.CcsFile.Character.Proficiency.GetProficiencyById(id);
            this.ProficiencyTextBox.Text = proficiency.Proficiency;
            this.ProficiencyComboBox.Text = proficiency.ProficiencyTypes.ToString();
            this.ProficiencyComboBox.IsEnabled = false;

            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;

            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void ClearFields()
        {
            this.ProficiencyTextBox.Text = string.Empty;
            this.ProficiencyComboBox.Text = string.Empty;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageWindowResult.Exit;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = MessageWindowResult.OK;

            if (this.Editing)
            {
                Program.CcsFile.Character.Proficiency.SetProficiencyById(this.SelectedProficiencyId, this.ProficiencyTextBox.Text);
            }
            else
            {
                Program.CcsFile.Character.Proficiency.AddProficiencyByProficiencyType(
                    (ProficiencyTypes)Enum.Parse(typeof(ProficiencyTypes), this.ProficiencyComboBox.Text),
                    this.ProficiencyTextBox.Text);
            }

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            Program.CcsFile.Character.Proficiency.AddProficiencyByProficiencyType(
                (ProficiencyTypes)Enum.Parse(typeof(ProficiencyTypes), this.ProficiencyComboBox.Text),
                this.ProficiencyTextBox.Text);

            this.ClearFields();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageWindowResult.Cancel;
            this.Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = MessageWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void ProficiencyComboBox_DropDownOpened(object sender, EventArgs e)
        {
            ConciergeSound.UpdateValue();
        }
    }
}
