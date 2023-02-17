// <copyright file="AttacksPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Items;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Services;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for AttacksPage.xaml.
    /// </summary>
    public partial class AttacksPage : Page, IConciergePage
    {
        public AttacksPage()
        {
            this.InitializeComponent();
        }

        private delegate void DrawList();

        public bool HasEditableDataGrid => true;

        public ConciergePage ConciergePage => ConciergePage.Attacks;

        private List<Weapon> WeaponDisplayList => Program.CcsFile.Character.Weapons.Filter(this.SearchFilter.FilterText).ToList();

        public void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawWeaponList();
            this.DrawAmmoList();
            this.DrawStatusEffects();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is Ammunition ammunition)
            {
                var index = this.AmmoDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit<Ammunition>(
                    ammunition,
                    typeof(AmmunitionWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Attacks);
                this.DrawAmmoList();
                this.AmmoDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is Weapon weapon)
            {
                var index = this.WeaponDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit<Weapon>(
                    weapon,
                    typeof(AttacksWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Attacks);
                this.DrawWeaponList();
                this.WeaponDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is StatusEffect statusEffect)
            {
                var index = this.StatusEffectsDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit<StatusEffect>(
                    statusEffect,
                    typeof(StatusEffectsWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Attacks);
                this.DrawStatusEffects();
                this.StatusEffectsDataGrid.SetSelectedIndex(index);
            }
        }

        private static void ScrollDataGrid(ConciergeDataGrid dataGrid)
        {
            if (dataGrid.Items.Count > 0)
            {
                dataGrid.SelectedItem = dataGrid.Items[^1];
                dataGrid.UpdateLayout();
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
            }
        }

        private bool NextItem<T>(ConciergeDataGrid dataGrid, DrawList drawList, List<T> list, int limit, int increment)
        {
            var index = dataGrid.NextItem(list, limit, increment, this.ConciergePage);

            if (index != -1)
            {
                drawList();
                dataGrid.SetSelectedIndex(index);

                return true;
            }

            return false;
        }

        private void DrawWeaponList()
        {
            this.WeaponDataGrid.Items.Clear();

            foreach (var weapon in this.WeaponDisplayList)
            {
                this.WeaponDataGrid.Items.Add(weapon);
            }
        }

        private void DrawStatusEffects()
        {
            this.StatusEffectsDataGrid.Items.Clear();

            foreach (var effect in Program.CcsFile.Character.StatusEffects)
            {
                this.StatusEffectsDataGrid.Items.Add(effect);
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

        private void AmmoUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.AmmoDataGrid, this.DrawAmmoList, Program.CcsFile.Character.Ammunitions, 0, -1);
        }

        private void AmmonDownButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.AmmoDataGrid, this.DrawAmmoList, Program.CcsFile.Character.Ammunitions, Program.CcsFile.Character.Ammunitions.Count - 1, 1);
        }

        private void AttacksUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.WeaponDataGrid, this.DrawWeaponList, Program.CcsFile.Character.Weapons, 0, -1);
        }

        private void AttacksDownButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.WeaponDataGrid, this.DrawWeaponList, Program.CcsFile.Character.Weapons, Program.CcsFile.Character.Weapons.Count - 1, 1);
        }

        private void AmmoClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.AmmoDataGrid.UnselectAll();
        }

        private void AttacksClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.WeaponDataGrid.UnselectAll();
        }

        private void AmmoAddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<Ammunition>>(
                Program.CcsFile.Character.Ammunitions,
                typeof(AmmunitionWindow),
                this.Window_ApplyChanges,
                ConciergePage.Attacks);

            this.DrawAmmoList();
            if (added)
            {
                this.AmmoDataGrid.SetSelectedIndex(this.AmmoDataGrid.LastIndex);
            }
        }

        private void AttacksAddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<Weapon>>(
                Program.CcsFile.Character.Weapons,
                typeof(AttacksWindow),
                this.Window_ApplyChanges,
                ConciergePage.Attacks);

            this.DrawWeaponList();
            if (added)
            {
                this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.LastIndex);
            }
        }

        private void AmmoEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AmmoDataGrid.SelectedItem != null)
            {
                this.Edit(this.AmmoDataGrid.SelectedItem);
            }
        }

        private void AttacksEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem != null)
            {
                this.Edit(this.WeaponDataGrid.SelectedItem);
            }
        }

        private void AmmoUseButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AmmoDataGrid.SelectedItem is not Ammunition ammunition)
            {
                return;
            }

            var oldItem = ammunition.DeepCopy();
            var index = this.AmmoDataGrid.SelectedIndex;

            ammunition.Used++;
            this.DrawAmmoList();
            this.AmmoDataGrid.SetSelectedIndex(index);
            Program.UndoRedoService.AddCommand(new EditCommand<Ammunition>(ammunition, oldItem, this.ConciergePage));
        }

        private void AmmoDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AmmoDataGrid.SelectedItem is null)
            {
                return;
            }

            var ammo = (Ammunition)this.AmmoDataGrid.SelectedItem;
            var index = this.AmmoDataGrid.SelectedIndex;

            Program.UndoRedoService.AddCommand(new DeleteCommand<Ammunition>(Program.CcsFile.Character.Ammunitions, ammo, index, this.ConciergePage));
            Program.CcsFile.Character.Ammunitions.Remove(ammo);
            this.DrawAmmoList();
            this.AmmoDataGrid.SetSelectedIndex(index);

            Program.Modify();
        }

        private void AttacksDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem is null)
            {
                return;
            }

            var weapon = (Weapon)this.WeaponDataGrid.SelectedItem;
            var index = this.WeaponDataGrid.SelectedIndex;

            Program.UndoRedoService.AddCommand(new DeleteCommand<Weapon>(Program.CcsFile.Character.Weapons, weapon, index, this.ConciergePage));
            Program.CcsFile.Character.Weapons.Remove(weapon);
            this.DrawWeaponList();
            this.WeaponDataGrid.SetSelectedIndex(index);

            Program.Modify();
        }

        private void WeaponDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SortListFromDataGrid(this.WeaponDataGrid, Program.CcsFile.Character.Weapons, this.ConciergePage);
        }

        private void AmmoDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SortListFromDataGrid(this.AmmoDataGrid, Program.CcsFile.Character.Ammunitions, this.ConciergePage);
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(StatusEffectsWindow):
                    this.DrawStatusEffects();
                    ScrollDataGrid(this.StatusEffectsDataGrid);
                    break;
                case nameof(AmmunitionWindow):
                    this.DrawAmmoList();
                    ScrollDataGrid(this.AmmoDataGrid);
                    break;
                case nameof(AttacksWindow):
                    this.DrawWeaponList();
                    ScrollDataGrid(this.WeaponDataGrid);
                    break;
            }
        }

        private void ClearEffectsButton_Click(object sender, RoutedEventArgs e)
        {
            this.StatusEffectsDataGrid.UnselectAll();
        }

        private void DeleteEffectsButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.StatusEffectsDataGrid.SelectedItem == null)
            {
                return;
            }

            var effect = (StatusEffect)this.StatusEffectsDataGrid.SelectedItem;
            var index = this.StatusEffectsDataGrid.SelectedIndex;

            Program.UndoRedoService.AddCommand(new DeleteCommand<StatusEffect>(Program.CcsFile.Character.StatusEffects, effect, index, this.ConciergePage));
            Program.CcsFile.Character.StatusEffects.Remove(effect);
            this.DrawStatusEffects();
            this.StatusEffectsDataGrid.SetSelectedIndex(index);

            Program.Modify();
        }

        private void AddEffectsButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<StatusEffect>>(
                Program.CcsFile.Character.StatusEffects,
                typeof(StatusEffectsWindow),
                this.Window_ApplyChanges,
                ConciergePage.Attacks);
            this.DrawStatusEffects();

            if (added)
            {
                this.StatusEffectsDataGrid.SetSelectedIndex(this.StatusEffectsDataGrid.LastIndex);
            }
        }

        private void EditEffectsButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.StatusEffectsDataGrid.SelectedItem == null)
            {
                return;
            }

            this.Edit(this.StatusEffectsDataGrid.SelectedItem);
        }

        private void StatusEffectsDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SortListFromDataGrid(this.StatusEffectsDataGrid, Program.CcsFile.Character.StatusEffects, this.ConciergePage);
        }

        private void AttackDataGrid_Filtered(object sender, RoutedEventArgs e)
        {
            this.SearchFilter.SetButtonEnableState(this.AttacksUpButton);
            this.SearchFilter.SetButtonEnableState(this.AttacksDownButton);

            this.DrawWeaponList();
        }
    }
}
