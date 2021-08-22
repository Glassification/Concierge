// <copyright file="ModifyAbilitiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.AbilitiesPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Characteristics;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyAbilitiesWindow.xaml.
    /// </summary>
    public partial class ModifyAbilitiesWindow : Window
    {
        public ModifyAbilitiesWindow()
        {
            this.InitializeComponent();

            this.NameComboBox.ItemsSource = Constants.Abilities;
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private bool Editing { get; set; }

        private Ability SelectedAbility { get; set; }

        private List<Ability> Abilities { get; set; }

        public void ShowEdit(Ability ability)
        {
            this.HeaderTextBlock.Text = "Edit Ability";
            this.SelectedAbility = ability;
            this.Editing = true;
            this.FillFields(ability);
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.ShowDialog();
        }

        public void ShowAdd(List<Ability> abilities)
        {
            this.HeaderTextBlock.Text = "Add Ability";
            this.Abilities = abilities;
            this.Editing = false;
            this.ClearFields();
            this.ApplyButton.Visibility = Visibility.Visible;

            this.ShowDialog();
        }

        private void FillFields(Ability ability)
        {
            this.LevelUpDown.UpdatingValue();

            this.NameComboBox.Text = ability.Name;
            this.LevelUpDown.Value = ability.Level;
            this.UsesTextBox.Text = ability.Uses;
            this.RecoveryTextBox.Text = ability.Recovery;
            this.ActionTextBox.Text = ability.Action;
            this.NotesTextBox.Text = ability.Description;
        }

        private void ClearFields()
        {
            this.LevelUpDown.UpdatingValue();

            this.NameComboBox.Text = string.Empty;
            this.LevelUpDown.Value = 0;
            this.UsesTextBox.Text = string.Empty;
            this.RecoveryTextBox.Text = string.Empty;
            this.ActionTextBox.Text = string.Empty;
            this.NotesTextBox.Text = string.Empty;
        }

        private Ability ToAbility()
        {
            return new Ability()
            {
                Name = this.NameComboBox.Text,
                Level = this.LevelUpDown.Value ?? 0,
                Uses = this.UsesTextBox.Text,
                Recovery = this.RecoveryTextBox.Text,
                Action = this.ActionTextBox.Text,
                Description = this.NotesTextBox.Text,
            };
        }

        private void UpdateAbility(Ability ability)
        {
            ability.Name = this.NameComboBox.Text;
            ability.Level = this.LevelUpDown.Value ?? 0;
            ability.Uses = this.UsesTextBox.Text;
            ability.Recovery = this.RecoveryTextBox.Text;
            ability.Action = this.ActionTextBox.Text;
            ability.Description = this.NotesTextBox.Text;
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
            Program.Modify();

            this.Abilities.Add(this.ToAbility());
            this.ClearFields();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            if (this.Editing)
            {
                this.UpdateAbility(this.SelectedAbility);
            }
            else
            {
                Program.CcsFile.Character.Abilities.Add(this.ToAbility());
            }

            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NameComboBox.SelectedItem != null)
            {
                this.FillFields(this.NameComboBox.SelectedItem as Ability);
            }
        }

        private void NameComboBox_DropDownOpened(object sender, EventArgs e)
        {
            ConciergeSound.UpdateValue();
        }
    }
}
