// <copyright file="ModifyAbilitiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.AbilitiesPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Collections;

    /// <summary>
    /// Interaction logic for ModifyAbilitiesWindow.xaml.
    /// </summary>
    public partial class ModifyAbilitiesWindow : Window
    {
        public ModifyAbilitiesWindow()
        {
            this.InitializeComponent();
        }

        private bool Editing { get; set; }

        private Guid SelectedAbilityId { get; set; }

        public void ShowEdit(Ability ability)
        {
            this.HeaderTextBlock.Text = "Edit Ability";
            this.SelectedAbilityId = ability.ID;
            this.Editing = true;
            this.FillFields(ability);
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.ShowDialog();
        }

        public void ShowAdd()
        {
            this.HeaderTextBlock.Text = "Add Ability";
            this.Editing = false;
            this.ClearFields();
            this.ApplyButton.Visibility = Visibility.Visible;

            this.ShowDialog();
        }

        private void FillFields(Ability ability)
        {
            this.NameTextBox.Text = ability.Name;
            this.LevelTextBox.Text = ability.Level;
            this.UsesTextBox.Text = ability.Uses;
            this.RecoveryTextBox.Text = ability.Recovery;
            this.ActionTextBox.Text = ability.Action;
            this.NotesTextBox.Text = ability.Note;
        }

        private void ClearFields()
        {
            this.NameTextBox.Text = string.Empty;
            this.LevelTextBox.Text = string.Empty;
            this.UsesTextBox.Text = string.Empty;
            this.RecoveryTextBox.Text = string.Empty;
            this.ActionTextBox.Text = string.Empty;
            this.NotesTextBox.Text = string.Empty;
        }

        private Ability ToAbility()
        {
            var ability = new Ability()
            {
                Name = this.NameTextBox.Text,
                Level = this.LevelTextBox.Text,
                Uses = this.UsesTextBox.Text,
                Recovery = this.RecoveryTextBox.Text,
                Action = this.ActionTextBox.Text,
                Note = this.NotesTextBox.Text,
            };

            return ability;
        }

        private void UpdateAbility(Ability ability)
        {
            ability.Name = this.NameTextBox.Text;
            ability.Level = this.LevelTextBox.Text;
            ability.Uses = this.UsesTextBox.Text;
            ability.Recovery = this.RecoveryTextBox.Text;
            ability.Action = this.ActionTextBox.Text;
            ability.Note = this.NotesTextBox.Text;

            Program.Modified = true;
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

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Abilities.Add(this.ToAbility());
            Program.Modified = true;
            this.ClearFields();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Editing)
            {
                this.UpdateAbility(Program.Character.GetAbilityById(this.SelectedAbilityId));
            }
            else
            {
                Program.Character.Abilities.Add(this.ToAbility());
                Program.Modified = true;
            }

            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
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
