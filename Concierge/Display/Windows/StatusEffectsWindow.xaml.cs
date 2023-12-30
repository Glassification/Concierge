// <copyright file="StatusEffectsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for StatusEffectsWindow.xaml.
    /// </summary>
    public partial class StatusEffectsWindow : ConciergeWindow
    {
        public StatusEffectsWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.NameComboBox.ItemsSource = DefaultItems;
            this.TypeComboBox.ItemsSource = ComboBoxGenerator.StatusEffectTypesComboBox();
            this.ConciergePage = ConciergePage.None;
            this.SelectedEffect = new StatusEffect();
            this.StatusEffects = [];
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.NameComboBox);
            this.SetMouseOverEvents(this.TypeComboBox);
            this.SetMouseOverEvents(this.DescriptionTextBox, this.DescriptionTextBackground);
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Status Effect";

        public override string WindowName => nameof(StatusEffectsWindow);

        public bool ItemsAdded { get; private set; }

        private static List<ComboBoxItemControl> DefaultItems => ComboBoxGenerator.SelectorComboBox(Defaults.StatusEffects, Program.CustomItemService.GetCustomItems<StatusEffect>());

        private bool Editing { get; set; }

        private StatusEffect SelectedEffect { get; set; }

        private List<StatusEffect> StatusEffects { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.StatusEffects = Program.CcsFile.Character.Vitality.StatusEffects;
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
        }

        private void FillFields(StatusEffect statusEffect)
        {
            this.NameComboBox.Text = statusEffect.Name;
            this.TypeComboBox.Text = statusEffect.Type.ToString();
            this.DescriptionTextBox.Text = statusEffect.Description;
        }

        private void ClearFields(string name = "")
        {
            this.NameComboBox.Text = name;
            this.TypeComboBox.Text = StatusEffectTypes.None.ToString();
            this.DescriptionTextBox.Text = string.Empty;
        }

        private void UpdateStatusEffect()
        {
            var oldItem = this.SelectedEffect.DeepCopy();

            this.SelectedEffect.Name = this.NameComboBox.Text;
            this.SelectedEffect.Type = (StatusEffectTypes)Enum.Parse(typeof(StatusEffectTypes), this.TypeComboBox.Text);
            this.SelectedEffect.Description = this.DescriptionTextBox.Text;

            if (!this.SelectedEffect.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<StatusEffect>(this.SelectedEffect, oldItem, this.ConciergePage));
            }
        }

        private StatusEffect Create()
        {
            return new StatusEffect()
            {
                Name = this.NameComboBox.Text,
                Type = (StatusEffectTypes)Enum.Parse(typeof(StatusEffectTypes), this.TypeComboBox.Text),
                Description = this.DescriptionTextBox.Text,
            };
        }

        private StatusEffect ToStatusEffect()
        {
            this.ItemsAdded = true;
            var effect = this.Create();

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
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.Show(
                    "Could not save the Status Effect.\nA name is required before saving a custom item.",
                    "Warning",
                    ConciergeWindowButtons.Ok,
                    ConciergeWindowIcons.Alert);
                return;
            }

            Program.CustomItemService.AddCustomItem(this.Create());
            this.ClearFields();
            this.NameComboBox.ItemsSource = DefaultItems;
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NameComboBox.SelectedItem is ComboBoxItemControl item && item.Item is StatusEffect statusEffect)
            {
                this.FillFields(statusEffect);
            }
            else
            {
                this.ClearFields(this.NameComboBox.Text);
            }
        }
    }
}
