using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Concierge.Presentation
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
            ArmorImage.Source = Constants.ToBitmapImage(Properties.Resources.armor_class_icon);
        }

        public void Draw()
        {
            FillWeaponList();
            FillAmmoList();

            ArmorClassField.Text = Program.Character.Armor.TotalArmorClass.ToString();
            Shield.Text = Program.Character.Armor.Shield;
            ShieldAC.Text = Program.Character.Armor.ShieldArmorClass.ToString();
            MiscBonus.Text = Program.Character.Armor.MiscArmorClass.ToString();
            MagicBonus.Text = Program.Character.Armor.MagicArmorClass.ToString();
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

        public double WeaponHeight
        {
            get
            {
                return SystemParameters.PrimaryScreenHeight - 500;
            }
        }

        public double AmmoHeight
        {
            get
            {
                return 300;
            }
        }

        public double ArmorHeight
        {
            get
            {
                return 300;
            }
        }

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

        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            //colby.bauer@mts.net
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
    }
}
