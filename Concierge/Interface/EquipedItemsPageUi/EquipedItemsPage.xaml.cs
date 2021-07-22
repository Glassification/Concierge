// <copyright file="EquipedItemsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.EquipedItemsPageUi
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Characters.Enums;
    using Concierge.Characters.Items;
    using Concierge.Interface.Components;
    using Concierge.Interface.InventoryPageUi;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for EquipedItemsPage.xaml.
    /// </summary>
    public partial class EquipedItemsPage : Page
    {
        private readonly ModifyEquippedItemsWindow modifyEquippedItemsWindow = new ModifyEquippedItemsWindow();
        private readonly ModifyInventoryWindow modifyInventoryWindow = new ModifyInventoryWindow();

        public EquipedItemsPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public static double EquippedItemsHeight => SystemParameters.PrimaryScreenHeight - 100;

        public static double EquippedItemsWidth => SystemParameters.PrimaryScreenWidth;

        public static double InnerEquippedItemsHeight => SystemParameters.PrimaryScreenHeight - 100 - 50;

        public static double InnerEquippedItemsWidth => SystemParameters.PrimaryScreenWidth - 50;

        private Inventory SelectedItem { get; set; }

        private ConciergeDataGrid SelectedDataGrid { get; set; }

        public void Draw()
        {
            this.UsedAttunement.Text = $"{Program.CcsFile.Character.EquipedItems.Attuned}/{Constants.MaxAttunedItems}";

            ReadEquippedItems(Program.CcsFile.Character.EquipedItems.Head, this.HeadEquipmentDataGrid);
            ReadEquippedItems(Program.CcsFile.Character.EquipedItems.Torso, this.TorsoEquipmentDataGrid);
            ReadEquippedItems(Program.CcsFile.Character.EquipedItems.Hands, this.HandsEquipmentDataGrid);
            ReadEquippedItems(Program.CcsFile.Character.EquipedItems.Legs, this.LegsEquipmentDataGrid);
            ReadEquippedItems(Program.CcsFile.Character.EquipedItems.Feet, this.FeetEquipmentDataGrid);
        }

        private static void ReadEquippedItems(List<Inventory> items, ConciergeDataGrid dataGrid)
        {
            dataGrid.Items.Clear();

            foreach (var item in items)
            {
                dataGrid.Items.Add(item);
            }
        }

        private void EquipmentDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            var dataGrid = sender as ConciergeDataGrid;
            var equippedItems = Utilities.GetPropertyValue<List<Inventory>>(Program.CcsFile.Character.EquipedItems, dataGrid.Tag as string);

            equippedItems.Clear();
            foreach (var item in dataGrid.Items)
            {
                equippedItems.Add(item as Inventory);
            }
        }

        private void EquipmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = this.SelectedDataGrid = sender as ConciergeDataGrid;
            this.SelectedItem = dataGrid.SelectedItem as Inventory;

            if (this.SelectedItem == null)
            {
                return;
            }

            if (!(dataGrid.Tag as string).Equals(EquipmentSlot.Head.ToString()))
            {
                this.HeadEquipmentDataGrid.UnselectAll();
            }

            if (!(dataGrid.Tag as string).Equals(EquipmentSlot.Torso.ToString()))
            {
                this.TorsoEquipmentDataGrid.UnselectAll();
            }

            if (!(dataGrid.Tag as string).Equals(EquipmentSlot.Hands.ToString()))
            {
                this.HandsEquipmentDataGrid.UnselectAll();
            }

            if (!(dataGrid.Tag as string).Equals(EquipmentSlot.Legs.ToString()))
            {
                this.LegsEquipmentDataGrid.UnselectAll();
            }

            if (!(dataGrid.Tag as string).Equals(EquipmentSlot.Feet.ToString()))
            {
                this.FeetEquipmentDataGrid.UnselectAll();
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem == null)
            {
                return;
            }

            Program.Modify();

            var dataGrid = this.SelectedDataGrid;
            var equippedItems = Utilities.GetPropertyValue<List<Inventory>>(Program.CcsFile.Character.EquipedItems, dataGrid.Tag as string);
            var index = equippedItems.IndexOf(this.SelectedItem);

            if (index != 0)
            {
                Utilities.Swap(equippedItems, index, index - 1);
                this.Draw();
                dataGrid.SelectedIndex = index - 1;
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem == null)
            {
                return;
            }

            Program.Modify();

            var dataGrid = this.SelectedDataGrid;
            var equippedItems = Utilities.GetPropertyValue<List<Inventory>>(Program.CcsFile.Character.EquipedItems, dataGrid.Tag as string);
            var index = equippedItems.IndexOf(this.SelectedItem);

            if (index != equippedItems.Count - 1)
            {
                Utilities.Swap(equippedItems, index, index + 1);
                this.Draw();
                dataGrid.SelectedIndex = index + 1;
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.HeadEquipmentDataGrid.UnselectAll();
            this.TorsoEquipmentDataGrid.UnselectAll();
            this.HandsEquipmentDataGrid.UnselectAll();
            this.LegsEquipmentDataGrid.UnselectAll();
            this.FeetEquipmentDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            this.modifyEquippedItemsWindow.ShowAdd();
            this.Draw();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem == null)
            {
                return;
            }

            this.modifyInventoryWindow.ShowEdit(this.SelectedItem, true);
            this.Draw();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem == null)
            {
                return;
            }

            Program.Modify();

            Program.CcsFile.Character.EquipedItems.Dequip(
                this.SelectedItem,
                (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), this.SelectedDataGrid.Tag as string));

            this.Draw();
        }
    }
}
