using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Concierge.Presentation.EquipmentPageUi
{
    /// <summary>
    /// Interaction logic for EquipmentPage.xaml
    /// </summary>
    public partial class EquipmentPage : Page
    {
        public EquipmentPage()
        {
            InitializeComponent();
            DataContext = this;
            ModifyArmorWindow = new ModifyArmorWindow();
            EquipmentPopupWindow = new EquipmentPopupWindow();
            ModifyWeaponWindow = new ModifyWeaponWindow();
            ModifyAmmoWindow = new ModifyAmmoWindow();
        }

        public void Draw()
        {
            FillWeaponList();
            FillAmmoList();

            ArmorClassField.Text = Program.Character.Armor.TotalArmorClass.ToString();
            ArmorWornField.Text = Program.Character.Armor.Equiped;
            ArmorTypeField.Text = Program.Character.Armor.Type.ToString();
            ArmorStealthField.Text = Program.Character.Armor.Stealth.ToString();
            ShieldWornField.Text = Program.Character.Armor.Shield;
            ShieldAcField.Text = Program.Character.Armor.ShieldArmorClass.ToString();
            MiscBonusField.Text = Program.Character.Armor.MiscArmorClass.ToString();
            MagicBonusField.Text = Program.Character.Armor.MagicArmorClass.ToString();
        }

        private void FillWeaponList()
        {
            WeaponDataGrid.Items.Clear();

            foreach (var weapon in Program.Character.Weapons)
            {

                WeaponDataGrid.Items.Add(weapon);
            }
        }

        private void FillAmmoList()
        {
            AmmoDataGrid.Items.Clear();

            foreach (var ammo in Program.Character.Ammunitions)
            {

                AmmoDataGrid.Items.Add(ammo);
            }
        }

        private ModifyArmorWindow ModifyArmorWindow { get; }
        private ModifyWeaponWindow ModifyWeaponWindow { get; }
        private ModifyAmmoWindow ModifyAmmoWindow { get; }
        private EquipmentPopupWindow EquipmentPopupWindow { get; }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            int index;

            if (AmmoDataGrid.SelectedItem != null)
            {
                Ammunition ammo = (Ammunition)AmmoDataGrid.SelectedItem;
                index = Program.Character.Ammunitions.IndexOf(ammo);

                if (index != 0)
                {
                    Constants.Swap(Program.Character.Ammunitions, index, index - 1);
                    FillAmmoList();
                    AmmoDataGrid.SelectedIndex = index - 1;
                }
            }
            else if (WeaponDataGrid.SelectedItem != null)
            {
                Weapon weapon = (Weapon)WeaponDataGrid.SelectedItem;
                index = Program.Character.Weapons.IndexOf(weapon);

                if (index != 0)
                {
                    Constants.Swap(Program.Character.Weapons, index, index - 1);
                    FillWeaponList();
                    WeaponDataGrid.SelectedIndex = index - 1;
                }
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            int index;

            if (AmmoDataGrid.SelectedItem != null)
            {
                Ammunition ammo = (Ammunition)AmmoDataGrid.SelectedItem;
                index = Program.Character.Ammunitions.IndexOf(ammo);

                if (index != Program.Character.Ammunitions.Count - 1)
                {
                    Constants.Swap(Program.Character.Ammunitions, index, index + 1);
                    FillAmmoList();
                    AmmoDataGrid.SelectedIndex = index + 1;
                }
            }
            else if (WeaponDataGrid.SelectedItem != null)
            {
                Weapon weapon = (Weapon)WeaponDataGrid.SelectedItem;
                index = Program.Character.Weapons.IndexOf(weapon);

                if (index != Program.Character.Weapons.Count - 1)
                {
                    Constants.Swap(Program.Character.Weapons, index, index + 1);
                    FillWeaponList();
                    WeaponDataGrid.SelectedIndex = index + 1;
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            WeaponDataGrid.UnselectAll();
            AmmoDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Constants.PopupButtons popupButons;

            popupButons = EquipmentPopupWindow.ShowPopup();

            switch (popupButons)
            {
                case Constants.PopupButtons.AddWeapon:
                    ModifyWeaponWindow.ShowAdd();
                    FillWeaponList();
                    break;
                case Constants.PopupButtons.AddAmmo:
                    ModifyAmmoWindow.ShowAdd();
                    FillAmmoList();
                    break;
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (AmmoDataGrid.SelectedItem != null)
            {
                ModifyAmmoWindow.ShowEdit((Ammunition)AmmoDataGrid.SelectedItem);
                FillAmmoList();
            }
            else if (WeaponDataGrid.SelectedItem != null)
            {
                ModifyWeaponWindow.ShowEdit((Weapon)WeaponDataGrid.SelectedItem);
                FillWeaponList();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (AmmoDataGrid.SelectedItem != null)
            {
                Ammunition ammo = (Ammunition)AmmoDataGrid.SelectedItem;
                Program.Character.Ammunitions.Remove(ammo);
                FillAmmoList();
            }
            else if (WeaponDataGrid.SelectedItem != null)
            {
                Weapon weapon = (Weapon)WeaponDataGrid.SelectedItem;
                Program.Character.Weapons.Remove(weapon);
                FillWeaponList();
            }
        }

        private void AmmoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AmmoDataGrid.SelectedItem != null)
            {
                WeaponDataGrid.UnselectAll();
            }
        }

        private void WeaponDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeaponDataGrid.SelectedItem != null)
            {
                AmmoDataGrid.UnselectAll();
            }
        }

        private void EditDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyArmorWindow.ShowEdit();
            Draw();
        }
    }
}
