// <copyright file="ModifyAmmoWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.EquipmentPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyAmmoWindow.xaml.
    /// </summary>
    public partial class ModifyAmmoWindow : Window, IConciergeWindow
    {
        public ModifyAmmoWindow()
        {
            this.InitializeComponent();
            this.NameComboBox.ItemsSource = Constants.Ammunitions;
            this.DamageTypeComboBox.ItemsSource = Enum.GetValues(typeof(DamageTypes)).Cast<DamageTypes>();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private bool Editing { get; set; }

        private Ammunition SelectedAmmo { get; set; }

        private List<Ammunition> Ammunitions { get; set; }

        private MessageWindowResult Result { get; set; }

        public MessageWindowResult ShowWizardSetup()
        {
            this.Ammunitions = Program.CcsFile.Character.Ammunitions;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.Editing = false;
            this.HeaderTextBlock.Text = "Add Ammunitionn";

            this.ClearFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowAdd(List<Ammunition> ammunitions)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = "Add Ammunition";
            this.Ammunitions = ammunitions;
            this.ApplyButton.Visibility = Visibility.Visible;

            this.ClearFields();
            this.ShowDialog();
        }

        public void ShowEdit(Ammunition ammunition)
        {
            this.HeaderTextBlock.Text = "Edit Ammunition";
            this.SelectedAmmo = ammunition;
            this.Editing = true;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(ammunition);
            this.ShowDialog();
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
            ammunition.Name = this.NameComboBox.Text;
            ammunition.Quantity = this.QuantityUpDown.Value ?? 0;
            ammunition.Bonus = this.BonusTextBox.Text;
            ammunition.DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text);
            ammunition.Used = this.UsedUpDown.Value ?? 0;
        }

        private Ammunition ToAmmunition()
        {
            return new Ammunition()
            {
                Name = this.NameComboBox.Text,
                Quantity = this.QuantityUpDown.Value ?? 0,
                Bonus = this.BonusTextBox.Text,
                DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text),
                Used = this.UsedUpDown.Value ?? 0,
            };
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = MessageWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageWindowResult.Exit;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = MessageWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateAmmunition(this.SelectedAmmo);
            }
            else
            {
                this.Ammunitions.Add(this.ToAmmunition());
            }

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.Ammunitions.Add(this.ToAmmunition());
            this.ClearFields();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageWindowResult.Cancel;
            this.Hide();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NameComboBox.SelectedItem != null)
            {
                this.FillFields(this.NameComboBox.SelectedItem as Ammunition);
            }
        }

        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            ConciergeSound.UpdateValue();
        }
    }
}
