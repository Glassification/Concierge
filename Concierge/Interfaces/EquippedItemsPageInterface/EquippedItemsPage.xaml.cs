// <copyright file="EquippedItemsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.EquippedItemsPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.InventoryPageInterface;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for EquippedItemsPage.xaml.
    /// </summary>
    public partial class EquippedItemsPage : Page, IConciergePage
    {
        private readonly ModifyEquippedItemsWindow modifyEquippedItemsWindow = new ();
        private readonly ModifyCharacterImageWindow modifyCharacterImageWindow = new ("768x1024 image ratio is recommended");
        private readonly ModifyInventoryWindow modifyInventoryWindow = new ();

        public EquippedItemsPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.modifyEquippedItemsWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyCharacterImageWindow.ApplyChanges += this.Window_ApplyChanges;
        }

        public static double EquippedItemsHeight => SystemParameters.PrimaryScreenHeight - 100;

        public static double EquippedItemsWidth => SystemParameters.PrimaryScreenWidth;

        public static double InnerEquippedItemsHeight => SystemParameters.PrimaryScreenHeight - 100 - 50;

        public static double InnerEquippedItemsWidth => SystemParameters.PrimaryScreenWidth - 50;

        public ConciergePage ConciergePage => ConciergePage.EquippedItems;

        private Inventory SelectedItem { get; set; }

        private int SelectedIndex { get; set; }

        private ConciergeDataGrid SelectedDataGrid { get; set; }

        public void Draw()
        {
            this.UsedAttunement.Text = $"{Program.CcsFile.Character.EquippedItems.Attuned}/{Constants.MaxAttunedItems}";

            ReadEquippedItems(Program.CcsFile.Character.EquippedItems.Head, this.HeadEquipmentDataGrid);
            ReadEquippedItems(Program.CcsFile.Character.EquippedItems.Torso, this.TorsoEquipmentDataGrid);
            ReadEquippedItems(Program.CcsFile.Character.EquippedItems.Hands, this.HandsEquipmentDataGrid);
            ReadEquippedItems(Program.CcsFile.Character.EquippedItems.Legs, this.LegsEquipmentDataGrid);
            ReadEquippedItems(Program.CcsFile.Character.EquippedItems.Feet, this.FeetEquipmentDataGrid);

            this.LoadImage();
        }

        private static void ReadEquippedItems(List<Inventory> items, ConciergeDataGrid dataGrid)
        {
            dataGrid.Items.Clear();

            foreach (var item in items)
            {
                dataGrid.Items.Add(item);
            }
        }

        private void LoadImage()
        {
            this.CharacterImage.Source = Program.CcsFile.Character.CharacterImage.ToImage();
            this.CharacterImage.Stretch = Program.CcsFile.Character.CharacterImage.Stretch;

            this.DefaultCharacterImage.Visibility = this.CharacterImage.Source == null ? Visibility.Visible : Visibility.Hidden;
        }

        private void EquipmentDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            var dataGrid = sender as ConciergeDataGrid;
            var equippedItems = Utilities.GetPropertyValue<List<Inventory>>(Program.CcsFile.Character.EquippedItems, dataGrid.Tag as string);

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
            this.SelectedIndex = dataGrid.SelectedIndex;

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
            if (this.SelectedDataGrid == null)
            {
                return;
            }

            var equippedItems = Utilities.GetPropertyValue<List<Inventory>>(Program.CcsFile.Character.EquippedItems, this.SelectedDataGrid.Tag as string);
            var index = this.SelectedDataGrid.NextItem(equippedItems, 0, -1);

            if (index != -1)
            {
                this.Draw();
                this.SelectedDataGrid.SelectedIndex = index;
                this.SelectedDataGrid.ScrollIntoView(this.SelectedDataGrid.SelectedItem);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedDataGrid == null)
            {
                return;
            }

            var equippedItems = Utilities.GetPropertyValue<List<Inventory>>(Program.CcsFile.Character.EquippedItems, this.SelectedDataGrid.Tag as string);
            var index = this.SelectedDataGrid.NextItem(equippedItems, equippedItems.Count - 1, 1);

            if (index != -1)
            {
                this.Draw();
                this.SelectedDataGrid.SelectedIndex = index;
                this.SelectedDataGrid.ScrollIntoView(this.SelectedDataGrid.SelectedItem);
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

            var index = this.SelectedIndex;
            Program.CcsFile.Character.EquippedItems.Dequip(
                this.SelectedItem,
                (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), this.SelectedDataGrid.Tag as string));

            this.Draw();
            Utilities.SetDataGridSelectedIndex(this.SelectedDataGrid, index);
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case "ModifyCharacterImageWindow":
                case "ModifyEquippedItemsWindow":
                    this.Draw();
                    break;
            }
        }

        private void ImageEditButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyCharacterImageWindow.ShowEdit(Program.CcsFile.Character.CharacterImage);
            this.Draw();
        }
    }
}
