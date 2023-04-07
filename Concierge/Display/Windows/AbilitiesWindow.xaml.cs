// <copyright file="AbilitiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for AbilitiesWindow.xaml.
    /// </summary>
    public partial class AbilitiesWindow : ConciergeWindow
    {
        public AbilitiesWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.NameComboBox.ItemsSource = GenerateComboBoxItems();
            this.TypeComboBox.ItemsSource = StringUtility.FormatEnumForDisplay(typeof(AbilityTypes));
            this.ConciergePage = ConciergePage.None;
            this.Abilities = new List<Ability>();
            this.SelectedAbility = new Ability();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetFocusEvents(this.NameComboBox);
            this.SetFocusEvents(this.TypeComboBox);
            this.SetFocusEvents(this.UsesTextBox);
            this.SetFocusEvents(this.ActionTextBox);
            this.SetFocusEvents(this.LevelUpDown);
            this.SetFocusEvents(this.RecoveryTextBox);
            this.SetFocusEvents(this.NotesTextBox);
        }

        public bool ItemsAdded { get; private set; }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Ability";

        public override string WindowName => nameof(AbilitiesWindow);

        private bool Editing { get; set; }

        private Ability SelectedAbility { get; set; }

        private List<Ability> Abilities { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Abilities = Program.CcsFile.Character.Abilities;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T ability)
        {
            if (ability is not Ability castItem)
            {
                return;
            }

            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedAbility = castItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        public override bool ShowAdd<T>(T abilities)
        {
            if (abilities is not List<Ability> castItem)
            {
                return false;
            }

            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Abilities = castItem;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        protected override void ReturnAndClose()
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

            this.CloseConciergeWindow();

            Program.Modify();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
        }

        private static CompositeCollection GenerateComboBoxItems()
        {
            return new CompositeCollection
            {
                new CollectionContainer() { Collection = Constants.Abilities.Where(x => x.Type == AbilityTypes.Feat).OrderBy(x => x.Name).ToList() },
                new Separator(),
                new CollectionContainer() { Collection = Constants.Abilities.Where(x => x.Type == AbilityTypes.Background).OrderBy(x => x.Name).ToList() },
            };
        }

        private void FillFields(Ability ability)
        {
            this.NameComboBox.Text = ability.Name;
            this.TypeComboBox.Text = ability.Type.ToString().FormatFromEnum();
            this.LevelUpDown.Value = ability.Level;
            this.UsesTextBox.Text = ability.Uses;
            this.RecoveryTextBox.Text = ability.Recovery;
            this.ActionTextBox.Text = ability.Action;
            this.NotesTextBox.Text = ability.Description;
        }

        private void ClearFields(string name = "")
        {
            this.NameComboBox.Text = name;
            this.TypeComboBox.Text = AbilityTypes.None.ToString();
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
                Type = (AbilityTypes)Enum.Parse(typeof(AbilityTypes), this.TypeComboBox.Text.Strip(" ")),
                Level = this.LevelUpDown.Value,
                Uses = this.UsesTextBox.Text,
                Recovery = this.RecoveryTextBox.Text,
                Action = this.ActionTextBox.Text,
                Description = this.NotesTextBox.Text,
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Ability>(this.Abilities, ability, this.ConciergePage));

            return ability;
        }

        private void UpdateAbility(Ability ability)
        {
            var oldItem = ability.DeepCopy();

            ability.Name = this.NameComboBox.Text;
            ability.Type = (AbilityTypes)Enum.Parse(typeof(AbilityTypes), this.TypeComboBox.Text.Strip(" "));
            ability.Level = this.LevelUpDown.Value;
            ability.Uses = this.UsesTextBox.Text;
            ability.Recovery = this.RecoveryTextBox.Text;
            ability.Action = this.ActionTextBox.Text;
            ability.Description = this.NotesTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Ability>(ability, oldItem, this.ConciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
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
            this.ReturnAndClose();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NameComboBox.SelectedItem is Ability ability)
            {
                this.FillFields(ability);
            }
            else
            {
                this.ClearFields(this.NameComboBox.Text);
            }
        }
    }
}
