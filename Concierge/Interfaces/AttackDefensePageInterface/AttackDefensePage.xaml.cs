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
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for EquipmentPage.xaml.
    /// </summary>
    public partial class AttackDefensePage : Page, IConciergePage
    {
        private readonly ModifyArmorWindow modifyArmorWindow = new ();
        private readonly ModifyAttackWindow modifyAttackWindow = new ();
        private readonly ModifyAmmoWindow modifyAmmoWindow = new ();
        private readonly AttacksPopupWindow attacksPopupWindow = new ();
        private readonly ModifyStatusEffectsWindow modifyStatusEffectsWindow = new ();

        public AttackDefensePage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.modifyAmmoWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyAttackWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyArmorWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyStatusEffectsWindow.ApplyChanges += this.Window_ApplyChanges;
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
                this.modifyAmmoWindow.ShowEdit(itemToEdit as Ammunition);
                this.DrawAmmoList();
                this.AmmoDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is Weapon)
            {
                var index = this.WeaponDataGrid.SelectedIndex;
                this.modifyAttackWindow.ShowEdit(itemToEdit as Weapon);
                this.DrawWeaponList();
                this.WeaponDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is StatusEffect)
            {
                var index = this.StatusEffectsDataGrid.SelectedIndex;
                this.modifyStatusEffectsWindow.ShowEdit(itemToEdit as StatusEffect);
                this.DrawStatusEffects();
                this.StatusEffectsDataGrid.SetSelectedIndex(index);
            }
        }

        private static bool NextItem<T>(ConciergeDataGrid dataGrid, DrawList drawList, List<T> list, int limit, int increment)
        {
            var index = dataGrid.NextItem(list, limit, increment);

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
            if (!NextItem(this.AmmoDataGrid, this.DrawAmmoList, Program.CcsFile.Character.Ammunitions, 0, -1))
            {
                NextItem(this.WeaponDataGrid, this.DrawWeaponList, Program.CcsFile.Character.Weapons, 0, -1);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (!NextItem(this.AmmoDataGrid, this.DrawAmmoList, Program.CcsFile.Character.Ammunitions, Program.CcsFile.Character.Ammunitions.Count - 1, 1))
            {
                NextItem(this.WeaponDataGrid, this.DrawWeaponList, Program.CcsFile.Character.Weapons, Program.CcsFile.Character.Weapons.Count - 1, 1);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.WeaponDataGrid.UnselectAll();
            this.AmmoDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var popupButons = this.attacksPopupWindow.ShowPopup();

            switch (popupButons)
            {
                case PopupButtons.AddWeapon:
                    this.modifyAttackWindow.ShowAdd(Program.CcsFile.Character.Weapons);
                    this.DrawWeaponList();
                    if (this.modifyAttackWindow.ItemsAdded)
                    {
                        this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.LastIndex);
                    }

                    break;
                case PopupButtons.AddAmmo:
                    this.modifyAmmoWindow.ShowAdd(Program.CcsFile.Character.Ammunitions);
                    this.DrawAmmoList();
                    if (this.modifyAmmoWindow.ItemsAdded)
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
                Program.Modify();

                var ammo = (Ammunition)this.AmmoDataGrid.SelectedItem;
                var index = this.AmmoDataGrid.SelectedIndex;

                Program.CcsFile.Character.Ammunitions.Remove(ammo);
                this.DrawAmmoList();
                this.AmmoDataGrid.SetSelectedIndex(index);
            }
            else if (this.WeaponDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var weapon = (Weapon)this.WeaponDataGrid.SelectedItem;
                var index = this.WeaponDataGrid.SelectedIndex;

                Program.CcsFile.Character.Weapons.Remove(weapon);
                this.DrawWeaponList();
                this.WeaponDataGrid.SetSelectedIndex(index);
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
            this.modifyArmorWindow.ShowEdit(Program.CcsFile.Character.Armor);
            this.Draw();
        }

        private void WeaponDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            Program.CcsFile.Character.Weapons.Clear();

            foreach (var weapon in this.WeaponDataGrid.Items)
            {
                Program.CcsFile.Character.Weapons.Add(weapon as Weapon);
            }
        }

        private void AmmoDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            Program.CcsFile.Character.Ammunitions.Clear();

            foreach (var ammo in this.AmmoDataGrid.Items)
            {
                Program.CcsFile.Character.Ammunitions.Add(ammo as Ammunition);
            }
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

            Program.Modify();

            var effect = (StatusEffect)this.StatusEffectsDataGrid.SelectedItem;
            var index = this.StatusEffectsDataGrid.SelectedIndex;

            Program.CcsFile.Character.StatusEffects.Remove(effect);
            this.DrawStatusEffects();
            this.StatusEffectsDataGrid.SetSelectedIndex(index);
        }

        private void AddEffectsButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyStatusEffectsWindow.ShowAdd(Program.CcsFile.Character.StatusEffects);
            this.DrawStatusEffects();

            if (this.modifyStatusEffectsWindow.ItemsAdded)
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
    }
}