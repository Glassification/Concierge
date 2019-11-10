using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Concierge.Presentation.EquipmentPageUi
{
    /// <summary>
    /// Interaction logic for ModifyAmmoWindow.xaml
    /// </summary>
    public partial class ModifyAmmoWindow : Window
    {
        public ModifyAmmoWindow()
        {
            InitializeComponent();
            NameComboBox.ItemsSource = Constants.Ammunitions;
            DamageTypeComboBox.ItemsSource = Enum.GetValues(typeof(Constants.DamageTypes)).Cast<Constants.DamageTypes>();
        }

        public void ShowAdd()
        {
            HeaderTextBlock.Text = "Add Ammunition";
            Editing = false;
            ApplyButton.Visibility = Visibility.Visible;
            ClearFields();

            ShowDialog();
        }

        public void ShowEdit(Ammunition ammunition)
        {
            HeaderTextBlock.Text = "Edit Ammunition";
            SelectedAmmoId = ammunition.ID;
            Editing = true;
            ApplyButton.Visibility = Visibility.Collapsed;
            FillFields(ammunition);

            ShowDialog();
        }

        private void FillFields(Ammunition ammunition)
        {
            NameComboBox.Text = ammunition.Name;
            QuantityUpDown.Value = ammunition.Quantity;
            BonusTextBox.Text = ammunition.Bonus;
            DamageTypeComboBox.Text = ammunition.DamageType.ToString();
            UsedUpDown.Value = ammunition.Used;
        }

        private void ClearFields()
        {
            NameComboBox.Text = string.Empty;
            QuantityUpDown.Value = 0;
            BonusTextBox.Text = string.Empty;
            DamageTypeComboBox.Text = string.Empty;
            UsedUpDown.Value = 0;
        }

        private void UpdateAmmunition(Ammunition ammunition)
        {
            ammunition.Name = NameComboBox.Text;
            ammunition.Quantity = QuantityUpDown.Value ?? 0;
            ammunition.Bonus = BonusTextBox.Text;
            ammunition.DamageType = (Constants.DamageTypes)Enum.Parse(typeof(Constants.DamageTypes), DamageTypeComboBox.Text);
            ammunition.Used = UsedUpDown.Value ?? 0;
        }

        private Ammunition ToAmmunition()
        {
            Ammunition ammunition = new Ammunition()
            {
                Name = NameComboBox.Text,
                Quantity = QuantityUpDown.Value ?? 0,
                Bonus = BonusTextBox.Text,
                DamageType = (Constants.DamageTypes)Enum.Parse(typeof(Constants.DamageTypes), DamageTypeComboBox.Text),
                Used = UsedUpDown.Value ?? 0
            };

            return ammunition;
        }

        private bool Editing { get; set; }
        private Guid SelectedAmmoId { get; set; }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Editing)
            {
                UpdateAmmunition(Program.Character.GetAmmunitionById(SelectedAmmoId));
            }
            else
            {
                Program.Character.Ammunitions.Add(ToAmmunition());
            }

            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Ammunitions.Add(ToAmmunition());
            ClearFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NameComboBox.SelectedItem != null)
            {
                FillFields(NameComboBox.SelectedItem as Ammunition);
            }
        }
    }
}
