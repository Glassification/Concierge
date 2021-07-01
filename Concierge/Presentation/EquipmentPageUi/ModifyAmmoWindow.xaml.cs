// <copyright file="ModifyAmmoWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.EquipmentPageUi
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Characters.Collections;
    using Concierge.Characters.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyAmmoWindow.xaml.
    /// </summary>
    public partial class ModifyAmmoWindow : Window
    {
        public ModifyAmmoWindow()
        {
            this.InitializeComponent();
            this.NameComboBox.ItemsSource = Constants.Ammunitions;
            this.DamageTypeComboBox.ItemsSource = Enum.GetValues(typeof(DamageTypes)).Cast<DamageTypes>();
        }

        private bool Editing { get; set; }

        private Guid SelectedAmmoId { get; set; }

        public void ShowAdd()
        {
            this.HeaderTextBlock.Text = "Add Ammunition";
            this.Editing = false;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.ClearFields();

            this.ShowDialog();
        }

        public void ShowEdit(Ammunition ammunition)
        {
            this.HeaderTextBlock.Text = "Edit Ammunition";
            this.SelectedAmmoId = ammunition.ID;
            this.Editing = true;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.FillFields(ammunition);

            this.ShowDialog();
        }

        private void FillFields(Ammunition ammunition)
        {
            this.NameComboBox.Text = ammunition.Name;
            this.QuantityUpDown.Value = ammunition.Quantity;
            this.BonusTextBox.Text = ammunition.Bonus;
            this.DamageTypeComboBox.Text = ammunition.DamageType.ToString();
            this.UsedUpDown.Value = ammunition.Used;
        }

        private void ClearFields()
        {
            this.NameComboBox.Text = string.Empty;
            this.QuantityUpDown.Value = 0;
            this.BonusTextBox.Text = string.Empty;
            this.DamageTypeComboBox.Text = string.Empty;
            this.UsedUpDown.Value = 0;
        }

        private void UpdateAmmunition(Ammunition ammunition)
        {
            ammunition.Name = this.NameComboBox.Text;
            ammunition.Quantity = this.QuantityUpDown.Value ?? 0;
            ammunition.Bonus = this.BonusTextBox.Text;
            ammunition.DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text);
            ammunition.Used = this.UsedUpDown.Value ?? 0;

            Program.Modified = true;
        }

        private Ammunition ToAmmunition()
        {
            var ammunition = new Ammunition()
            {
                Name = this.NameComboBox.Text,
                Quantity = this.QuantityUpDown.Value ?? 0,
                Bonus = this.BonusTextBox.Text,
                DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text),
                Used = this.UsedUpDown.Value ?? 0,
            };

            return ammunition;
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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Editing)
            {
                this.UpdateAmmunition(Program.Character.GetAmmunitionById(this.SelectedAmmoId));
            }
            else
            {
                Program.Character.Ammunitions.Add(this.ToAmmunition());
                Program.Modified = true;
            }

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Ammunitions.Add(this.ToAmmunition());
            Program.Modified = true;
            this.ClearFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
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
