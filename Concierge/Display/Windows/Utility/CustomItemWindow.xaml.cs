// <copyright file="CustomItemWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows.Utility
{
    using System;
    using System.Windows;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Equipable;
    using Concierge.Character.Spellcasting;
    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Concierge.Data;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for CustomItemWindow.xaml.
    /// </summary>
    public partial class CustomItemWindow : ConciergeWindow
    {
        public CustomItemWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
        }

        public override string HeaderText => "Custom Items";

        public override string WindowName => nameof(CustomItemWindow);

        public override object? ShowWindow()
        {
            this.Draw();
            this.ShowConciergeWindow();

            return null;
        }

        public override void ShowEdit<T>(T item)
        {
            Type? type = null;

            if (item is Ability)
            {
                type = typeof(AbilitiesWindow);
            }
            else if (item is Ammunition)
            {
                type = typeof(AmmunitionWindow);
            }
            else if (item is ClassResource)
            {
                type = typeof(ClassResourceWindow);
            }
            else if (item is CustomColor color)
            {
                var result = ConciergeWindowService.ShowColorWindow(typeof(CustomColorWindow), color);
                if (result.IsValid)
                {
                    color.ShallowCopy(result);
                    this.Draw();
                }

                return;
            }
            else if (item is Inventory)
            {
                ConciergeWindowService.ShowEdit(
                    item,
                    false,
                    typeof(InventoryWindow),
                    this.Window_ApplyChanges,
                    Enums.ConciergePage.None);
                this.Draw();
                return;
            }
            else if (item is Language)
            {
                type = typeof(LanguagesWindow);
            }
            else if (item is MagicClass)
            {
                type = typeof(MagicClassWindow);
            }
            else if (item is Proficiency)
            {
                type = typeof(ProficiencyWindow);
            }
            else if (item is Spell)
            {
                type = typeof(SpellWindow);
            }
            else if (item is StatusEffect)
            {
                type = typeof(StatusEffectsWindow);
            }
            else if (item is Weapon)
            {
                type = typeof(AttacksWindow);
            }

            if (type is null)
            {
                return;
            }

            ConciergeWindowService.ShowEdit(
                item,
                type,
                this.Window_ApplyChanges,
                Enums.ConciergePage.None);
            this.Draw();
        }

        public void Draw()
        {
            var customItems = Program.CustomItemService.GetCustomItems();
            var customColors = Program.CustomColorService.CustomColors;
            var count = customItems.Count + customColors.Count;

            this.ItemTotalField.Text = $"({count} Item{(count == 1 ? string.Empty : "s")})";
            this.CustomItemsDataGrid.Items.Clear();
            foreach (var item in customItems)
            {
                this.CustomItemsDataGrid.Items.Add(item);
            }

            foreach (var color in customColors)
            {
                this.CustomItemsDataGrid.Items.Add(color);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.CustomItemsDataGrid.UnselectAll();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.CustomItemsDataGrid.SelectedItem is CustomColor color)
            {
                this.ShowEdit(color);
                Program.CustomColorService.UpdateCustomColors();
                this.Draw();
            }
            else
            if (this.CustomItemsDataGrid.SelectedItem is IUnique unique)
            {
                this.ShowEdit(unique);
                Program.CustomItemService.UpdateCustomItem();
                this.Draw();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.CustomItemsDataGrid.SelectedItem is not null)
            {
                var result = ConciergeMessageBox.Show(
                    "Are you sure you wish to delete the item? This action cannot be undone.",
                    "Confirm Delete",
                    ConciergeWindowButtons.YesNo,
                    ConciergeWindowIcons.Question);

                if (result != ConciergeWindowResult.Yes)
                {
                    return;
                }
            }

            if (this.CustomItemsDataGrid.SelectedItem is CustomColor color)
            {
                Program.CustomColorService.RemoveCustomColor(color);
                this.Draw();
            }
            else if (this.CustomItemsDataGrid.SelectedItem is IUnique unique)
            {
                Program.CustomItemService.RemoveCustomItem(unique);
                this.Draw();
            }
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            this.Draw();
        }
    }
}
