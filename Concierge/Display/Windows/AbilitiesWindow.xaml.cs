﻿// <copyright file="AbilitiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Details;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for AbilitiesWindow.xaml.
    /// </summary>
    public partial class AbilitiesWindow : ConciergeWindow
    {
        private bool editing;
        private Ability selectedAbility = new ();
        private List<Ability> abilities = [];

        public AbilitiesWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.NameComboBox.ItemsSource = DefaultItems;
            this.TypeComboBox.ItemsSource = ComboBoxGenerator.AbilityTypesComboBox();
            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.NameComboBox);
            this.SetMouseOverEvents(this.TypeComboBox);
            this.SetMouseOverEvents(this.UsesTextBox, this.UsesTextBackground);
            this.SetMouseOverEvents(this.ActionTextBox, this.ActionTextBackground);
            this.SetMouseOverEvents(this.LevelUpDown);
            this.SetMouseOverEvents(this.RecoveryTextBox, this.RecoveryTextBackground);
            this.SetMouseOverEvents(this.NotesTextBox, this.NotesTextBackground);
        }

        public bool ItemsAdded { get; private set; }

        public override string HeaderText => $"{(this.editing ? "Edit" : "Add")} Ability";

        public override string WindowName => nameof(AbilitiesWindow);

        private static List<ComboBoxItemControl> DefaultItems => ComboBoxGenerator.SelectorComboBox(Defaults.Abilities, Program.CustomItemService.GetItems<Ability>());

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.abilities = Program.CcsFile.Character.Detail.Abilities;
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

            this.editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.selectedAbility = castItem;
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

            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.abilities = castItem;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            if (this.editing)
            {
                this.UpdateAbility(this.selectedAbility);
            }
            else
            {
                Program.CcsFile.Character.Detail.Abilities.Add(this.ToAbility());
            }

            this.CloseConciergeWindow();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
        }

        private void FillFields(Ability ability)
        {
            Program.Drawing();

            this.NameComboBox.Text = ability.Name;
            this.TypeComboBox.Text = ability.Type.PascalCase();
            this.LevelUpDown.Value = ability.Level;
            this.UsesTextBox.Text = ability.Uses;
            this.RecoveryTextBox.Text = ability.Recovery;
            this.ActionTextBox.Text = ability.Action;
            this.NotesTextBox.Text = ability.Description;

            Program.NotDrawing();
        }

        private void ClearFields(string name = "")
        {
            Program.Drawing();

            this.NameComboBox.Text = name;
            this.TypeComboBox.Text = AbilityTypes.None.ToString();
            this.LevelUpDown.Value = 0;
            this.UsesTextBox.Text = string.Empty;
            this.RecoveryTextBox.Text = string.Empty;
            this.ActionTextBox.Text = string.Empty;
            this.NotesTextBox.Text = string.Empty;

            Program.NotDrawing();
        }

        private Ability Create()
        {
            return new Ability()
            {
                Name = this.NameComboBox.Text,
                Type = this.TypeComboBox.Text.ToEnum<AbilityTypes>(),
                Level = this.LevelUpDown.Value,
                Uses = this.UsesTextBox.Text,
                Recovery = this.RecoveryTextBox.Text,
                Action = this.ActionTextBox.Text,
                Description = this.NotesTextBox.Text,
            };
        }

        private Ability ToAbility()
        {
            this.ItemsAdded = true;
            var ability = this.Create();

            Program.UndoRedoService.AddCommand(new AddCommand<Ability>(this.abilities, ability, this.ConciergePage));

            return ability;
        }

        private void UpdateAbility(Ability ability)
        {
            var oldItem = ability.DeepCopy();

            ability.Name = this.NameComboBox.Text;
            ability.Type = this.TypeComboBox.Text.ToEnum<AbilityTypes>();
            ability.Level = this.LevelUpDown.Value;
            ability.Uses = this.UsesTextBox.Text;
            ability.Recovery = this.RecoveryTextBox.Text;
            ability.Action = this.ActionTextBox.Text;
            ability.Description = this.NotesTextBox.Text;

            if (!ability.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<Ability>(ability, oldItem, this.ConciergePage));
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.abilities.Add(this.ToAbility());
            this.ClearFields();
            this.InvokeApplyChanges();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isLocked = this.LockButton.IsChecked ?? false;
            if (this.NameComboBox.SelectedItem is ComboBoxItemControl item && item.Item is Ability ability && !isLocked)
            {
                this.FillFields(ability);
            }
            else if (!isLocked)
            {
                this.ClearFields(this.NameComboBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.ShowWarning("Could not save the Ability.\nA name is required before saving a custom item.");
                return;
            }

            Program.CustomItemService.AddItem(this.Create());
            this.ClearFields();
            this.NameComboBox.ItemsSource = DefaultItems;
        }

        private void LockButton_Checked(object sender, RoutedEventArgs e)
        {
            this.LockIcon.Kind = PackIconKind.Lock;
        }

        private void LockButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.LockIcon.Kind = PackIconKind.LockOpenVariant;
        }
    }
}
