// <copyright file="AttackDefensePage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.AttackDefensePageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Services;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for EquipmentPage.xaml.
    /// </summary>
    public partial class AttackDefensePage : Page, IConciergePage
    {
        public AttackDefensePage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private delegate void DrawList();

        public ConciergePage ConciergePage => ConciergePage.AttackDefense;

        public void Draw()
        {
            this.DrawWeaponList();
            this.DrawAmmoList();
            this.DrawArmor();
            this.DrawStatusEffects();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is Ammunition)
            {
                var index = this.AmmoDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit<Ammunition>(
                    itemToEdit as Ammunition,
                    typeof(ModifyAmmoWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.AttackDefense);
                this.DrawAmmoList();
                this.AmmoDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is Weapon)
            {
                var index = this.WeaponDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit<Weapon>(
                    itemToEdit as Weapon,
                    typeof(ModifyAttackWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.AttackDefense);
                this.DrawWeaponList();
                this.WeaponDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is StatusEffect)
            {
                var index = this.StatusEffectsDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit<StatusEffect>(
                    itemToEdit as StatusEffect,
                    typeof(ModifyStatusEffectsWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.AttackDefense);
                this.DrawStatusEffects();
                this.StatusEffectsDataGrid.SetSelectedIndex(index);
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

        private void DrawArmor()
        {
            var armor = Program.CcsFile.Character.Armor;

            this.AcField.Text = armor.TotalArmorClass.ToString();
            this.ArmorWornField.Text = armor.Equiped;
            this.ArmorTypeField.Text = armor.Type.ToString();
            this.ArmorStealthField.Text = armor.Stealth.ToString();
            this.ShieldWornField.Text = armor.Shield;
            this.ShieldAcField.Text = armor.ShieldArmorClass.ToString();
            this.MiscAcField.Text = armor.MiscArmorClass.ToString();
            this.MagicAcField.Text = armor.MagicArmorClass.ToString();
        }

        private void DrawWeaponList()
        {
            this.WeaponDataGrid.Items.Clear();

            foreach (var weapon in Program.CcsFile.Character.Weapons)
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

        private void ScrollDataGrid(ConciergeDataGrid dataGrid)
        {
            if (dataGrid.Items.Count > 0)
            {
                dataGrid.SelectedItem = dataGrid.Items[^1];
                dataGrid.UpdateLayout();
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (!this.NextItem(this.AmmoDataGrid, this.DrawAmmoList, Program.CcsFile.Character.Ammunitions, 0, -1))
            {
                this.NextItem(this.WeaponDataGrid, this.DrawWeaponList, Program.CcsFile.Character.Weapons, 0, -1);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (!this.NextItem(this.AmmoDataGrid, this.DrawAmmoList, Program.CcsFile.Character.Ammunitions, Program.CcsFile.Character.Ammunitions.Count - 1, 1))
            {
                this.NextItem(this.WeaponDataGrid, this.DrawWeaponList, Program.CcsFile.Character.Weapons, Program.CcsFile.Character.Weapons.Count - 1, 1);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.WeaponDataGrid.UnselectAll();
            this.AmmoDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var popupButons = ConciergeWindowService.ShowPopup(typeof(AttacksPopupWindow));
            bool added;

            switch (popupButons)
            {
                case PopupButtons.AddWeapon:
                    added = ConciergeWindowService.ShowAdd<List<Weapon>>(
                        Program.CcsFile.Character.Weapons,
                        typeof(ModifyAttackWindow),
                        this.Window_ApplyChanges,
                        ConciergePage.AttackDefense);
                    this.DrawWeaponList();
                    if (added)
                    {
                        this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.LastIndex);
                    }

                    break;
                case PopupButtons.AddAmmo:
                    added = ConciergeWindowService.ShowAdd<List<Ammunition>>(
                        Program.CcsFile.Character.Ammunitions,
                        typeof(ModifyAmmoWindow),
                        this.Window_ApplyChanges,
                        ConciergePage.AttackDefense);
                    this.DrawAmmoList();
                    if (added)
                    {
                        this.AmmoDataGrid.SetSelectedIndex(this.AmmoDataGrid.LastIndex);
                    }

                    break;
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.AmmoDataGrid.SelectedItem != null)
            {
                this.Edit(this.AmmoDataGrid.SelectedItem);
            }
            else if (this.WeaponDataGrid.SelectedItem != null)
            {
                this.Edit(this.WeaponDataGrid.SelectedItem);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.AmmoDataGrid.SelectedItem != null)
            {
                var ammo = (Ammunition)this.AmmoDataGrid.SelectedItem;
                var index = this.AmmoDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Ammunition>(Program.CcsFile.Character.Ammunitions, ammo, index, this.ConciergePage));
                Program.CcsFile.Character.Ammunitions.Remove(ammo);
                this.DrawAmmoList();
                this.AmmoDataGrid.SetSelectedIndex(index);

                Program.Modify();
            }
            else if (this.WeaponDataGrid.SelectedItem != null)
            {
                var weapon = (Weapon)this.WeaponDataGrid.SelectedItem;
                var index = this.WeaponDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Weapon>(Program.CcsFile.Character.Weapons, weapon, index, this.ConciergePage));
                Program.CcsFile.Character.Weapons.Remove(weapon);
                this.DrawWeaponList();
                this.WeaponDataGrid.SetSelectedIndex(index);

                Program.Modify();
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
            ConciergeWindowService.ShowEdit<Armor>(
                Program.CcsFile.Character.Armor,
                typeof(ModifyArmorWindow),
                this.Window_ApplyChanges,
                ConciergePage.AttackDefense);
            this.Draw();
        }

        private void WeaponDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Utilities.SortListFromDataGrid(this.WeaponDataGrid, Program.CcsFile.Character.Weapons, this.ConciergePage);
        }

        private void AmmoDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Utilities.SortListFromDataGrid(this.AmmoDataGrid, Program.CcsFile.Character.Ammunitions, this.ConciergePage);
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case "ModifyArmorWindow":
                    this.DrawArmor();
                    break;
                case "ModifyStatusEffectsWindow":
                    this.DrawStatusEffects();
                    this.ScrollDataGrid(this.StatusEffectsDataGrid);
                    break;
                case "ModifyAmmoWindow":
                    this.DrawAmmoList();
                    this.ScrollDataGrid(this.AmmoDataGrid);
                    break;
                case "ModifyAttackWindow":
                    this.DrawWeaponList();
                    this.ScrollDataGrid(this.WeaponDataGrid);
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
                typeof(ModifyStatusEffectsWindow),
                this.Window_ApplyChanges,
                ConciergePage.AttackDefense);
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
            Utilities.SortListFromDataGrid(this.StatusEffectsDataGrid, Program.CcsFile.Character.StatusEffects, this.ConciergePage);
        }
    }
}