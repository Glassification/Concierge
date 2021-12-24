// <copyright file="ModifyAbilitiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.AbilitiesPageInterface
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Characteristics;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyAbilitiesWindow.xaml.
    /// </summary>
    public partial class ModifyAbilitiesWindow : ConciergeWindow, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifyAbilitiesWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();

            this.NameComboBox.ItemsSource = Constants.Abilities;
            this.conciergePage = conciergePage;
        }

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Ability";

        private Ability SelectedAbility { get; set; }

        private List<Ability> Abilities { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Abilities = Program.CcsFile.Character.Abilities;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public void ShowEdit(Ability ability)
        {
            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedAbility = ability;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;

            this.FillFields(ability);
            this.ShowConciergeWindow();
        }

        public void ShowAdd(List<Ability> abilities)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Abilities = abilities;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
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
            this.ItemsAdded = true;

            var ability = new Ability()
            {
                Name = this.NameComboBox.Text,
                Level = this.LevelUpDown.Value ?? 0,
                Uses = this.UsesTextBox.Text,
                Recovery = this.RecoveryTextBox.Text,
                Action = this.ActionTextBox.Text,
                Description = this.NotesTextBox.Text,
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Ability>(this.Abilities, ability, this.conciergePage));

            return ability;
        }

        private void UpdateAbility(Ability ability)
        {
            var oldItem = ability.DeepCopy() as Ability;

            ability.Name = this.NameComboBox.Text;
            ability.Level = this.LevelUpDown.Value ?? 0;
            ability.Uses = this.UsesTextBox.Text;
            ability.Recovery = this.RecoveryTextBox.Text;
            ability.Action = this.ActionTextBox.Text;
            ability.Description = this.NotesTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Ability>(ability, oldItem, this.conciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Abilities.Add(this.ToAbility());
            this.ClearFields();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateAbility(this.SelectedAbility);
            }
            else
            {
                Program.CcsFile.Character.Abilities.Add(this.ToAbility());
            }

            this.HideConciergeWindow();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.HideConciergeWindow();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NameComboBox.SelectedItem != null)
            {
                this.FillFields(this.NameComboBox.SelectedItem as Ability);
            }
        }
    }
}
