// <copyright file="EquipmentPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.EquipmentPageUi
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Characters.Collections;
    using Concierge.Characters.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for EquipmentPage.xaml.
    /// </summary>
    public partial class EquipmentPage : Page
    {
        public EquipmentPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.ModifyArmorWindow = new ModifyArmorWindow();
            this.EquipmentPopupWindow = new EquipmentPopupWindow();
            this.ModifyWeaponWindow = new ModifyWeaponWindow();
            this.ModifyAmmoWindow = new ModifyAmmoWindow();
        }

        private ModifyArmorWindow ModifyArmorWindow { get; }

        private ModifyWeaponWindow ModifyWeaponWindow { get; }

        private ModifyAmmoWindow ModifyAmmoWindow { get; }

        private EquipmentPopupWindow EquipmentPopupWindow { get; }

        public void Draw()
        {
            this.FillWeaponList();
            this.FillAmmoList();

            this.ArmorClassField.Text = Program.Character.Armor.TotalArmorClass.ToString();
            this.ArmorWornField.Text = Program.Character.Armor.Equiped;
            this.ArmorTypeField.Text = Program.Character.Armor.Type.ToString();
            this.ArmorStealthField.Text = Program.Character.Armor.Stealth.ToString();
            this.ShieldWornField.Text = Program.Character.Armor.Shield;
            this.ShieldAcField.Text = Program.Character.Armor.ShieldArmorClass.ToString();
            this.MiscBonusField.Text = Program.Character.Armor.MiscArmorClass.ToString();
            this.MagicBonusField.Text = Program.Character.Armor.MagicArmorClass.ToString();
        }

        private void FillWeaponList()
        {
            this.WeaponDataGrid.Items.Clear();

            foreach (var weapon in Program.Character.Weapons)
            {
                this.WeaponDataGrid.Items.Add(weapon);
            }
        }

        private void FillAmmoList()
        {
            this.AmmoDataGrid.Items.Clear();

            foreach (var ammo in Program.Character.Ammunitions)
            {
                this.AmmoDataGrid.Items.Add(ammo);
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            int index;

            if (this.AmmoDataGrid.SelectedItem != null)
            {
                var ammo = (Ammunition)this.AmmoDataGrid.SelectedItem;
                index = Program.Character.Ammunitions.IndexOf(ammo);

                if (index != 0)
                {
                    Utilities.Swap(Program.Character.Ammunitions, index, index - 1);
                    this.FillAmmoList();
                    this.AmmoDataGrid.SelectedIndex = index - 1;
                }
            }
            else if (this.WeaponDataGrid.SelectedItem != null)
            {
                var weapon = (Weapon)this.WeaponDataGrid.SelectedItem;
                index = Program.Character.Weapons.IndexOf(weapon);

                if (index != 0)
                {
                    Utilities.Swap(Program.Character.Weapons, index, index - 1);
                    this.FillWeaponList();
                    this.WeaponDataGrid.SelectedIndex = index - 1;
                }
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            int index;

            if (this.AmmoDataGrid.SelectedItem != null)
            {
                var ammo = (Ammunition)this.AmmoDataGrid.SelectedItem;
                index = Program.Character.Ammunitions.IndexOf(ammo);

                if (index != Program.Character.Ammunitions.Count - 1)
                {
                    Utilities.Swap(Program.Character.Ammunitions, index, index + 1);
                    this.FillAmmoList();
                    this.AmmoDataGrid.SelectedIndex = index + 1;
                }
            }
            else if (this.WeaponDataGrid.SelectedItem != null)
            {
                var weapon = (Weapon)this.WeaponDataGrid.SelectedItem;
                index = Program.Character.Weapons.IndexOf(weapon);

                if (index != Program.Character.Weapons.Count - 1)
                {
                    Utilities.Swap(Program.Character.Weapons, index, index + 1);
                    this.FillWeaponList();
                    this.WeaponDataGrid.SelectedIndex = index + 1;
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.WeaponDataGrid.UnselectAll();
            this.AmmoDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var popupButons = this.EquipmentPopupWindow.ShowPopup();

            switch (popupButons)
            {
                case PopupButtons.AddWeapon:
                    this.ModifyWeaponWindow.ShowAdd();
                    this.FillWeaponList();
                    break;
                case PopupButtons.AddAmmo:
                    this.ModifyAmmoWindow.ShowAdd();
                    this.FillAmmoList();
                    break;
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.AmmoDataGrid.SelectedItem != null)
            {
                this.ModifyAmmoWindow.ShowEdit((Ammunition)this.AmmoDataGrid.SelectedItem);
                this.FillAmmoList();
            }
            else if (this.WeaponDataGrid.SelectedItem != null)
            {
                this.ModifyWeaponWindow.ShowEdit((Weapon)this.WeaponDataGrid.SelectedItem);
                this.FillWeaponList();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.AmmoDataGrid.SelectedItem != null)
            {
                var ammo = (Ammunition)this.AmmoDataGrid.SelectedItem;
                Program.Character.Ammunitions.Remove(ammo);
                this.FillAmmoList();
            }
            else if (this.WeaponDataGrid.SelectedItem != null)
            {
                var weapon = (Weapon)this.WeaponDataGrid.SelectedItem;
                Program.Character.Weapons.Remove(weapon);
                this.FillWeaponList();
            }
        }

        private void AmmoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AmmoDataGrid.SelectedItem != null)
            {
                this.WeaponDataGrid.UnselectAll();
            }
        }

        private void WeaponDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem != null)
            {
                this.AmmoDataGrid.UnselectAll();
            }
        }

        private void EditDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            this.ModifyArmorWindow.ShowEdit();
            this.Draw();
        }

        private void WeaponDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Character.Weapons.Clear();

            foreach (var weapon in this.WeaponDataGrid.Items)
            {
                Program.Character.Weapons.Add(weapon as Weapon);
            }
        }

        private void AmmoDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Character.Ammunitions.Clear();

            foreach (var ammo in this.AmmoDataGrid.Items)
            {
                Program.Character.Ammunitions.Add(ammo as Ammunition);
            }
        }
    }
}
