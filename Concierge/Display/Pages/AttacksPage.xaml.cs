// <copyright file="AttacksPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Equipable;
    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Common;
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
    public partial class AttacksPage : ConciergePage
    {
        private readonly MultiSelectService multiSelectService;

        public AttacksPage()
        {
            this.InitializeComponent();

            this.HasEditableDataGrid = true;
            this.ConciergePages = ConciergePages.Attacks;
            this.MultiSelectButton.Initialize(ConciergeBrushes.DarkPink);
            this.multiSelectService = new MultiSelectService(
                this.AugmentUpButton,
                this.AugmentDownButton,
                this.AugmentRecoverButton,
                this.AugmentEditButton,
                this.AugmentDeleteButton,
                this.MultiSelectButton,
                this.AugmentDataGrid);
        }

        private delegate void DrawList();

        private List<Weapon> WeaponDisplayList => [.. Program.CcsFile.Character.Equipment.Weapons.Filter(this.SearchFilter.FilterText)];

        public override void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawWeaponList();
            this.DrawAugmentList();
            this.DrawStatusEffects();
        }

        public override void Edit(object itemToEdit)
        {
            if (itemToEdit is Augment ammunition)
            {
                var index = this.AugmentDataGrid.SelectedIndex;
                WindowService.ShowEdit(ammunition, typeof(AugmentationWindow), this.Window_ApplyChanges, ConciergePages.Attacks);
                this.DrawAugmentList();
                this.AugmentDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is Weapon weapon)
            {
                var index = this.WeaponDataGrid.SelectedIndex;
                WindowService.ShowEdit(weapon, typeof(AttacksWindow), this.Window_ApplyChanges, ConciergePages.Attacks);
                this.DrawWeaponList();
                this.WeaponDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is StatusEffect statusEffect)
            {
                var index = this.StatusEffectsDataGrid.SelectedIndex;
                WindowService.ShowEdit(statusEffect, typeof(StatusEffectsWindow), this.Window_ApplyChanges, ConciergePages.Attacks);
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
            var index = dataGrid.NextItem(list, limit, increment, this.ConciergePages);

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
            this.multiSelectService.SetControlState();
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

        private List<Augment> GetActiveAugments()
        {
            var items = new List<Augment>();
            if (!this.MultiSelectButton.IsChecked ?? false)
            {
                if (this.AugmentDataGrid.SelectedItem is Augment augment)
                {
                    items.Add(augment);
                }
            }
            else
            {
                foreach (var item in this.AugmentDataGrid.SelectedItems)
                {
                    if (item is Augment augment)
                    {
                        items.Add(augment);
                    }
                }
            }

            return items;
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
            var added = WindowService.ShowAdd(
                Program.CcsFile.Character.Equipment.Augmentation,
                typeof(AugmentationWindow),
                this.Window_ApplyChanges,
                ConciergePages.Attacks);

            this.DrawAugmentList();
            if (added)
            {
                this.AugmentDataGrid.SetSelectedIndex(this.AugmentDataGrid.LastIndex);
            }
        }

        private void AttacksAddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = WindowService.ShowAdd(
                Program.CcsFile.Character.Equipment.Weapons,
                typeof(AttacksWindow),
                this.Window_ApplyChanges,
                ConciergePages.Attacks);

            this.DrawWeaponList();
            if (added)
            {
                this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.LastIndex);
            }
        }

        private void AugmentEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AugmentDataGrid.SelectedItem is not null)
            {
                this.Edit(this.AugmentDataGrid.SelectedItem);
            }
        }

        private void AttacksEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem is not null)
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

            Program.UndoRedoService.AddCommand(new DeleteCommand<Augment>(Program.CcsFile.Character.Equipment.Augmentation, augment, index, this.ConciergePages));
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

            Program.UndoRedoService.AddCommand(new DeleteCommand<Weapon>(Program.CcsFile.Character.Equipment.Weapons, weapon, index, this.ConciergePages));
            Program.CcsFile.Character.Equipment.Weapons.Remove(weapon);
            this.DrawWeaponList();
            this.WeaponDataGrid.SetSelectedIndex(index);
        }

        private void WeaponDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.WeaponDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Equipment.Weapons, this.ConciergePages);
        }

        private void AugmentDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.AugmentDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Equipment.Augmentation, this.ConciergePages);
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

            Program.UndoRedoService.AddCommand(new DeleteCommand<StatusEffect>(Program.CcsFile.Character.Vitality.Status.StatusEffects, effect, index, this.ConciergePages));
            Program.CcsFile.Character.Vitality.Status.StatusEffects.Remove(effect);
            this.DrawStatusEffects();
            this.StatusEffectsDataGrid.SetSelectedIndex(index);
        }

        private void AddEffectsButton_Click(object sender, RoutedEventArgs e)
        {
            var added = WindowService.ShowAdd(
                Program.CcsFile.Character.Vitality.Status.StatusEffects,
                typeof(StatusEffectsWindow),
                this.Window_ApplyChanges,
                ConciergePages.Attacks);
            this.DrawStatusEffects();

            if (added)
            {
                this.StatusEffectsDataGrid.SetSelectedIndex(this.StatusEffectsDataGrid.LastIndex);
            }
        }

        private void EditEffectsButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.StatusEffectsDataGrid.SelectedItem is null)
            {
                return;
            }

            this.Edit(this.StatusEffectsDataGrid.SelectedItem);
        }

        private void StatusEffectsDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.StatusEffectsDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Vitality.Status.StatusEffects, this.ConciergePages);
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

            var augmentation = this.GetActiveAugments();
            var weapon = (Weapon)this.WeaponDataGrid.SelectedItem;
            var result = weapon.Use(new UseItem(0, [.. augmentation]));

            var windowResult = WindowService.ShowUseItemWindow(typeof(UseItemWindow), result);

            if (!augmentation.IsEmpty() && windowResult == ConciergeResult.OK)
            {
                var commands = new List<Command>();
                foreach (var augment in augmentation)
                {
                    if (augment.Recoverable)
                    {
                        var oldItem = augment.DeepCopy();
                        augment.Use(UseItem.Empty);
                        commands.Add(new EditCommand<Augment>(augment, oldItem, this.ConciergePages));
                    }
                }

                this.DrawAugmentList();

                Program.UndoRedoService.AddCommand(new CompositeCommand(this.ConciergePages, [.. commands]));
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

            Program.UndoRedoService.AddCommand(new EditCommand<Augment>(augment, oldItem, this.ConciergePages));
        }

        private void AugmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.MultiSelectButton.IsChecked ?? false)
            {
                e.Handled = true;
            }

            this.multiSelectService.SetControlState();
        }

        private void StatusEffectsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetStatusDataGridControlState();
        }

        private void WeaponDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetWeaponDataGridControlState();
        }

        private void MultiSelectButton_Checked(object sender, RoutedEventArgs e)
        {
            this.multiSelectService.Check();
        }

        private void MultiSelectButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.multiSelectService.Uncheck();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Collapsed)
            {
                this.MultiSelectButton.UnCheck();
            }
        }

        private void AugmentDataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is not UIElement element || (!this.MultiSelectButton.IsChecked ?? true))
            {
                return;
            }

            var row = DisplayUtility.FindVisualParent<DataGridRow>(element);
            if (e.LeftButton == MouseButtonState.Pressed && row is not null)
            {
                row.IsSelected = !row.IsSelected;
                e.Handled = true;
            }
        }
    }
}
