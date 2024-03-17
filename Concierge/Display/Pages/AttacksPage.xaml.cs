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

    using Concierge.Character.Equipable;
    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Display.Windows.Utility;
    using Concierge.Services;
    using Concierge.Tools;

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

        private List<Weapon> WeaponDisplayList => Program.CcsFile.Character.Equipment.Weapons.Filter(this.SearchFilter.FilterText).ToList();

        public void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawWeaponList();
            this.DrawAugmentList();
            this.DrawStatusEffects();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is Augment ammunition)
            {
                var index = this.AugmentDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit(
                    ammunition,
                    typeof(AugmentationWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Attacks);
                this.DrawAugmentList();
                this.AugmentDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is Weapon weapon)
            {
                var index = this.WeaponDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit(
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
                ConciergeWindowService.ShowEdit(
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
            this.WeaponDisplayList.ForEach(weapon => this.WeaponDataGrid.Items.Add(weapon));
            this.SetWeaponDataGridControlState();
        }

        private void DrawStatusEffects()
        {
            this.StatusEffectsDataGrid.Items.Clear();
            Program.CcsFile.Character.Vitality.Status.StatusEffects.ForEach(effect => this.StatusEffectsDataGrid.Items.Add(effect));
            this.SetStatusDataGridControlState();
        }

        private void DrawAugmentList()
        {
            this.AugmentDataGrid.Items.Clear();
            Program.CcsFile.Character.Equipment.Augmentation.ForEach(augment => this.AugmentDataGrid.Items.Add(augment));
            this.SetAugmentDataGridControlState();
        }

        private void SetAugmentDataGridControlState()
        {
            this.AugmentDataGrid.SetButtonControlsEnableState(
                this.AugmentUpButton,
                this.AugmentDownButton,
                this.AugmentRecoverButton,
                this.AugmentEditButton,
                this.AugmentDeleteButton);

            if (this.AugmentDataGrid.SelectedItem is Augment augment)
            {
                DisplayUtility.SetControlEnableState(this.AugmentRecoverButton, augment.Recoverable && augment.Total < augment.Quantity);
            }
        }

        private void SetStatusDataGridControlState()
        {
            this.StatusEffectsDataGrid.SetButtonControlsEnableState(this.EditEffectsButton, this.DeleteEffectsButton);
        }

        private void SetWeaponDataGridControlState()
        {
            this.WeaponDataGrid.SetButtonControlsEnableState(
                this.AttacksUpButton,
                this.AttacksDownButton,
                this.AttacksEditButton,
                this.AttackUseButton,
                this.AttacksDeleteButton);
        }

        private void AugmentUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.AugmentDataGrid, this.DrawAugmentList, Program.CcsFile.Character.Equipment.Augmentation, 0, -1);
        }

        private void AugmentDownButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.AugmentDataGrid, this.DrawAugmentList, Program.CcsFile.Character.Equipment.Augmentation, Program.CcsFile.Character.Equipment.Augmentation.Count - 1, 1);
        }

        private void AttacksUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.WeaponDataGrid, this.DrawWeaponList, Program.CcsFile.Character.Equipment.Weapons, 0, -1);
        }

        private void AttacksDownButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.WeaponDataGrid, this.DrawWeaponList, Program.CcsFile.Character.Equipment.Weapons, Program.CcsFile.Character.Equipment.Weapons.Count - 1, 1);
        }

        private void AugmentClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.AugmentDataGrid.UnselectAll();
        }

        private void AttacksClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.WeaponDataGrid.UnselectAll();
        }

        private void AugmentAddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd(
                Program.CcsFile.Character.Equipment.Augmentation,
                typeof(AugmentationWindow),
                this.Window_ApplyChanges,
                ConciergePage.Attacks);

            this.DrawAugmentList();
            if (added)
            {
                this.AugmentDataGrid.SetSelectedIndex(this.AugmentDataGrid.LastIndex);
            }
        }

        private void AttacksAddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd(
                Program.CcsFile.Character.Equipment.Weapons,
                typeof(AttacksWindow),
                this.Window_ApplyChanges,
                ConciergePage.Attacks);

            this.DrawWeaponList();
            if (added)
            {
                this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.LastIndex);
            }
        }

        private void AugmentEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AugmentDataGrid.SelectedItem != null)
            {
                this.Edit(this.AugmentDataGrid.SelectedItem);
            }
        }

        private void AttacksEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem != null)
            {
                this.Edit(this.WeaponDataGrid.SelectedItem);
            }
        }

        private void AugmentDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AugmentDataGrid.SelectedItem is null)
            {
                return;
            }

            var augment = (Augment)this.AugmentDataGrid.SelectedItem;
            var index = this.AugmentDataGrid.SelectedIndex;

            Program.UndoRedoService.AddCommand(new DeleteCommand<Augment>(Program.CcsFile.Character.Equipment.Augmentation, augment, index, this.ConciergePage));
            Program.CcsFile.Character.Equipment.Augmentation.Remove(augment);
            this.DrawAugmentList();
            this.AugmentDataGrid.SetSelectedIndex(index);
        }

        private void AttacksDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem is null)
            {
                return;
            }

            var weapon = (Weapon)this.WeaponDataGrid.SelectedItem;
            var index = this.WeaponDataGrid.SelectedIndex;

            Program.UndoRedoService.AddCommand(new DeleteCommand<Weapon>(Program.CcsFile.Character.Equipment.Weapons, weapon, index, this.ConciergePage));
            Program.CcsFile.Character.Equipment.Weapons.Remove(weapon);
            this.DrawWeaponList();
            this.WeaponDataGrid.SetSelectedIndex(index);
        }

        private void WeaponDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.WeaponDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Equipment.Weapons, this.ConciergePage);
        }

        private void AugmentDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.AugmentDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Equipment.Augmentation, this.ConciergePage);
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(StatusEffectsWindow):
                    this.DrawStatusEffects();
                    ScrollDataGrid(this.StatusEffectsDataGrid);
                    break;
                case nameof(AugmentationWindow):
                    this.DrawAugmentList();
                    ScrollDataGrid(this.AugmentDataGrid);
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
            if (this.StatusEffectsDataGrid.SelectedItem is null)
            {
                return;
            }

            var effect = (StatusEffect)this.StatusEffectsDataGrid.SelectedItem;
            var index = this.StatusEffectsDataGrid.SelectedIndex;

            Program.UndoRedoService.AddCommand(new DeleteCommand<StatusEffect>(Program.CcsFile.Character.Vitality.Status.StatusEffects, effect, index, this.ConciergePage));
            Program.CcsFile.Character.Vitality.Status.StatusEffects.Remove(effect);
            this.DrawStatusEffects();
            this.StatusEffectsDataGrid.SetSelectedIndex(index);
        }

        private void AddEffectsButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd(
                Program.CcsFile.Character.Vitality.Status.StatusEffects,
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
            this.StatusEffectsDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Vitality.Status.StatusEffects, this.ConciergePage);
        }

        private void AttackDataGrid_Filtered(object sender, RoutedEventArgs e)
        {
            this.SearchFilter.SetButtonEnableState(this.AttacksUpButton);
            this.SearchFilter.SetButtonEnableState(this.AttacksDownButton);

            this.DrawWeaponList();
        }

        private void AttackUseButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem is null)
            {
                return;
            }

            var ammunition = this.AugmentDataGrid.SelectedItem as Augment;
            var weapon = (Weapon)this.WeaponDataGrid.SelectedItem;
            var result = weapon.Use(new UseItem(ammunition, 0));

            var windowResult = ConciergeWindowService.ShowUseItemWindow(typeof(UseItemWindow), result);

            if (ammunition is not null && windowResult == ConciergeResult.OK)
            {
                var index = this.AugmentDataGrid.SelectedIndex;
                var oldItem = ammunition.DeepCopy();

                ammunition.Use(UseItem.Empty);
                this.DrawAugmentList();
                this.AugmentDataGrid.SetSelectedIndex(index);

                Program.UndoRedoService.AddCommand(new EditCommand<Augment>(ammunition, oldItem, this.ConciergePage));
            }
        }

        private void AugmentRecoverButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AugmentDataGrid.SelectedItem is null)
            {
                return;
            }

            var augment = (Augment)this.AugmentDataGrid.SelectedItem;
            var index = this.AugmentDataGrid.SelectedIndex;
            var oldItem = augment.DeepCopy();

            augment.Recover();
            this.DrawAugmentList();
            this.AugmentDataGrid.SetSelectedIndex(index);

            Program.UndoRedoService.AddCommand(new EditCommand<Augment>(augment, oldItem, this.ConciergePage));
        }

        private void AugmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetAugmentDataGridControlState();
        }

        private void StatusEffectsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetStatusDataGridControlState();
        }

        private void WeaponDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetWeaponDataGridControlState();
        }
    }
}
