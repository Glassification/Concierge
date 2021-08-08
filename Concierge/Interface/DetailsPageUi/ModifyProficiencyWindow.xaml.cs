// <copyright file="ModifyProficiencyWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.DetailsPageUi
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyProficiencyWindow.xaml.
    /// </summary>
    public partial class ModifyProficiencyWindow : Window
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

        public void ShowAdd()
        {
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Editing = false;
            this.ClearFields();
            this.ApplyButton.Visibility = Visibility.Visible;
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

            this.ShowDialog();
        }

        private void ClearFields()
        {
            this.ProficiencyTextBox.Text = string.Empty;
            this.ProficiencyComboBox.Text = string.Empty;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.TapNavigation();

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
            ConciergeSound.TapNavigation();

            Program.CcsFile.Character.Proficiency.AddProficiencyByProficiencyType(
                (ProficiencyTypes)Enum.Parse(typeof(ProficiencyTypes), this.ProficiencyComboBox.Text),
                this.ProficiencyTextBox.Text);

            this.ClearFields();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
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

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }

        private void ProficiencyComboBox_DropDownOpened(object sender, EventArgs e)
        {
            ConciergeSound.UpdateValue();
        }
    }
}
