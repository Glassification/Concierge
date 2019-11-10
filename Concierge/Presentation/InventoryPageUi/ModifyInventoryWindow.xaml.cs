using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Concierge.Presentation.InventoryPageUi
{
    /// <summary>
    /// Interaction logic for ModifyInventoryWindow.xaml
    /// </summary>
    public partial class ModifyInventoryWindow : Window
    {

        #region Constructor

        public ModifyInventoryWindow()
        {
            InitializeComponent();
            NameComboBox.ItemsSource = Constants.Inventories;
        }

        #endregion

        #region Methods

        public void ShowEdit(Inventory inventory)
        {
            HeaderTextBlock.Text = "Edit Item";
            SelectedItemId = inventory.ID;
            Editing = true;
            FillFields(inventory);
            ButtonApply.Visibility = Visibility.Collapsed;

            ShowDialog();
        }

        public void ShowAdd()
        {
            HeaderTextBlock.Text = "Add Item";
            Editing = false;
            ClearFields();
            ButtonApply.Visibility = Visibility.Visible;

            ShowDialog();
        }

        private void FillFields(Inventory inventory)
        {
            NameComboBox.Text = inventory.Name;
            AmountUpDown.Value = inventory.Amount;
            WeightUpDown.Value = inventory.Weight;
            NotesTextBox.Text = inventory.Note;
        }

        private void ClearFields()
        {
            NameComboBox.Text = string.Empty;
            AmountUpDown.Value = 0;
            WeightUpDown.Value = 0.0;
            NotesTextBox.Text = string.Empty;
        }

        private Inventory ToInventory()
        {
            Inventory inventory = new Inventory()
            {
                Name = NameComboBox.Text,
                Amount = AmountUpDown.Value ?? 0,
                Weight = WeightUpDown.Value ?? 0.0,
                Note = NotesTextBox.Text
            };

            return inventory;
        }

        private void UpdateInventory(Inventory inventory)
        {
            inventory.Name = NameComboBox.Text;
            inventory.Amount = AmountUpDown.Value ?? 0;
            inventory.Weight = WeightUpDown.Value ?? 0.0;
            inventory.Note = NotesTextBox.Text;
        }

        #endregion

        #region Accessors

        private bool Editing { get; set; }
        private Guid SelectedItemId { get; set; }

        #endregion

        #region Events

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Hide();
                    break;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Inventories.Add(ToInventory());
            ClearFields();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (Editing)
            {
                UpdateInventory(Program.Character.GetInventoryById(SelectedItemId));
            }
            else
            {
                Program.Character.Inventories.Add(ToInventory());
            }

            Hide();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NameComboBox.SelectedItem != null)
            {
                FillFields(NameComboBox.SelectedItem as Inventory);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        #endregion

    }
}
