// <copyright file="EquipmentPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.EquipmentPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Characters.Enums;
    using Concierge.Characters.Items;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for EquipmentPage.xaml.
    /// </summary>
    public partial class EquipmentPage : Page
    {
        private readonly ModifyArmorWindow modifyArmorWindow = new ModifyArmorWindow();
        private readonly ModifyWeaponWindow modifyWeaponWindow = new ModifyWeaponWindow();
        private readonly ModifyAmmoWindow modifyAmmoWindow = new ModifyAmmoWindow();
        private readonly EquipmentPopupWindow equipmentPopupWindow = new EquipmentPopupWindow();

        public EquipmentPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.modifyAmmoWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyWeaponWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyArmorWindow.ApplyChanges += this.Window_ApplyChanges;
        }

        public void Draw()
        {
            this.DrawWeaponList();
            this.DrawAmmoList();
            this.DrawArmor();
        }

        private void DrawArmor()
        {
            var armor = Program.CcsFile.Character.Armor;

            this.ArmorClassField.Text = armor.TotalArmorClass.ToString();
            this.ArmorWornField.Text = armor.Equiped;
            this.ArmorTypeField.Text = armor.Type.ToString();
            this.ArmorStealthField.Text = armor.Stealth.ToString();
            this.ShieldWornField.Text = armor.Shield;
            this.ShieldAcField.Text = armor.ShieldArmorClass.ToString();
            this.MiscBonusField.Text = armor.MiscArmorClass.ToString();
            this.MagicBonusField.Text = armor.MagicArmorClass.ToString();
        }

        private void DrawWeaponList()
        {
            this.WeaponDataGrid.Items.Clear();

            foreach (var weapon in Program.CcsFile.Character.Weapons)
            {
                this.WeaponDataGrid.Items.Add(weapon);
            }
        }

        private void ScrollWeapons()
        {
            if (this.WeaponDataGrid.Items.Count > 0)
            {
                this.WeaponDataGrid.SelectedItem = this.WeaponDataGrid.Items[this.WeaponDataGrid.Items.Count - 1];
                this.WeaponDataGrid.UpdateLayout();
                this.WeaponDataGrid.ScrollIntoView(this.WeaponDataGrid.SelectedItem);
            }
        }

        private void DrawAmmoList()
        {
            this.AmmoDataGrid.Items.Clear();

            foreach (var ammo in Program.CcsFile.Character.Ammunitions)
            {
                this.AmmoDataGrid.Items.Add(ammo);
            }
        }

        private void ScrollAmmo()
        {
            if (this.AmmoDataGrid.Items.Count > 0)
            {
                this.AmmoDataGrid.SelectedItem = this.AmmoDataGrid.Items[this.AmmoDataGrid.Items.Count - 1];
                this.AmmoDataGrid.UpdateLayout();
                this.AmmoDataGrid.ScrollIntoView(this.AmmoDataGrid.SelectedItem);
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (this.AmmoDataGrid.SelectedItem != null)
            {
                Program.Modify();
                ConciergeSound.TapNavigation();

                var ammo = (Ammunition)this.AmmoDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.Ammunitions.IndexOf(ammo);

                if (index != 0)
                {
                    Utilities.Swap(Program.CcsFile.Character.Ammunitions, index, index - 1);
                    this.DrawAmmoList();
                    this.AmmoDataGrid.SelectedIndex = index - 1;
                }
            }
            else if (this.WeaponDataGrid.SelectedItem != null)
            {
                Program.Modify();
                ConciergeSound.TapNavigation();

                var weapon = (Weapon)this.WeaponDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.Weapons.IndexOf(weapon);

                if (index != 0)
                {
                    Utilities.Swap(Program.CcsFile.Character.Weapons, index, index - 1);
                    this.DrawWeaponList();
                    this.WeaponDataGrid.SelectedIndex = index - 1;
                }
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (this.AmmoDataGrid.SelectedItem != null)
            {
                Program.Modify();
                ConciergeSound.TapNavigation();

                var ammo = (Ammunition)this.AmmoDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.Ammunitions.IndexOf(ammo);

                if (index != Program.CcsFile.Character.Ammunitions.Count - 1)
                {
                    Utilities.Swap(Program.CcsFile.Character.Ammunitions, index, index + 1);
                    this.DrawAmmoList();
                    this.AmmoDataGrid.SelectedIndex = index + 1;
                }
            }
            else if (this.WeaponDataGrid.SelectedItem != null)
            {
                Program.Modify();
                ConciergeSound.TapNavigation();

                var weapon = (Weapon)this.WeaponDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.Weapons.IndexOf(weapon);

                if (index != Program.CcsFile.Character.Weapons.Count - 1)
                {
                    Utilities.Swap(Program.CcsFile.Character.Weapons, index, index + 1);
                    this.DrawWeaponList();
                    this.WeaponDataGrid.SelectedIndex = index + 1;
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.WeaponDataGrid.UnselectAll();
            this.AmmoDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            var popupButons = this.equipmentPopupWindow.ShowPopup();

            switch (popupButons)
            {
                case PopupButtons.AddWeapon:
                    this.modifyWeaponWindow.ShowAdd(Program.CcsFile.Character.Weapons);
                    this.DrawWeaponList();
                    break;
                case PopupButtons.AddAmmo:
                    this.modifyAmmoWindow.ShowAdd(Program.CcsFile.Character.Ammunitions);
                    this.DrawAmmoList();
                    break;
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.AmmoDataGrid.SelectedItem != null)
            {
                ConciergeSound.TapNavigation();
                this.modifyAmmoWindow.ShowEdit((Ammunition)this.AmmoDataGrid.SelectedItem);
                this.DrawAmmoList();
            }
            else if (this.WeaponDataGrid.SelectedItem != null)
            {
                ConciergeSound.TapNavigation();
                this.modifyWeaponWindow.ShowEdit((Weapon)this.WeaponDataGrid.SelectedItem);
                this.DrawWeaponList();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.AmmoDataGrid.SelectedItem != null)
            {
                Program.Modify();
                ConciergeSound.TapNavigation();

                var ammo = (Ammunition)this.AmmoDataGrid.SelectedItem;
                Program.CcsFile.Character.Ammunitions.Remove(ammo);
                this.DrawAmmoList();
            }
            else if (this.WeaponDataGrid.SelectedItem != null)
            {
                Program.Modify();
                ConciergeSound.TapNavigation();

                var weapon = (Weapon)this.WeaponDataGrid.SelectedItem;
                Program.CcsFile.Character.Weapons.Remove(weapon);
                this.DrawWeaponList();
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
            ConciergeSound.TapNavigation();
            this.modifyArmorWindow.ShowEdit(Program.CcsFile.Character.Armor);
            this.Draw();
        }

        private void WeaponDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            Program.CcsFile.Character.Weapons.Clear();

            foreach (var weapon in this.WeaponDataGrid.Items)
            {
                Program.CcsFile.Character.Weapons.Add(weapon as Weapon);
            }
        }

        private void AmmoDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            Program.CcsFile.Character.Ammunitions.Clear();

            foreach (var ammo in this.AmmoDataGrid.Items)
            {
                Program.CcsFile.Character.Ammunitions.Add(ammo as Ammunition);
            }
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case "ModifyArmorWindow":
                    this.DrawArmor();
                    break;
                case "ModifyAmmoWindow":
                    this.DrawAmmoList();
                    this.ScrollAmmo();
                    break;
                case "ModifyWeaponWindow":
                    this.DrawWeaponList();
                    this.ScrollWeapons();
                    break;
            }
        }
    }
}
