// <copyright file="ModifyAmmoWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.AttackDefensePageInterface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyAmmoWindow.xaml.
    /// </summary>
    public partial class ModifyAmmoWindow : ConciergeWindow, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifyAmmoWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.NameComboBox.ItemsSource = Constants.Ammunitions;
            this.DamageTypeComboBox.ItemsSource = Enum.GetValues(typeof(DamageTypes)).Cast<DamageTypes>();
            this.conciergePage = conciergePage;
        }

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Ammunition";

        private Ammunition SelectedAmmo { get; set; }

        private List<Ammunition> Ammunitions { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Ammunitions = Program.CcsFile.Character.Ammunitions;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public void ShowAdd(List<Ammunition> ammunitions)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Ammunitions = ammunitions;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();
        }

        public void ShowEdit(Ammunition ammunition)
        {
            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedAmmo = ammunition;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;

            this.FillFields(ammunition);
            this.ShowConciergeWindow();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void FillFields(Ammunition ammunition)
        {
            this.QuantityUpDown.UpdatingValue();
            this.UsedUpDown.UpdatingValue();

            this.NameComboBox.Text = ammunition.Name;
            this.QuantityUpDown.Value = ammunition.Quantity;
            this.BonusTextBox.Text = ammunition.Bonus;
            this.DamageTypeComboBox.Text = ammunition.DamageType.ToString();
            this.UsedUpDown.Value = ammunition.Used;
        }

        private void ClearFields()
        {
            this.QuantityUpDown.UpdatingValue();
            this.UsedUpDown.UpdatingValue();

            this.NameComboBox.Text = string.Empty;
            this.QuantityUpDown.Value = 0;
            this.BonusTextBox.Text = string.Empty;
            this.DamageTypeComboBox.Text = DamageTypes.None.ToString();
            this.UsedUpDown.Value = 0;
        }

        private void UpdateAmmunition(Ammunition ammunition)
        {
            var oldItem = ammunition.DeepCopy();

            ammunition.Name = this.NameComboBox.Text;
            ammunition.Quantity = this.QuantityUpDown.Value ?? 0;
            ammunition.Bonus = this.BonusTextBox.Text;
            ammunition.DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text);
            ammunition.Used = this.UsedUpDown.Value ?? 0;

            Program.UndoRedoService.AddCommand(new EditCommand<Ammunition>(ammunition, oldItem, this.conciergePage));
        }

        private Ammunition ToAmmunition()
        {
            this.ItemsAdded = true;

            var ammo = new Ammunition()
            {
                Name = this.NameComboBox.Text,
                Quantity = this.QuantityUpDown.Value ?? 0,
                Bonus = this.BonusTextBox.Text,
                DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text),
                Used = this.UsedUpDown.Value ?? 0,
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Ammunition>(this.Ammunitions, ammo, this.conciergePage));

            return ammo;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateAmmunition(this.SelectedAmmo);
            }
            else
            {
                this.Ammunitions.Add(this.ToAmmunition());
            }

            this.HideConciergeWindow();
            Program.Modify();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Ammunitions.Add(this.ToAmmunition());
            this.ClearFields();

            this.InvokeApplyChanges();
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
                this.FillFields(this.NameComboBox.SelectedItem as Ammunition);
            }
        }
    }
}
