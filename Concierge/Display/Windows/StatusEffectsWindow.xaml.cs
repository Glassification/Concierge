﻿// <copyright file="StatusEffectsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.Enums;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for StatusEffectsWindow.xaml.
    /// </summary>
    public partial class StatusEffectsWindow : ConciergeWindow
    {
        public StatusEffectsWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.NameComboBox.ItemsSource = Constants.StatusEffects;
            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(StatusEffectTypes)).Cast<StatusEffectTypes>();
            this.ConciergePage = ConciergePage.None;
            this.SelectedEffect = new StatusEffect();
            this.StatusEffects = new List<StatusEffect>();
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Status Effect";

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private StatusEffect SelectedEffect { get; set; }

        private List<StatusEffect> StatusEffects { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.StatusEffects = Program.CcsFile.Character.StatusEffects;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T statusEffects)
        {
            if (statusEffects is not List<StatusEffect> castItem)
            {
                return false;
            }

            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.StatusEffects = castItem;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        public override void ShowEdit<T>(T statusEffect)
        {
            if (statusEffect is not StatusEffect castItem)
            {
                return;
            }

            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.SelectedEffect = castItem;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateStatusEffect();
            }
            else if (!this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                this.StatusEffects.Add(this.ToStatusEffect());
            }

            this.CloseConciergeWindow();
            Program.Modify();
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
            var oldItem = this.SelectedEffect.DeepCopy();

            this.SelectedEffect.Name = this.NameComboBox.Text;
            this.SelectedEffect.Type = (StatusEffectTypes)Enum.Parse(typeof(StatusEffectTypes), this.TypeComboBox.Text);
            this.SelectedEffect.Description = this.DescriptionTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<StatusEffect>(this.SelectedEffect, oldItem, this.ConciergePage));
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

            Program.UndoRedoService.AddCommand(new AddCommand<StatusEffect>(this.StatusEffects, effect, this.ConciergePage));

            return effect;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.StatusEffects.Add(this.ToStatusEffect());
            this.ClearFields();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}