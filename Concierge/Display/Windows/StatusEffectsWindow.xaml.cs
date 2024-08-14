// <copyright file="StatusEffectsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
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
        private bool editing;
        private StatusEffect selectedEffect = new ();
        private List<StatusEffect> statusEffects = [];

        public StatusEffectsWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.NameComboBox.ItemsSource = DefaultItems;
            this.TypeComboBox.ItemsSource = ComboBoxGenerator.StatusEffectTypesComboBox();
            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.NameComboBox);
            this.SetMouseOverEvents(this.TypeComboBox);
            this.SetMouseOverEvents(this.DescriptionTextBox, this.DescriptionTextBackground);
        }

        public override string HeaderText => $"{(this.editing ? "Edit" : "Add")} Status Effect";

        public override string WindowName => nameof(StatusEffectsWindow);

        public bool ItemsAdded { get; private set; }

        private static List<ComboBoxItemControl> DefaultItems => ComboBoxGenerator.SelectorComboBox(Defaults.StatusEffects, Program.CustomItemService.GetItems<StatusEffect>());

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.statusEffects = Program.CcsFile.Character.Vitality.Status.StatusEffects;
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

            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.statusEffects = castItem;
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

            this.editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.selectedEffect = castItem;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            if (this.editing)
            {
                this.UpdateStatusEffect();
            }
            else if (!this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                this.statusEffects.Add(this.ToStatusEffect());
            }

            this.CloseConciergeWindow();
        }

        private void FillFields(StatusEffect statusEffect)
        {
            Program.Drawing();

            this.NameComboBox.Text = statusEffect.Name;
            this.TypeComboBox.Text = statusEffect.Type.ToString();
            this.DescriptionTextBox.Text = statusEffect.Description;

            Program.NotDrawing();
        }

        private void ClearFields(string name = "")
        {
            Program.Drawing();

            this.NameComboBox.Text = name;
            this.TypeComboBox.Text = StatusEffectTypes.None.ToString();
            this.DescriptionTextBox.Text = string.Empty;

            Program.NotDrawing();
        }

        private void UpdateStatusEffect()
        {
            var oldItem = this.selectedEffect.DeepCopy();

            this.selectedEffect.Name = this.NameComboBox.Text;
            this.selectedEffect.Type = this.TypeComboBox.Text.ToEnum<StatusEffectTypes>();
            this.selectedEffect.Description = this.DescriptionTextBox.Text;

            if (!this.selectedEffect.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<StatusEffect>(this.selectedEffect, oldItem, this.ConciergePage));
            }
        }

        private StatusEffect Create()
        {
            return new StatusEffect()
            {
                Name = this.NameComboBox.Text,
                Type = this.TypeComboBox.Text.ToEnum<StatusEffectTypes>(),
                Description = this.DescriptionTextBox.Text,
            };
        }

        private StatusEffect ToStatusEffect()
        {
            this.ItemsAdded = true;
            var effect = this.Create();

            Program.UndoRedoService.AddCommand(new AddCommand<StatusEffect>(this.statusEffects, effect, this.ConciergePage));

            return effect;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.statusEffects.Add(this.ToStatusEffect());
            this.ClearFields();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.ShowWarning("Could not save the Status Effect.\nA name is required before saving a custom item.");
                return;
            }

            Program.CustomItemService.AddItem(this.Create());
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
