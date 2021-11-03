// <copyright file="ModifyStatusEffectsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.AttackDefensePageInterface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Enums;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for ModifyStatusEffectsWindow.xaml.
    /// </summary>
    public partial class ModifyStatusEffectsWindow : Window, IConciergeModifyWindow
    {
        public ModifyStatusEffectsWindow()
        {
            this.InitializeComponent();

            this.NameComboBox.ItemsSource = Constants.StatusEffects;
            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(StatusEffectTypes)).Cast<StatusEffectTypes>();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Status Effect";

        private StatusEffect SelectedEffect { get; set; }

        private List<StatusEffect> StatusEffects { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.StatusEffects = Program.CcsFile.Character.StatusEffects;

            this.ClearFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowAdd(List<StatusEffect> statusEffects)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;
            this.StatusEffects = statusEffects;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowDialog();
        }

        public void ShowEdit(StatusEffect statusEffect)
        {
            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;
            this.SelectedEffect = statusEffect;

            this.FillFields(statusEffect);
            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void FillFields(StatusEffect statusEffect)
        {
            this.NameComboBox.Text = statusEffect.Name;
            this.TypeComboBox.Text = statusEffect.Type.ToString();
            this.DescriptionTextBox.Text = statusEffect.Description;
        }

        private void ClearFields()
        {
            this.NameComboBox.Text = string.Empty;
            this.TypeComboBox.Text = StatusEffectTypes.None.ToString();
            this.DescriptionTextBox.Text = string.Empty;
        }

        private void UpdateStatusEffect()
        {
            var oldItem = this.SelectedEffect.DeepCopy() as StatusEffect;

            this.SelectedEffect.Name = this.NameComboBox.Text;
            this.SelectedEffect.Type = (StatusEffectTypes)Enum.Parse(typeof(StatusEffectTypes), this.TypeComboBox.Text);
            this.SelectedEffect.Description = this.DescriptionTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<StatusEffect>(this.SelectedEffect, oldItem));
        }

        private StatusEffect ToStatusEffect()
        {
            this.ItemsAdded = true;

            var effect = new StatusEffect()
            {
                Name = this.NameComboBox.Text,
                Type = (StatusEffectTypes)Enum.Parse(typeof(StatusEffectTypes), this.TypeComboBox.Text),
                Description = this.DescriptionTextBox.Text,
            };

            Program.UndoRedoService.AddCommand(new AddCommand<StatusEffect>(this.StatusEffects, effect));

            return effect;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateStatusEffect();
            }
            else if (!this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                this.StatusEffects.Add(this.ToStatusEffect());
            }

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.StatusEffects.Add(this.ToStatusEffect());
            this.ClearFields();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = ConciergeWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
