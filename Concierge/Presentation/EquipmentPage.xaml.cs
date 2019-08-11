using Concierge.Characters.Collections;
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
        }

        public void Draw()
        {
            FillWeaponList();
            FillAmmoList();

            ArmorWorn.Text = Program.Character.Armor.Equiped;
            ArmorType.Text = Program.Character.Armor.Type.ToString();
            AC.Text = Program.Character.Armor.ArmorClass.ToString(); ;
            Stealth.Text = Program.Character.Armor.Stealth.ToString();
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
            
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
